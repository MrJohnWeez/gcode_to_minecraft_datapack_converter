say mcode1 is printing...

scoreboard objectives add gp_mcodeInited dummy
scoreboard players set #mcode1 gp_mcodeInited 1

scoreboard objectives add gp_mcodeLineNum dummy
scoreboard players set #mcode1 gp_mcodeLineNum 0

scoreboard objectives setdisplay sidebar gp_mcodeLineNum

kill @e[type=minecraft:armor_stand,tag=PrintGroup]
fill 1 53 12 -7 59 -1 minecraft:air replace

# Set up local centerpoint where player is
execute at @p run summon leash_knot ~ ~ ~ {Tags:[CenterPoint]}
execute at @e[type=leash_knot,limit=1,tag=CenterPoint] run summon armor_stand ~ ~0.5 ~ {DisabledSlots:2039583, Small:1b, Tags:[Home,PrintGroup], NoGravity:1b}
kill @e[type=leash_knot,limit=1,tag=CenterPoint]

# Spawn in Printer head and current mcode node
execute at @e[type=armor_stand,tag=Home,limit=1] run summon armor_stand ~-1.0 ~0.0 ~-12.0 {DisabledSlots:2039583, Small:1b, Tags:[PrintHead,PrintGroup]}
execute at @e[type=armor_stand,tag=Home,limit=1] run summon armor_stand ~-7.0 ~0.0 ~-6.0 {DisabledSlots:2039583, Small:1b, Tags:[Node,PrintGroup], NoGravity:1b}