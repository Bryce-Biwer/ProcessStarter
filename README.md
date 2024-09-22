# Process Starter

## Overview
Process Starter is a Windows Forms application that allows users to configure and launch processes or commands via customizable buttons. The configuration is handled through a simple text file, making it easy to set up and modify.

## Features
- Dynamic button creation based on a configuration file.
- Support for customizing button text, size, and styles (bold, italic).
- Ability to run executables or console commands.
- Configurable layout options, including spacing and row spacing.

## Configuration File
The configuration file `process_starter.cfg` defines the buttons and their properties. Here’s how to structure it:

### Config Instructions
ProcessStart config file
Text such as this will be ignored.
To define a window title use Form_Title = name
To define a button use -Begin Button-

Buttons have the following properties:
text - The text displayed on the button
text_size - the size of text, (8 is the default)
bold - bolds the text
italic - italicize the text

button_size - this changes the size for the current button and all buttons afterwards
			must be defined as width, height

process - this can be any file your computer can run
command - if process is empty this will run as a console command 
	otherwise it will act as commandline arguments for process

same_line - used to make button appear on the line of the previous button

spacing - creates an x amount of blank spaces the width of default button size
row_spacing - creates an x amount of blank spaces the height of default button size
		note: negative row spacing is allowed, but can have messy results

### Example Config
Set a window title
```
Form_Title = Example Title
```
These will apear vertical which is the default
```
-Begin Button- # comments on rows with commands need a hashtag
text = Button 1

-Begin Button-
text = Button 2 
```

```
-Begin Button-
text = Button 1
row_spacing = 1 # creates a blank line

-Begin Button-
text = Button 2 
same_line = true # will appear next to Button 1
```

```
-Begin Button-
row_spacing = 1 # creates a blank line
text = Button 1
italic = true
width = 3
height = 2

-Begin Button-
text = Button 2

-Begin Button-
text = Button 3
spacing = 1 # creates a blank space
same_line = true
```

```
-Begin Button-
text = Button 1
row_spacing = 1

-Begin Button-
row_spacing = 1
text = Button 2
bold = true
width = 2
height = 2
same_line = true

-Begin Button- # Because Button 2 is twice the size we have this blank spot under Button 2
text = Button 3

-Begin Button-
text = Button 4
same_line = true
```

```
-Begin Button-
text = Button 1
row_spacing = 1
button_size = 100, 25 # This effects this button and all buttons afterwards

-Begin Button-
row_spacing = 1
text = Button 2
bold = true
italic = true
text_size = 15
width = 2
height = 2
same_line = true

Adding a button here and adding same_line tag would put it above button 4

-Begin Button-
text = Button 3
row_spacing = -1 	# This goes up a line, same line buttons may be covered by Button 2

-Begin Button-
text = Button 4
same_line = true
spacing = 2 # To fix this appearing behind Button 2 we can use spacing
```

## Getting Started
1. Clone this repository to your local machine.
2. Create a configuration file named `process_starter.cfg` in the same directory as the executable.
3. Run the application, and the buttons defined in the configuration will appear.

## Building the Project
To build the project, use the following steps:
1. Open the solution in Visual Studio.
2. Restore NuGet packages if necessary.
3. Build the solution.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.

## Contributions
Contributions are welcome! Please feel free to submit issues or pull requests.
