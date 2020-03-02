#> Every time this function is called the next mcode line# will be set active
data merge entity @s {Motion:[0.0d,0.0d,0.0d]}
tp @s @e[type=minecraft:armor_stand,limit=1,tag=TagNode]
scoreboard players add #fakePlayerVar gp_ArgVar002 1

execute as @s run function mcode1:update_code_line