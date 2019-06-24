# SK-AltInjector

Work in progress alternative injector for Special K. Works by allowing the user to manually specify the process Special K should get loaded into, by selecting its window title through a tray icon. Requires Special K installed.

**Note** that as Special K is delay injected into the target process, certain features might not be available (e.g. flip model presentation etc).


# Instructions

Due to how Special K internally works through the use of a whitelist, users must first properly ensure that the desired games are whitelisted on said whitelist before manually injecting Special K into them. Failure to do this will cause the Special K DLL file to go into its idle state, and not properly boot up after injection.

1. Navigate to \Documents\My Mods\SpecialK\Global\ and create a file called **whitelist.ini**
2. Within this file, specify a part of the path to the game(s) Special K will be manually loaded into.
   * For example specify "Games" on its own on a line to allow injection into all games that are installed in a location that containes "Games" somewhere within it.
   * Similarly, specify "WindowsApps" to allow injection into UWP/Microsoft Store based titles.
     * Note that support for UWP based games are minimal at the moment.
3. Save and close the file.
4. Download this tool from the [https://github.com/Idearum/SK-AltInjector/releases Releases] section.
5. Launch this injector and use the icon in the notification area to inject Special K into the game.


# Tips and tricks

* Special K's compatibility menu (Ctrl+Shift) is still accessible if holding down Ctrl+Shift when clicking on a window in the list. This menu will either allow you to re-configure what API Special K will use, or act as a shortcut to install wrapper DLLs for the injected game.

* Special K have been confirmed working (although with reduced functionality) for both Void Bastards and Prey on the Microsoft Store when injected in this capacity. A lot of other UWP based titles will not work.
