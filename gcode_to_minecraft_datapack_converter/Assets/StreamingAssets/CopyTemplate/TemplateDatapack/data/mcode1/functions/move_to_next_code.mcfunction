#> Every time this function is called the next mcode line# will be set active
data merge entity @s {Motion:[0.0d,0.0d,0.0d]}
tp @s @e[type=minecraft:armor_stand,limit=1,tag=TagNode]
scoreboard players add #fakePlayerVar gp_ArgVar002 1

# Sets TagNode to next mcode cord
execute if score #fakePlayerVar gp_ArgVar002 matches 1 as @s run execute at @e[type=armor_stand,tag=TagHome,limit=1] run tp @e[type=minecraft:armor_stand,limit=1,tag=TagNode] ~-7.0 ~0.0 ~-1.0
execute if score #fakePlayerVar gp_ArgVar002 matches 2 as @s run execute at @e[type=armor_stand,tag=TagHome,limit=1] run tp @e[type=minecraft:armor_stand,limit=1,tag=TagNode] ~-7.0 ~1.0 ~-1.0
execute if score #fakePlayerVar gp_ArgVar002 matches 3 as @s run execute at @e[type=armor_stand,tag=TagHome,limit=1] run tp @e[type=minecraft:armor_stand,limit=1,tag=TagNode] ~-7.0 ~1.0 ~-7.0