#> This is where all mcode (converted gcode) is ran
scoreboard players operation LineNum gp_ArgVar002 = #fakePlayerVar gp_ArgVar002
execute if score #fakePlayerVar gp_ArgVar002 matches 0 as @s run function mcode1:line1
execute if score #fakePlayerVar gp_ArgVar002 matches 1 as @s run function mcode1:line2
execute if score #fakePlayerVar gp_ArgVar002 matches 2 as @s run function mcode1:line3
execute if score #fakePlayerVar gp_ArgVar002 matches 3 as @s run function mcode1:line4