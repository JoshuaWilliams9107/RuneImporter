# Rune Importer
An Auto-Importer for Runes for the game League of Legends that scrapes rune data off of the internet.
## - About the Project -
I created this project because I played League of Legends way too much.  However, my main issue with the game was that every time I chose a champion I would have to open chrome, type in champion.gg or u.gg and look up the current runes that are best for that champion.  Then I would have to manually click them into the league client.  This process took about 1-2 minutes every time I wanted to play a game of LoL.  So I came up with the idea to have a program that would do this process automatically.
<br/><br/>
This program automatically scrapes rune, spell order, and item build data off of champion.gg for your desired champion and position using XML data generated by the HTML Agility Pack.  That data can then automatically be imported into your league client by running a auto-clicker built into the program.

## - Built With -
 * [.NET Framework](https://dotnet.microsoft.com/)
 * [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
 * [HTML Agility Pack](https://html-agility-pack.net/)
 
## - Installation -
1. Download Release.zip located inside the Release Folder
2. Extract contents of zip into a directory

## - Usage -
* Run Rune Importer.exe
* Type in your champion name (position name is optional)
* Click "Import from Champion.gg" or hit Enter
* Ensure that your league of legends client is in the foreground and that your are in champion select.
* Click Import into League Client

## - Roadmap -
* Have program detect when league of legends is not open and have it not take over the mouse if it does not see league.
* Allow for import from multiple websites
* Fix most frequent build button and most frequent rune button



### - Warning -
This Program takes control of your computer's mouse upon clicking "Import into League Client", use with caution.  
