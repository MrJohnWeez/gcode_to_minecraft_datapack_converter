data merge entity @s {Motion:[-0.07071d,0.0d,0.07071d]}
execute at @s run setblock ~ ~-1 ~ minecraft:light_blue_wool replace
execute at @e[type=minecraft:armor_stand,limit=1,tag=Node] if entity @s[distance=..0.2] as @s run function mcode1:move_to_next_code