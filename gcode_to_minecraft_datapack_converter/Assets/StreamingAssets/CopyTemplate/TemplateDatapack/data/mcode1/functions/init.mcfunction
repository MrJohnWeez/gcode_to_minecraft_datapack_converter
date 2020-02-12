say mcode1 is printing...

scoreboard objectives add gp_ArgVar001 dummy
scoreboard players set #fakePlayerVar gp_ArgVar001 1

scoreboard objectives add gp_ArgVar002 dummy
scoreboard players set #fakePlayerVar gp_ArgVar002 0

scoreboard objectives setdisplay sidebar gp_ArgVar002

kill @e[type=minecraft:armor_stand,tag=TagPrintGroup]
fill 1 53 12 -7 59 -1 minecraft:air replace

# Set up local centerpoint where player is
execute at @p run summon leash_knot ~ ~ ~ {Tags:[TagCenterPoint]}
execute at @e[type=leash_knot,limit=1,tag=TagCenterPoint] run summon armor_stand ~ ~0.5 ~ {DisabledSlots:2039583, Small:1b, Tags:[TagHome,TagPrintGroup], NoGravity:1b}
kill @e[type=leash_knot,limit=1,tag=TagCenterPoint]

# Spawn in Printer head and current mcode TagNode
execute at @e[type=armor_stand,tag=TagHome,limit=1] run summon armor_stand ~-1.0 ~0.0 ~-12.0 {DisabledSlots:2039583, Small:1b, Tags:[TagPrintHead,TagPrintGroup]}
execute at @e[type=armor_stand,tag=TagHome,limit=1] run summon armor_stand ~-7.0 ~0.0 ~-6.0 {DisabledSlots:2039583, Small:1b, Tags:[TagNode,TagPrintGroup], NoGravity:1b}