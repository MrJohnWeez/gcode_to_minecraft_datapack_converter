#> This is where all mcode (converted gcode) is ran
scoreboard players operation LineNum gp_ArgVar002 = #fakePlayerVar gp_ArgVar002
scoreboard players add LineNum gp_ArgVar002 1
execute if score #fakePlayerVar gp_ArgVar002 matches 0 as @s run function mcode1:line1