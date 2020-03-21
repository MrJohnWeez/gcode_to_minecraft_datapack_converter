# Create a toggle state using a switch statement
scoreboard players set #fakePlayerVar gp_ArgVar003 0
execute if score #fakePlayerVar gp_ArgVar003 matches 0 if score #fakePlayerVar gp_ArgVar001 matches ..-1 run scoreboard players set #fakePlayerVar gp_ArgVar003 1
execute if score #fakePlayerVar gp_ArgVar003 matches 0 if score #fakePlayerVar gp_ArgVar001 matches 0.. run scoreboard players set #fakePlayerVar gp_ArgVar003 2

execute if score #fakePlayerVar gp_ArgVar003 matches 1 run scoreboard players set #fakePlayerVar gp_ArgVar001 1
execute if score #fakePlayerVar gp_ArgVar003 matches 1 as @e[type=minecraft:armor_stand,limit=1,tag=TAGGPrintHead] run function mcode1:update_code_line
execute if score #fakePlayerVar gp_ArgVar003 matches 1 run say Printing started!

execute if score #fakePlayerVar gp_ArgVar003 matches 2 run scoreboard players set #fakePlayerVar gp_ArgVar001 -1
execute if score #fakePlayerVar gp_ArgVar003 matches 2 run say Printing paused!