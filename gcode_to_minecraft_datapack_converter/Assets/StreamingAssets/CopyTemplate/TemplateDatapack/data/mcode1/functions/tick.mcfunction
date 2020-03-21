scoreboard players operation gp_ArgVar LineNumber = #fakePlayerVar gp_ArgVar002
execute if score #fakePlayerVar gp_ArgVar001 matches ..-1 as @e[type=minecraft:armor_stand,limit=1,tag=TAGGPrintHead] run function mcode1:halt
execute if score #fakePlayerVar gp_ArgVar001 matches 1 as @e[type=minecraft:armor_stand,limit=1,tag=TAGGPrintHead] run function mcode1:move_head
execute if score #fakePlayerVar gp_ArgVar001 matches -100.. as @e[type=minecraft:armor_stand,limit=1,tag=TAGGPrintHead] run function mcode1:move_printer_arms