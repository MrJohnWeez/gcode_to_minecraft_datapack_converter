# Gcode -> Minecraft Datapack Converter

[Downloads](https://github.com/MrJohnWeez/gcode_to_minecraft_datapack_converter/releases)

## Minecraft World and Datapack Examples

View the [3D_Printer_Emulator_In_Minecraft](https://github.com/MrJohnWeez/3D_Printer_Emulator_In_Minecraft/releases?raw=true "3D_Printer_Emulator_In_Minecraft") repository to download a Minecraft save file for java edition. This contains the datapack examples from which were generated using this program

## Installation Instructions

1. [Download Gcode_To_Minecraft_Datapack_Converter_vX.X.zip](https://github.com/MrJohnWeez/gcode_to_minecraft_datapack_converter/releases)
2. You may need to press Keep when using Chrome [See why](https://support.google.com/webmasters/thread/23193211?hl=en&msgid=24301018)

    ![Ignore Warning](Resources\Images\Screenshots\Errors\DownloadWarning.png)
3. Run program
   1. Use program within .zip
      1. Double click on **Gcode_To_Minecraft_Datapack_Converter_vX.X.zip**
      2. Double click on **Gcode_To_Minecraft_Datapack_Converter.exe**
   2. Copy files to another folder
      1. Right click **Gcode_To_Minecraft_Datapack_Converter_vX.X.zip**
      2. Select Extract to **Gcode_To_Minecraft_Datapack_Converter_vX.X**
      3. Double click on **Gcode_To_Minecraft_Datapack_Converter.exe** within the created folder

## How To Use

![Help Guide](Resources\Images\Screenshots\Gcode_To_Datapack_Converter\HelpGuide.png?raw=true "Help Guide")

1. Select the Gcode file you want to convert
2. Select where you want the datapack to be saved to
3. Select what Minecraft blocks to print with
4. Enter a custon name for your datapack or leave blanc for the default name
5. Change the speed at whitch the print will run at (Higher speeds may miss a few blocks)
6. Generate the Minecraft Datapack
7. Link to the downloads section of the repository

## How To Create Optimized Gcode Files

The Gcode -> Datapack converter program will emulate every move within Minecraft exactly like a 3D printer would

Since the average 3d printer has a nozzle size of about 0.4mm and a layer hight of 0.15mm but the conversion program converts 1mm to 1 Minecraft block, there are wasted gcode lines since fraction of blocks cannot be placed in a minecraft world.

To solve this you can

- [Use PrusaSlicer](Conversion%20Examples\Using_PrusaSlicer.md) and inport the Minecraft_config.ini configeration file
- [Configure your own Slicer Program](Conversion%20Examples\Slicer_Config_Settings.md)