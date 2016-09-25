# monodevelop-elementaryui
MonoDevelop UI Adapter for elementary OS

# Build it yourself
Using MonoDevelop open the .sln file, compile.    
Then open a command line window, change to the project directory binary outputs (monodevelop-elementary-plugin/bin/Debug/) and execute:   
    mdtool setup pack monodevelop-elementary-plugin.dll  
Then you should have a .mpack file you can install using the MonoDevelop Add-in manager GUI *install from file* option.

# A bug
To work as expected, when the popup menu is shown click on the option, 
for some reason just hovering opens the submenu but the selection made after 
does not execute the clicked item function.

**So, for example:**  
If Open popup > Hover 'Help' > Click 'About' then nothing happens.  
but  
If Open popup > Click 'Help' > Click 'About' then the about dialog appears.  
If you know what is happening here comment or send a PR.