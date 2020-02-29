# Create a toggle state using a switch statement
scoreboard players set #fakePlayerVar gp_ArgVar003 0
execute if score #fakePlayerVar gp_ArgVar003 matches 0 if score #fakePlayerVar gp_ArgVar001 matches ..-1 run scoreboard players set #fakePlayerVar gp_ArgVar003 1
execute if score #fakePlayerVar gp_ArgVar003 matches 0 if score #fakePlayerVar gp_ArgVar001 matches 0.. run scoreboard players set #fakePlayerVar gp_ArgVar003 2

execute if score #fakePlayerVar gp_ArgVar003 matches 1 run scoreboard players set #fakePlayerVar gp_ArgVar001 1
execute if score #fakePlayerVar gp_ArgVar003 matches 2 run scoreboard players set #fakePlayerVar gp_ArgVar001 -1