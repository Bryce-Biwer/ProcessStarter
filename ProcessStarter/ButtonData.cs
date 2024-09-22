using System.Diagnostics;

namespace ProcessStarter;

public class ButtonData
{
    private Button Button { get; set; }
    private string Command { get; set; } = "";
    private string ProcessExe { get; set; } = "";
    private int TextSize { get; set; } = 8;
    private bool Bold { get; set; }
    private bool Italic { get; set; }
    private Form1 MainForm { get; set; }

    public bool IsPartOfControl { get; private set; }
    public ButtonData(Form1 mainForm)
    {
        MainForm = mainForm;
        Button = new Button();
        Button.Click += ExecuteCommand;
    }

    public void SetText(string text)
    {
        Button.Text = text;
    }

    public void SetBold(bool bold)
    {
        Bold = bold;
    }

    public void SetItalic(bool italic)
    {
        Italic = italic;
    }

    public void SetTextSize(int size)
    {
        TextSize = size;
    }

    public void SetProcess(string process)
    {
        ProcessExe = process.Replace("{CurrentDirectory}", AppDomain.CurrentDomain.BaseDirectory);
    }

    public void SetCommand(string command)
    {
        Command = command;
    }

    public void SetLocation(Point location)
    {
        Button.Location = location;
    }

    public void SetSize(Size size)
    {
        Button.Size = size;
    }

    public void AddToControl()
    {
        if(IsPartOfControl) return;
        var styleFlag = FontStyle.Regular;
        if (Bold && Italic) styleFlag = FontStyle.Bold | FontStyle.Italic;
        else if (Bold) styleFlag = FontStyle.Bold;
        else if (Italic) styleFlag = FontStyle.Italic;
        
        Button.Font = new Font(Button.Font.FontFamily, TextSize, styleFlag);
        MainForm.Controls.Add(Button);
        IsPartOfControl = true;
    }

    private void ExecuteCommand(object? sender, EventArgs eventArgs)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = string.IsNullOrEmpty(ProcessExe) ? "cmd.exe" : ProcessExe,
            WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
            Arguments = string.IsNullOrEmpty(ProcessExe) ? $"/K {Command}" : Command,
            RedirectStandardInput = false,
            RedirectStandardOutput = false,
            UseShellExecute = true,
            CreateNoWindow = false
        };

        Process.Start(startInfo);

    }

    public string ButtonText => Button.Text;

}