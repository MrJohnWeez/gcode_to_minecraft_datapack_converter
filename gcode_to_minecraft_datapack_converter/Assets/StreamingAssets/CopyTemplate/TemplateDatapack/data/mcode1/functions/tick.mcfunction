execute if score #fakePlayerVar gp_ArgVar001 matches 1 run scoreboard players operation LineNum gp_ArgVar002 = #fakePlayerVar gp_ArgVar002
execute if score #fakePlayerVar gp_ArgVar001 matches 1 run scoreboard players add LineNum gp_ArgVar002 1
execute if score #fakePlayerVar gp_ArgVar001 matches 1 as @e[type=minecraft:armor_stand,limit=1,tag=TagPrintHead] run function mcode1:execute_mcode