// Created by MrJohnWeez
// March 2020
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the dropdowns for the program
/// </summary>
public class DropdownManager : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown _printMaterial = null;
	[SerializeField] private TMP_Dropdown _printBedMaterial = null;

	void Start()
	{
		List<TMP_Dropdown.OptionData> newOptions = GenerateOptions();
		_printMaterial.AddOptions(newOptions);
		_printBedMaterial.AddOptions(newOptions);

		_printMaterial.SetValueWithoutNotify(240);
		_printBedMaterial.SetValueWithoutNotify(17);
	}

	#region Gets
	/// <summary>
	/// Gets the print bed minecraft block id string
	/// </summary>
	/// <returns>Minecraft block string</returns>
	public string GetPrintBedMaterial()
	{
		return _printBedMaterial.options[_printBedMaterial.value].text;
	}

	/// <summary>
	/// Gets the print material minecraft block id string
	/// </summary>
	/// <returns>Minecraft block string</returns>
	public string GetPrintMaterial()
	{
		return _printMaterial.options[_printMaterial.value].text;
	}

	#endregion Gets

	/// <summary>
	/// Creates a dropdown list configuration of all blocks within minecraft 1.15.2
	/// </summary>
	/// <returns>dropdown list configuration of all block string values</returns>
	public List<TMP_Dropdown.OptionData> GenerateOptions()
	{
		List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();

		newOptions.Add(new TMP_Dropdown.OptionData("acacia_leaves"));
		newOptions.Add(new TMP_Dropdown.OptionData("acacia_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("acacia_planks"));
		newOptions.Add(new TMP_Dropdown.OptionData("acacia_pressure_plate"));
		newOptions.Add(new TMP_Dropdown.OptionData("acacia_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("andesite"));
		newOptions.Add(new TMP_Dropdown.OptionData("anvil"));
		newOptions.Add(new TMP_Dropdown.OptionData("barrel"));
		newOptions.Add(new TMP_Dropdown.OptionData("barrier"));
		newOptions.Add(new TMP_Dropdown.OptionData("beacon"));
		newOptions.Add(new TMP_Dropdown.OptionData("bedrock"));
		newOptions.Add(new TMP_Dropdown.OptionData("bell"));
		newOptions.Add(new TMP_Dropdown.OptionData("birch_leaves"));
		newOptions.Add(new TMP_Dropdown.OptionData("birch_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("birch_planks"));
		newOptions.Add(new TMP_Dropdown.OptionData("birch_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("black_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("black_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("black_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("black_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("black_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("black_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("blast_furnace"));
		newOptions.Add(new TMP_Dropdown.OptionData("blue_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("blue_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("blue_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("blue_ice"));
		newOptions.Add(new TMP_Dropdown.OptionData("blue_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("blue_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("blue_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("bone_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("bookshelf"));
		newOptions.Add(new TMP_Dropdown.OptionData("brain_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("brewing_stand"));
		newOptions.Add(new TMP_Dropdown.OptionData("bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("brown_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("brown_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("brown_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("brown_mushroom_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("brown_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("brown_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("brown_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("bubble_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("cake"));
		newOptions.Add(new TMP_Dropdown.OptionData("campfire"));
		newOptions.Add(new TMP_Dropdown.OptionData("cartography_table"));
		newOptions.Add(new TMP_Dropdown.OptionData("carved_pumpkin"));
		newOptions.Add(new TMP_Dropdown.OptionData("cauldron"));
		newOptions.Add(new TMP_Dropdown.OptionData("chest"));
		newOptions.Add(new TMP_Dropdown.OptionData("chipped_anvil"));
		newOptions.Add(new TMP_Dropdown.OptionData("chiseled_quartz_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("chiseled_red_sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("chiseled_sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("chiseled_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("clay"));
		newOptions.Add(new TMP_Dropdown.OptionData("coal_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("coal_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("coarse_dirt"));
		newOptions.Add(new TMP_Dropdown.OptionData("cobblestone"));
		newOptions.Add(new TMP_Dropdown.OptionData("cobweb"));
		newOptions.Add(new TMP_Dropdown.OptionData("composter"));
		newOptions.Add(new TMP_Dropdown.OptionData("cracked_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("crafting_table"));
		newOptions.Add(new TMP_Dropdown.OptionData("cut_red_sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("cut_sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("cyan_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("cyan_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("cyan_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("cyan_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("cyan_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("cyan_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("damaged_anvil"));
		newOptions.Add(new TMP_Dropdown.OptionData("dark_oak_leaves"));
		newOptions.Add(new TMP_Dropdown.OptionData("dark_oak_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("dark_oak_planks"));
		newOptions.Add(new TMP_Dropdown.OptionData("dark_oak_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("dark_prismarine"));
		newOptions.Add(new TMP_Dropdown.OptionData("dead_brain_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("dead_bubble_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("dead_fire_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("dead_horn_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("dead_tube_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("diamond_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("diamond_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("diorite"));
		newOptions.Add(new TMP_Dropdown.OptionData("dirt"));
		newOptions.Add(new TMP_Dropdown.OptionData("dispenser"));
		newOptions.Add(new TMP_Dropdown.OptionData("dried_kelp_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("dropper"));
		newOptions.Add(new TMP_Dropdown.OptionData("emerald_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("emerald_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("end_stone"));
		newOptions.Add(new TMP_Dropdown.OptionData("end_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("fire_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("fletching_table"));
		newOptions.Add(new TMP_Dropdown.OptionData("frosted_ice"));
		newOptions.Add(new TMP_Dropdown.OptionData("furnace"));
		newOptions.Add(new TMP_Dropdown.OptionData("glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("glowstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("gold_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("gold_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("granite"));
		newOptions.Add(new TMP_Dropdown.OptionData("grass_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("gravel"));
		newOptions.Add(new TMP_Dropdown.OptionData("gray_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("gray_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("gray_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("gray_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("gray_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("gray_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("green_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("green_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("green_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("green_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("green_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("green_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("hay_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("hopper"));
		newOptions.Add(new TMP_Dropdown.OptionData("horn_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("ice"));
		newOptions.Add(new TMP_Dropdown.OptionData("infested_chiseled_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("infested_cobblestone"));
		newOptions.Add(new TMP_Dropdown.OptionData("infested_cracked_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("infested_mossy_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("infested_stone"));
		newOptions.Add(new TMP_Dropdown.OptionData("infested_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("iron_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("iron_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("jack_o_lantern"));
		newOptions.Add(new TMP_Dropdown.OptionData("jukebox"));
		newOptions.Add(new TMP_Dropdown.OptionData("jungle_leaves"));
		newOptions.Add(new TMP_Dropdown.OptionData("jungle_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("jungle_planks"));
		newOptions.Add(new TMP_Dropdown.OptionData("jungle_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("lantern"));
		newOptions.Add(new TMP_Dropdown.OptionData("lapis_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("lapis_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_blue_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_blue_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_blue_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_blue_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_blue_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_gray_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_gray_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_gray_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_gray_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_gray_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("light_gray_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("lime_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("lime_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("lime_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("lime_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("lime_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("lime_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("loom"));
		newOptions.Add(new TMP_Dropdown.OptionData("magenta_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("magenta_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("magenta_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("magenta_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("magenta_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("magenta_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("magma_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("melon"));
		newOptions.Add(new TMP_Dropdown.OptionData("mossy_cobblestone"));
		newOptions.Add(new TMP_Dropdown.OptionData("mossy_stone_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("mycelium"));
		newOptions.Add(new TMP_Dropdown.OptionData("nether_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("nether_quartz_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("nether_wart_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("netherrack"));
		newOptions.Add(new TMP_Dropdown.OptionData("note_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("oak_leaves"));
		newOptions.Add(new TMP_Dropdown.OptionData("oak_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("oak_planks"));
		newOptions.Add(new TMP_Dropdown.OptionData("oak_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("observer"));
		newOptions.Add(new TMP_Dropdown.OptionData("obsidian"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_tulip"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_wall_banner"));
		newOptions.Add(new TMP_Dropdown.OptionData("orange_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("packed_ice"));
		newOptions.Add(new TMP_Dropdown.OptionData("pink_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("pink_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("pink_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("pink_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("pink_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("pink_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("piston"));
		newOptions.Add(new TMP_Dropdown.OptionData("podzol"));
		newOptions.Add(new TMP_Dropdown.OptionData("polished_andesite"));
		newOptions.Add(new TMP_Dropdown.OptionData("polished_diorite"));
		newOptions.Add(new TMP_Dropdown.OptionData("polished_granite"));
		newOptions.Add(new TMP_Dropdown.OptionData("prismarine"));
		newOptions.Add(new TMP_Dropdown.OptionData("prismarine_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("pumpkin"));
		newOptions.Add(new TMP_Dropdown.OptionData("purple_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("purple_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("purple_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("purple_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("purple_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("purple_wall_banner"));
		newOptions.Add(new TMP_Dropdown.OptionData("purple_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("purpur_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("purpur_pillar"));
		newOptions.Add(new TMP_Dropdown.OptionData("quartz_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_mushroom_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_nether_bricks"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_sand"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("red_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("redstone_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("redstone_lamp"));
		newOptions.Add(new TMP_Dropdown.OptionData("redstone_ore"));
		newOptions.Add(new TMP_Dropdown.OptionData("sand"));
		newOptions.Add(new TMP_Dropdown.OptionData("sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("slime_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("smithing_table"));
		newOptions.Add(new TMP_Dropdown.OptionData("smoker"));
		newOptions.Add(new TMP_Dropdown.OptionData("smooth_quartz"));
		newOptions.Add(new TMP_Dropdown.OptionData("smooth_red_sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("smooth_sandstone"));
		newOptions.Add(new TMP_Dropdown.OptionData("smooth_stone"));
		newOptions.Add(new TMP_Dropdown.OptionData("snow_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("soul_sand"));
		newOptions.Add(new TMP_Dropdown.OptionData("sponge"));
		newOptions.Add(new TMP_Dropdown.OptionData("spruce_leaves"));
		newOptions.Add(new TMP_Dropdown.OptionData("spruce_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("spruce_planks"));
		newOptions.Add(new TMP_Dropdown.OptionData("spruce_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("sticky_piston"));
		newOptions.Add(new TMP_Dropdown.OptionData("stone"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_acacia_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_acacia_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_birch_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_birch_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_dark_oak_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_dark_oak_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_jungle_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_jungle_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_oak_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_oak_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_spruce_log"));
		newOptions.Add(new TMP_Dropdown.OptionData("stripped_spruce_wood"));
		newOptions.Add(new TMP_Dropdown.OptionData("tnt"));
		newOptions.Add(new TMP_Dropdown.OptionData("terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("tube_coral_block"));
		newOptions.Add(new TMP_Dropdown.OptionData("white_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("white_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("white_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("white_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("white_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("white_wool"));
		newOptions.Add(new TMP_Dropdown.OptionData("yellow_concrete_powder"));
		newOptions.Add(new TMP_Dropdown.OptionData("yellow_concrete"));
		newOptions.Add(new TMP_Dropdown.OptionData("yellow_glazed_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("yellow_stained_glass"));
		newOptions.Add(new TMP_Dropdown.OptionData("yellow_terracotta"));
		newOptions.Add(new TMP_Dropdown.OptionData("yellow_wool"));

		return newOptions;
	}
}
