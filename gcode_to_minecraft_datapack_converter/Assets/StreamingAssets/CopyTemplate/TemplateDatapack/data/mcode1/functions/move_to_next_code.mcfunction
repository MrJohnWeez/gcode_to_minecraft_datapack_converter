#> Every time this function is called the next mcode line# will be set active
tp @s @e[type=minecraft:armor_stand,limit=1,tag=TAGGNode]
scoreboard players add #fakePlayerVar gp_ArgVar002 1

# update progress bar
execute store result bossbar mcode1:progressbar value run scoreboard players get #fakePlayerVar gp_ArgVar002

execute as @s run function mcode1:update_code_line