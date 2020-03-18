say mcode1 is printing...

function mcode1:uninstall

scoreboard objectives add gp_ArgVar001 dummy
scoreboard players set #fakePlayerVar gp_ArgVar001 1

scoreboard objectives add gp_ArgVar002 dummy
scoreboard players set #fakePlayerVar gp_ArgVar002 0

scoreboard objectives add gp_ArgVar003 dummy
scoreboard players set #fakePlayerVar gp_ArgVar003 0

scoreboard objectives add gp_ArgVar004 dummy
scoreboard players set #fakePlayerVar gp_ArgVar004 0

scoreboard objectives setdisplay sidebar gp_ArgVar002

function mcode1:clear_print_bed
function mcode1:create_progress_bar

# Create home node
summon armor_stand 0 2 0 {DisabledSlots:2039583, Small:1b, Tags:[TagHome,TagPrintGroup,PrinterArmor], NoGravity:1b}

# Spawn in Printer head and current mcode TagNode
execute at @e[type=armor_stand,tag=TagHome,limit=1] run summon armor_stand ~0 ~0 ~0 {DisabledSlots:2039583, Small:1b, Tags:[TagPrintHead,TagPrintGroup,PrinterArmor], NoGravity:1b}
execute at @e[type=armor_stand,tag=TagHome,limit=1] run summon armor_stand ~0 ~0 ~0 {DisabledSlots:2039583, Small:1b, Tags:[TagNode,TagPrintGroup,PrinterArmor], NoGravity:1b}