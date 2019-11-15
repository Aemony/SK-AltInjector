# SK-TinyInjector

Work in progress alternative companion injector for Special K, where injection is performed either through a global hotkey (Alt+X, if enabled) or a list of windows in the traybar icon menu.

Requires the global injector of [Special K](https://steamcommunity.com/groups/SpecialK_Mods/discussions/0/) installed.


# Instructions

1. Use [the ClickOnce install package](https://github.com/Idearum/SK-AltInjector/raw/master/64bitMainApp/publish/setup.exe) to install the application.

2. After the installation, SK-TinyInjector should automatically start and its features be accessible through its tray icon (the pokeball 😃).


# Instructions - OLD

0. Ensure that global injection of Special K and SKIM (Special K Install Manager) is installed, but not enabled nor running.

1. Download and extract this tool from the [Releases](https://github.com/Idearum/SK-AltInjector/releases) section.

2. Launch **SK-TinyInjector.exe**

3. Right click on its tray icon (the pokeball 😃) and enable the desired settings:
   * **Keyboard shortcut (Alt+X)** - Allows the use of Alt+X to inject Special K's DLL files into the focus/active window.
   * **Whitelist automatically** - Automatically adds the focused/active window to Special K's whitelist.ini file before loading the DLL files into the process.
   
4. Use **Alt+X** to automatically inject SK into the active window.


### Limited functionality

**Note that as Special K is delay injected into the target process, certain features might not be available** (e.g. flip model presentation etc). It is recommended to install a wrapper DLL to enable such functionality. A wrapper DLL can be installed through one of the following ways:
* Inject Special K by clicking Ctrl+Shift+Alt+X when the game have focus, which will trigger Special K's compatibility menu to open where an option to install a wrapper DLL is available.
* Use the option in Special K's in-game control panel, File -> *Install Wrapper DLL for this game*.


### Whitelist automatically

By default Special K is only configured to fully initialize itself into Steam games. To allow it to also initalize in non-Steam games, right click on the SK-TinyInjector tray icon and select Settings > **Whitelist automatically**. Now SK-TinyInjector will automatically update the whitelist.ini file to include the active/focused process when performing an injeciton.
  * Another option is to select **Edit whitelist.ini** and manually specify a part of the path to the game(s) Special K will be manually loaded into.
    * For example specify "Games" on its own on a line to allow injection into all games that are installed in a location that containes "Games" somewhere within it.
    * Similarly, specify "WindowsApps" to allow injection into UWP/Microsoft Store based titles.
     * Note that support for UWP based games is minimal at the moment.
     * Note that if **Whitelist automatically** is enabled, a restart of SK-TinyInjector is necessary for the tool to properly take into account manual edits of the whitelist file.
 

# Tips and tricks

* Special K's compatibility menu is still accessible if holding down Ctrl+Shift when clicking on a window in the list, or when clicking Alt+X (so Ctrl+Shift+Alt+X) to inject into the active window. This menu will either allow you to re-configure what API Special K will use, or act as a shortcut to install wrapper DLLs or reset the config for the injected game.

* Special K have been confirmed working (although with reduced functionality) for both Void Bastards and Prey on the Microsoft Store when injected in this capacity. A lot of other UWP based titles will not work.


# Future ideas and/or plans

* Better handling of the window list in general (more informative, possibly better filtered, etc).


# Credits

* [Special K](https://gitlab.com/Kaldaien/SpecialK/) for doing the heavy lifting here. This tiny companion piece is basic as hell when compared to the versatility of Special K that makes this companion piece possible. 

* Kudos to Nefarius and their [Injector](https://github.com/nefarius/Injector) which showed me this was possible.

* [Pokeball icon](https://www.iconfinder.com/icons/1337537/game_go_play_pokeball_pokemon_icon) from [Roundicons.com](https://roundicons.com/).
