#> Main setup of the printer and settings
# Created by MrJohnWeez
function mcode1:uninstall

# Current pause state
scoreboard objectives add gp_ArgVar001 dummy
scoreboard players set #fakePlayerVar gp_ArgVar001 1

# Current line of gcode
scoreboard objectives add gp_ArgVar002 dummy
scoreboard players set #fakePlayerVar gp_ArgVar002 0

# Temp int var
scoreboard objectives add gp_ArgVar003 dummy
scoreboard players set #fakePlayerVar gp_ArgVar003 0

# Should be extruding (bool)
scoreboard objectives add gp_ArgVar004 dummy
scoreboard players set #fakePlayerVar gp_ArgVar004 0

scoreboard objectives add LineNumber dummy

scoreboard objectives setdisplay sidebar LineNumber

function mcode1:clear_print_bed
function mcode1:create_progress_bar

# Create home node
summon armor_stand 0 2 0 {DisabledSlots:2039583, Small:1b, Tags:[TAGGHome,TAGGPrintGroup,PrinterArmor], NoGravity:1b,Invulnerable:1b,Invisible:1b}

# Spawn in Printer head and Node
execute at @e[type=armor_stand,tag=TAGGHome,limit=1] run summon armor_stand ~0 ~0 ~0 {DisabledSlots:2039583, Small:1b, Tags:[TAGGPrintHead,TAGGPrintGroup,PrinterArmor], NoGravity:1b,Invulnerable:1b,Invisible:1b}
execute at @e[type=armor_stand,tag=TAGGHome,limit=1] run summon armor_stand ~0 ~0 ~0 {DisabledSlots:2039583, Small:1b, Tags:[TAGGNode,TAGGPrintGroup,PrinterArmor], NoGravity:1b,Invulnerable:1b,Invisible:1b}

# Create the visual nozzle
execute as @e[type=armor_stand,tag=TAGGHome,limit=1] run function mcode1:create_nozzle