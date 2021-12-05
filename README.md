# TeleportHotkey - Teleport Using Hotkey #

## [ Compatibility ] ##
* Graveyard Keeper 1.404
* Tested with All DLC Installed
    * Should work without DLC too

## [ Compilation ] ##

### [ Instructions ] ###

To compile this project, it requires several references. Because these are linked to a specific location on the system where the compilation is done. 

So to make it work on your system you must most likely alter these references. Or being more precise: alter the location of where the referenced DDL-files are stored, being `%game_path%\Graveyard Keeper_Data\Managed`.

The game specific DLL's that are referenced in this mod are:

* Assembly-CSharp.dll
* Assembly-CSharp-firstpass.dll
* SmartBearAssembly.dll
* UnityEngine.dll
* UnityEngine.CoreModule.dll
* UnityEngine.InputLegacyModule.dll
* UnityEngine.UI.dll
* 0Harmony.dll 
  
The last one will only be present **after** QModManager is installed, the rest are included with the game itself.

Assuming that you are using Visual Studio to compile this project, you can just:
* Go to the `Solution Explorer`
* Right click the `References` item (located under the `TeleportHotkey` project)
* Click `Add Reference`
* Click `Browse`
* Navigate to the `%game_path%\Graveyard Keeper_Data\Managed\` folder
* Add all the DLL's specified above. (you can use `ctrl` to select multiple files at once)
* Click `Ok` to confirm adding the files
* This will in fact not re-add the same references, but rather update the paths of the ones that were already listed
* Once this is done, you should be able to build the project
* This will result in a new compiled .DLL file, called `TeleportHotkey.dll`, located in:
  - `%project_path%/bin/Debug` - when running a Debug build 
  - `%project_path%/bin/Release` - when running a Release build 

### [ Output ] ###
  * Compiles to `%repo_path%/TeleportHotkey/bin/debug/TeleportHotkey.dll`

For the proper functioning of the mod, three additional files are required, which can be found in `TeleportHotkey/files/`:

### [ Additional files ] ###

* `mod.json`, a file required for QModManager, containing version info among others.
* `config.txt`, can also be automatically generated (see **`Installation`**) 
* `Alias.txt` (see **`Installation`**) 

## [ Installation ] ##

1. Make sure QModManager is [installed and configured](https://www.nexusmods.com/graveyardkeeper/mods/1) properly.
2. Create a new directory named `TeleportHotkey` in the `Qmods` directory in your Graveyard Keepers.
3. Copy all (either 3 or 4) files mentioned in the previous section to the that directory (`%game_path%\QMods\TeleportHotkey\`).
4. If you decided to copy the config.txt file manually, the mod will now work with the default values.

## [ Configuration ] ##
Configuration of the mod can be done by manually changing the configuration values in the `config.txt` file. 


## [ Credits ] ##
[mdn522](https://github.com/mdn522)