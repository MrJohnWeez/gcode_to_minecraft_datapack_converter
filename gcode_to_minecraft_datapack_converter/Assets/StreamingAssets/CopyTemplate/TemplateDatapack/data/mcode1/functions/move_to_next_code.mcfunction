#> Every time this function is called the next mcode line# will be set active
data merge entity @s {Motion:[0.0d,0.0d,0.0d]}
tp @s @e[type=minecraft:armor_stand,limit=1,tag=Node]
scoreboard players add #mcode1 gp_mcodeLineNum 1

# Sets node to next mcode cord
execute if score #mcode1 gp_mcodeLineNum matches 1 as @s run execute at @e[type=armor_stand,tag=Home,limit=1] run tp @e[type=minecraft:armor_stand,limit=1,tag=Node] ~-7.0 ~0.0 ~-1.0
execute if score #mcode1 gp_mcodeLineNum matches 2 as @s run execute at @e[type=armor_stand,tag=Home,limit=1] run tp @e[type=minecraft:armor_stand,limit=1,tag=Node] ~-7.0 ~1.0 ~-1.0
execute if score #mcode1 gp_mcodeLineNum matches 3 as @s run execute at @e[type=armor_stand,tag=Home,limit=1] run tp @e[type=minecraft:armor_stand,limit=1,tag=Node] ~-7.0 ~1.0 ~-7.0