data merge entity @s {Motion:[XNUMd,YNUMd,ZNUMd]}
execute at @s run setblock ~ ~-1 ~ minecraft:FILLBLOCK replace
execute at @e[type=minecraft:armor_stand,limit=1,tag=TagNode] if entity @s[distance=..1] as @s run function mcode1:move_to_next_code