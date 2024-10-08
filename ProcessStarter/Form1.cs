using System.Diagnostics;
using System.Reflection;

namespace ProcessStarter;

public partial class Form1 : Form
{
    private const string DefaultConfig = "ProcessStarter.process_starter.cfg";
    
    private readonly string config = $"{AppDomain.CurrentDomain.BaseDirectory}\\process_starter.cfg";
    private Size defaultButtonSize = new(130, 25);
    
    private new const int Padding = 10;
    private int currentYLevel = Padding;
    
    private int lowestButtonBottom = Padding;
    private int rightMostX = Padding;

    
    public Form1()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        ClientSize = Size.Empty;
        try
        {
            LoadButtons();
            ClientSize = new Size(rightMostX + Padding, lowestButtonBottom + Padding);
        }
        catch (FileNotFoundException ex)
        {
            Visible = false;
            var result = MessageBox.Show("Missing Config, autogenerated template.\nClick OK to open config.", "Missing Config");
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(DefaultConfig);
            if (stream == null)
            {
                Console.WriteLine("Resource not found.");
                return;
            }

            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            File.WriteAllText(config, content);
            
            if (result == DialogResult.OK)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = config,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            OnLoad(e);
        }
        catch (Exception)
        {
            Visible = false;
            var result = MessageBox.Show("Config is either corrupt or improperly setup.\nClick OK to open config.", "Corrupt Config");
            if (result == DialogResult.OK)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = config,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            Environment.Exit(2);
        }
    }

    private void LoadButtons()
    {
        var lines = File.ReadAllLines(config);
        var x = Padding;
        var y = Padding;

        var height = 1;
        var width = 1;
        var spacing = 0;
        var rowSpacing = 0;
        var sameLine = false;
        ButtonData? buttonData = null;
        
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            
            if (line.Split('#')[0].Trim() == "-Begin Button-")
            {
                // Add the previous button to the form, if it exists
                if (buttonData != null) AddButton(ref x, ref y, ref width, ref height, ref spacing, ref rowSpacing, ref sameLine, buttonData);

                // Initialize a new ButtonData for the next button
                buttonData = new ButtonData(this);
                continue;
            }
            
            

            // Parse key-value pairs from the config
            var split = line.Split('=');
            if (split.Length >= 2)
            {
                var key = split[0].Trim();
                var value = split[1].Split('#')[0].Trim();

                if (key == "Form_Title")
                {
                    Text = value;
                    continue;
                }

                if (key == "button_size")
                {
                    var size = value.Split(',');
                    defaultButtonSize = new Size(int.Parse(size[0].Trim()), int.Parse(size[1].Trim()));
                }



                // Use a dictionary to handle properties
                var propertyHandlers = new Dictionary<string, Action<string>>
                {
                    { "text", v => buttonData?.SetText(v) },
                    { "process", v => buttonData?.SetProcess(v) },
                    { "command", v => buttonData?.SetCommand(v) },
                    { "spacing", v => spacing = int.Parse(v) },
                    { "row_spacing", v => rowSpacing = int.Parse(v) },
                    { "width", v => width = int.Parse(v) },
                    { "height", v => height = int.Parse(v) },
                    { "text_size", v => buttonData?.SetTextSize(int.Parse(v)) },
                    { "bold", v => buttonData?.SetBold(v.ToLower().Equals("true")) },
                    { "italic", v => buttonData?.SetItalic(v.ToLower().Equals("true")) },
                    { "same_line", v => sameLine = v.ToLower().Equals("true") },
                };

                if (propertyHandlers.ContainsKey(key))
                {
                    propertyHandlers[key](value);
                }
            }
        }

        // Add the final button to the form
        if (buttonData != null) AddButton(ref x, ref y, ref width, ref height, ref spacing, ref rowSpacing, ref sameLine, buttonData);
        
        Console.WriteLine(rowSpacing);
    }
    
    
    private void AddButton(ref int x, ref int y, ref int width, ref int height, ref int spacing, ref int rowSpacing, ref bool sameLine, ButtonData buttonData)
    {
        var size = defaultButtonSize;
        if (width < 1) width = 1;
        if (height < 1) height = 1;

        if (width > 1 || height > 1) size = GetScaledSize(width, height);

        // Set the button location and size
        buttonData.SetSize(size);
        
        y += rowSpacing * defaultButtonSize.Height;
        rowSpacing = 0;
        if (sameLine)
        {
            x += defaultButtonSize.Width * spacing;         
            buttonData.SetLocation(new Point(x, currentYLevel));
        }
        else
        {
            x = Padding;
            x += defaultButtonSize.Width * spacing;
            buttonData.SetLocation(new Point(x, y));
            currentYLevel = y;
            y += size.Height;
        }

        x += size.Width;

        // Track the lowest button's bottom position
        var buttonBottom = y;
        if (buttonBottom > lowestButtonBottom)
        {
            lowestButtonBottom = buttonBottom;
        }

        // Track the right-most X position
        if (x > rightMostX)
        {
            rightMostX = x;
        }

        // Add the button to the form and reset the style
        buttonData.AddToControl();
        width = 1;
        height = 1;
        spacing = 0;
        sameLine = false;
    }

    private Size GetScaledSize(int width, int height)
    {
        var x = defaultButtonSize.Width * width;
        var y = defaultButtonSize.Height * height;
        return new Size(x, y);
    }
}
