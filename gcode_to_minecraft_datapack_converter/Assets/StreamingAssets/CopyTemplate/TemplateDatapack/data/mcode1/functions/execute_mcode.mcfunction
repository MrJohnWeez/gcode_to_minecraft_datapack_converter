#> This is where all mcode (converted gcode) is ran
execute if score #fakePlayerVar gp_ArgVar002 matches 0 as @s run function mcode1:line1
execute if score #fakePlayerVar gp_ArgVar002 matches 1 as @s run function mcode1:line2