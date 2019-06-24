# SK-AltInjector

Work in progress alternative companion injector for Special K. Works by allowing the user to manually specify the process Special K should get loaded into, by selecting its window title through a tray icon. Requires Special K installed.

**Note that as Special K is delay injected into the target process, certain features might not be available** (e.g. flip model presentation etc). It is recommended to install a wrapper DLL (hold CTRL+Shift when clicking on the window title) to enable such functionality.


# Instructions

Due to how Special K internally works through the use of a whitelist, users must first properly ensure that the desired games are whitelisted on said whitelist before manually injecting Special K into them. Failure to do this will cause the Special K DLL file to go into its idle state, and not properly boot up after injection.

1. Navigate to \Documents\My Mods\SpecialK\Global\ and create a file called **whitelist.ini**
2. Within this file, specify a part of the path to the game(s) Special K will be manually loaded into.
   * For example specify "Games" on its own on a line to allow injection into all games that are installed in a location that containes "Games" somewhere within it.
   * Similarly, specify "WindowsApps" to allow injection into UWP/Microsoft Store based titles.
     * Note that support for UWP based games are minimal at the moment.
3. Save and close the file.
4. Download this tool from the [Releases](https://github.com/Idearum/SK-AltInjector/releases) section.
5. Launch this injector and use the icon in the notification area to inject Special K into the game.


# Tips and tricks

* Special K's compatibility menu is still accessible if holding down Ctrl+Shift when clicking on a window in the list. This menu will either allow you to re-configure what API Special K will use, or act as a shortcut to install wrapper DLLs or reset the config for the injected game.

* Special K have been confirmed working (although with reduced functionality) for both Void Bastards and Prey on the Microsoft Store when injected in this capacity. A lot of other UWP based titles will not work.


# Future ideas and/or plans

* Hotkey for injecting Special K into the focused application without accessing the tray icon. E.g. clicking Ctrl+Shift+F1 (or something similar) would trigger this tool to look the focused application up and inject Special K's DLL file into said process.

* Automatically add relevant path to whitelist.ini when selecting a window in the list to inject into.

* Easy access to manual tweaking whitelist.ini.

* Better handling of the window list in general (more informative, possibly better filtered, etc).


# Credits

* [Special K](https://gitlab.com/Kaldaien/SpecialK/) for doing the heavy lifting here. This tiny companion piece is basic as hell when compared to the versatility of Special K that makes this companion piece possible. 

* Kudos to Nefarius and their [Injector](https://github.com/nefarius/Injector) which showed me this was possible.

* [Pokeball icon](https://www.iconfinder.com/icons/1337537/game_go_play_pokeball_pokemon_icon) from [Roundicons.com](https://roundicons.com/).
