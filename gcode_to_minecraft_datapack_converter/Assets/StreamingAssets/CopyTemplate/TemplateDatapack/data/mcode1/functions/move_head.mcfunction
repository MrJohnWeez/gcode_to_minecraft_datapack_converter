#> Move the printhead by an increment 0.1 -> 1.0
# 1.0 is the max movement due to minecraft's update cycle
# Created by MrJohnWeez
execute at @s run teleport @s ^ ^ ^PRINTSPEED facing entity @e[type=armor_stand,tag=TAGGNode,limit=1] feet
execute if score #fakePlayerVar gp_ArgVar004 matches 1 at @s run setblock ~ ~-1 ~ minecraft:FILLBLOCK replace
execute at @e[type=minecraft:armor_stand,limit=1,tag=TAGGNode] if entity @s[distance=..PRINTTOLERANCE] as @s run function mcode1:move_to_next_code