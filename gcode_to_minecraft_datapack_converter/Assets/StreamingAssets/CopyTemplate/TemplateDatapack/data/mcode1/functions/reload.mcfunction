#> Invoked every time user uses the reload command
# Created by MrJohnWeez
function mcode1:show_tellraw_options

# Print qulity 0=High, 1=Med, 2=Low, 3=Potato
scoreboard objectives add gp_ArgVar006 dummy
scoreboard players set #fakePlayerVar gp_ArgVar006 0