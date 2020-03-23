#> Add details to the printer
# Created by MrJohnWeez

# Spawn particles for the print arms decrease particles depending on quality settings 
execute if score #fakePlayerVar gp_ArgVar006 matches 0 at @s run particle flash ~ ~2.3 127 0 0 70 0 500 force
execute if score #fakePlayerVar gp_ArgVar006 matches 0 at @s run particle flash ~ 127 -10 0 70 0 0 500 force
execute if score #fakePlayerVar gp_ArgVar006 matches 0 at @s run particle flash ~ 127 264 0 70 0 0 500 force
execute if score #fakePlayerVar gp_ArgVar006 matches 0 at @s run particle flash 127 ~2.3 ~ 70 0 0 0 500 force

execute if score #fakePlayerVar gp_ArgVar006 matches 1 at @s run particle flash ~ ~2.3 127 0 0 70 0 200 force
execute if score #fakePlayerVar gp_ArgVar006 matches 1 at @s run particle flash ~ 127 -10 0 70 0 0 200 force
execute if score #fakePlayerVar gp_ArgVar006 matches 1 at @s run particle flash ~ 127 264 0 70 0 0 200 force
execute if score #fakePlayerVar gp_ArgVar006 matches 1 at @s run particle flash 127 ~2.3 ~ 70 0 0 0 200 force

# Move nozzle model into place every frame
execute at @s run teleport @e[type=armor_stand,tag=TAGGNozzleGroup] @s
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle1] ~0.0 ~-1.76 ~0.273438 0 180
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle2] ~-0.273438 ~-1.76 ~0.0 0 90
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle3] ~-0.19335 ~-1.76 ~0.19335 0 90
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle4] ~0.19335 ~-1.76 ~0.19335 0 90
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle5] ~0.0 ~-0.1 ~0.0 0 180
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle6] ~0.0 ~-0.35 ~0.0 0 180
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle7] ~0.0 ~0.275 ~0.0 0 180
execute at @s run teleport @e[type=armor_stand,limit=1,tag=TAGGNozzle8] ~-0.55 ~-1.6 ~-0.273438 0 180

# Show particles on nozzle model dependent on quality settings
execute if score #fakePlayerVar gp_ArgVar006 matches ..1 if score #fakePlayerVar gp_ArgVar004 matches 1 at @s run particle falling_dust minecraft:FILLBLOCK ~0.0 ~0.05 ~0.0 0.0 0.0 0.0 0 5 force
execute if score #fakePlayerVar gp_ArgVar006 matches 2 if score #fakePlayerVar gp_ArgVar004 matches 1 at @s run particle falling_dust minecraft:FILLBLOCK ~0.0 ~0.05 ~0.0 0.0 0.0 0.0 0 1 force