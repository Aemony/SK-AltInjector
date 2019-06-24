# SK-AltInjector

Work in progress alternative injector for Special K. Works by allowing the user to manually specify the process Special K should get loaded into, by selecting its window title through a tray icon. Requires Special K installed.


# Instructions

Due to how Special K internally works through the use of a whitelist, users must first properly ensure that the desired games are whitelisted on said whitelist before manually injecting Special K into them. Failure to do this will cause the Special K DLL file to go into its idle state, and not properly boot up after injection.

1. Navigate to \Documents\My Mods\SpecialK\Global\ and create a file called **whitelist.ini**
2. Within this file, specify a part of the path to the game(s) Special K will be manually loaded into.
   * For example specify "Games" on its own on a line to allow injection into all games that are installed in a location that containes "Games" somewhere within it.
   * Similarly, specify "WindowsApps" to allow injection into UWP/Microsoft Store based titles.
     * Note that support for UWP based games are minimal at the moment.
3. Save and close the file.
4. Launch this injector and use the icon in the notification area to inject Special K into the game.
