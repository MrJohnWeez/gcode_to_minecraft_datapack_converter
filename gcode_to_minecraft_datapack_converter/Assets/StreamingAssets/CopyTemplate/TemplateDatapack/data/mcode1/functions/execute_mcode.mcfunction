#> This is where all mcode (converted gcode) is ran
scoreboard players operation LineNum gp_mcodeLineNum = #mcode1 gp_mcodeLineNum
execute if score #mcode1 gp_mcodeLineNum matches 0 as @s run function mcode1:line1
execute if score #mcode1 gp_mcodeLineNum matches 1 as @s run function mcode1:line2
execute if score #mcode1 gp_mcodeLineNum matches 2 as @s run function mcode1:line3
execute if score #mcode1 gp_mcodeLineNum matches 3 as @s run function mcode1:line4