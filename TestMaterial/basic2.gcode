; generated by PrusaSlicer 2.1.1+win64 on 2020-02-29 at 02:13:00 UTC

; 

; external perimeters extrusion width = 0.45mm
; perimeters extrusion width = 0.45mm
; infill extrusion width = 0.45mm
; solid infill extrusion width = 0.45mm
; top infill extrusion width = 0.40mm
; first layer extrusion width = 0.42mm

M73 P0 R0
M73 Q0 S0
M201 X1000 Y1000 Z1000 E5000 ; sets maximum accelerations, mm/sec^2
M203 X200 Y200 Z12 E120 ; sets maximum feedrates, mm/sec
M204 P1250 R1250 T1250 ; sets acceleration (P, T) and retract acceleration (R), mm/sec^2
M205 X8.00 Y8.00 Z0.40 E4.50 ; sets the jerk limits, mm/sec
M205 S0 T0 ; sets the minimum extruding and travel feed rate, mm/sec
M107 ; disable fan
M862.3 P "MK3S" ; printer model check
M862.1 P0.4 ; nozzle diameter check
M115 U3.8.1 ; tell printer latest fw version
G90 ; use absolute coordinates
M83 ; extruder relative mode
M104 S215 ; set extruder temp
M140 S60 ; set bed temp
M190 S60 ; wait for bed temp
M109 S215 ; wait for extruder temp
G28 W ; home all without mesh bed level
G80 ; mesh bed leveling
G1 Y-3.0 F1000.0 ; go outside print area
G92 E0.0
G1 X60.0 E9.0 F1000.0 ; intro line
G1 X100.0 E12.5 F1000.0 ; intro line
G92 E0.0
M221 S95
G21 ; set units to millimeters
G90 ; use absolute coordinates
M83 ; use relative distances for extrusion
M900 K30 ; Filament gcode
;BEFORE_LAYER_CHANGE
G92 E0.0
;0.2


G1 E-0.80000 F2100.00000 ; retract
G1 Z0.600 F10800.000 ; lift Z
;AFTER_LAYER_CHANGE
;0.2
G1 X5.225 Y6.490 ; move to first skirt point
G1 Z0.200 ; restore layer Z
G1 E0.80000 F2100.00000 ; unretract
M204 S1000 ; adjust acceleration
G1 F900
G1 X5.838 Y6.066 E0.02334 ; skirt
G1 X6.522 Y5.774 E0.02334 ; skirt
G1 X7.223 Y5.628 E0.02245 ; skirt
G1 X8.056 Y5.606 E0.02612 ; skirt
G1 X9.124 Y5.765 E0.03384 ; skirt
G1 X10.104 Y6.228 E0.03398 ; skirt
G1 X10.907 Y6.955 E0.03398 ; skirt
G1 X11.465 Y7.884 E0.03398 ; skirt
G1 X11.661 Y8.504 E0.02038 ; skirt
G1 X11.745 Y9.149 E0.02038 ; skirt
G1 X11.744 Y14.492 E0.16755 ; skirt
G1 X11.624 Y15.256 E0.02423 ; skirt
G1 X11.349 Y15.978 E0.02423 ; skirt
G1 X10.930 Y16.627 E0.02423 ; skirt
G1 X10.388 Y17.172 E0.02408 ; skirt
G1 X9.754 Y17.589 E0.02379 ; skirt
G1 X9.053 Y17.866 E0.02364 ; skirt
G1 X8.357 Y17.991 E0.02216 ; skirt
G1 X7.439 Y17.999 E0.02880 ; skirt
G1 X6.403 Y17.797 E0.03310 ; skirt
G1 X5.470 Y17.312 E0.03295 ; skirt
G1 X4.709 Y16.581 E0.03310 ; skirt
G1 X4.185 Y15.664 E0.03310 ; skirt
G1 X4.006 Y15.069 E0.01949 ; skirt
G1 X3.930 Y14.452 E0.01949 ; skirt
G1 X3.932 Y9.118 E0.16726 ; skirt
G1 X4.046 Y8.372 E0.02364 ; skirt
G1 X4.307 Y7.670 E0.02349 ; skirt
G1 X4.705 Y7.035 E0.02349 ; skirt
G1 X5.184 Y6.533 E0.02176 ; skirt
G1 X5.455 Y6.796 F10800.000 ; move to first skirt point
G1 F900
G1 X6.041 Y6.385 E0.02246 ; skirt
G1 X6.659 Y6.126 E0.02099 ; skirt
G1 X7.285 Y6.000 E0.02001 ; skirt
G1 X8.056 Y5.983 E0.02418 ; skirt
G1 X9.036 Y6.132 E0.03107 ; skirt
G1 X9.928 Y6.564 E0.03107 ; skirt
G1 X10.650 Y7.237 E0.03094 ; skirt
G1 X11.144 Y8.096 E0.03107 ; skirt
G1 X11.369 Y9.188 E0.03498 ; skirt
G1 X11.367 Y14.478 E0.16586 ; skirt
G1 X11.258 Y15.166 E0.02184 ; skirt
G1 X11.008 Y15.816 E0.02184 ; skirt
G1 X10.626 Y16.403 E0.02198 ; skirt
G1 X10.132 Y16.894 E0.02184 ; skirt
G1 X9.558 Y17.265 E0.02141 ; skirt
G1 X8.921 Y17.511 E0.02141 ; skirt
G1 X8.301 Y17.618 E0.01973 ; skirt
G1 X7.453 Y17.623 E0.02658 ; skirt
G1 X6.512 Y17.435 E0.03010 ; skirt
G1 X5.660 Y16.983 E0.03024 ; skirt
G1 X4.973 Y16.306 E0.03024 ; skirt
G1 X4.511 Y15.460 E0.03024 ; skirt
G1 X4.306 Y14.413 E0.03345 ; skirt
G1 X4.309 Y9.132 E0.16558 ; skirt
G1 X4.414 Y8.457 E0.02141 ; skirt
G1 X4.653 Y7.822 E0.02127 ; skirt
G1 X5.016 Y7.249 E0.02127 ; skirt
G1 X5.413 Y6.839 E0.01790 ; skirt
G1 X5.749 Y7.035 F10800.000 ; move to first skirt point
G1 F900
G1 X6.245 Y6.703 E0.01870 ; skirt
G1 X6.797 Y6.479 E0.01870 ; skirt
G1 X7.350 Y6.373 E0.01765 ; skirt
G1 X8.056 Y6.360 E0.02212 ; skirt
G1 X8.940 Y6.498 E0.02806 ; skirt
G1 X9.742 Y6.894 E0.02806 ; skirt
G1 X10.388 Y7.514 E0.02806 ; skirt
G1 X10.818 Y8.299 E0.02806 ; skirt
G1 X10.993 Y9.219 E0.02937 ; skirt
G1 X10.990 Y14.468 E0.16457 ; skirt
G1 X10.890 Y15.082 E0.01950 ; skirt
G1 X10.662 Y15.665 E0.01963 ; skirt
G1 X10.316 Y16.187 E0.01963 ; skirt
G1 X9.870 Y16.620 E0.01950 ; skirt
G1 X9.359 Y16.944 E0.01897 ; skirt
G1 X8.792 Y17.155 E0.01897 ; skirt
G1 X8.249 Y17.243 E0.01725 ; skirt
G1 X7.463 Y17.246 E0.02465 ; skirt
G1 X6.611 Y17.070 E0.02727 ; skirt
G1 X5.848 Y16.653 E0.02727 ; skirt
G1 X5.239 Y16.032 E0.02727 ; skirt
G1 X4.841 Y15.263 E0.02714 ; skirt
G1 X4.682 Y14.382 E0.02806 ; skirt
G1 X4.686 Y9.142 E0.16431 ; skirt
G1 X4.782 Y8.540 E0.01910 ; skirt
G1 X4.998 Y7.975 E0.01897 ; skirt
G1 X5.325 Y7.466 E0.01897 ; skirt
G1 X5.707 Y7.078 E0.01709 ; skirt
G1 F8640
G1 X6.245 Y6.703 E-0.15133 ; wipe and retract
G1 X6.797 Y6.479 E-0.13774 ; wipe and retract
G1 X7.350 Y6.373 E-0.12995 ; wipe and retract
G1 X8.056 Y6.360 E-0.16291 ; wipe and retract
G1 X8.818 Y6.479 E-0.17807 ; wipe and retract
G1 E-0.04000 F2100.00000 ; retract
G1 Z0.800 F10800.000 ; lift Z
G1 X6.934 Y6.832 ; move to first skirt point
G1 Z0.200 ; restore layer Z
G1 E0.80000 F2100.00000 ; unretract
G1 F900
G1 X8.055 Y6.737 E0.03527 ; skirt
G1 X8.845 Y6.863 E0.02508 ; skirt
G1 X9.555 Y7.224 E0.02496 ; skirt
G1 X10.122 Y7.788 E0.02508 ; skirt
G1 X10.487 Y8.496 E0.02496 ; skirt
G1 X10.616 Y9.250 E0.02398 ; skirt
G1 X10.613 Y14.454 E0.16316 ; skirt
G1 X10.523 Y14.995 E0.01721 ; skirt
G1 X10.320 Y15.505 E0.01721 ; skirt
G1 X10.013 Y15.961 E0.01721 ; skirt
G1 X9.617 Y16.340 E0.01721 ; skirt
G1 X9.166 Y16.618 E0.01660 ; skirt
G1 X8.668 Y16.798 E0.01660 ; skirt
G1 X7.478 Y16.869 E0.03740 ; skirt
G1 X6.722 Y16.709 E0.02423 ; skirt
G1 X6.048 Y16.331 E0.02423 ; skirt
G1 X5.516 Y15.770 E0.02423 ; skirt
G1 X5.175 Y15.073 E0.02435 ; skirt
G1 X5.059 Y14.356 E0.02275 ; skirt
G1 X5.063 Y9.157 E0.16304 ; skirt
G1 X5.149 Y8.626 E0.01684 ; skirt
G1 X5.343 Y8.129 E0.01672 ; skirt
G1 X5.634 Y7.683 E0.01672 ; skirt
G1 X6.012 Y7.307 E0.01672 ; skirt
G1 X6.449 Y7.022 E0.01635 ; skirt
G1 X6.879 Y6.854 E0.01447 ; skirt
G1 X6.272 Y7.582 F10800.000 ; move to first skirt point
G1 F900
G1 X7.069 Y7.186 E0.02788 ; skirt
G1 X8.055 Y7.114 E0.03101 ; skirt
G1 X8.741 Y7.226 E0.02179 ; skirt
G1 X9.361 Y7.549 E0.02191 ; skirt
G1 X9.845 Y8.048 E0.02179 ; skirt
G1 X10.149 Y8.677 E0.02191 ; skirt
G1 X10.240 Y9.271 E0.01885 ; skirt
G1 X10.236 Y14.439 E0.16202 ; skirt
G1 X10.156 Y14.906 E0.01487 ; skirt
G1 X9.978 Y15.346 E0.01487 ; skirt
G1 X9.709 Y15.736 E0.01487 ; skirt
G1 X9.362 Y16.060 E0.01487 ; skirt
G1 X8.543 Y16.440 E0.02833 ; skirt
G1 X7.493 Y16.492 E0.03297 ; skirt
G1 X6.835 Y16.348 E0.02112 ; skirt
G1 X6.251 Y16.012 E0.02112 ; skirt
G1 X5.798 Y15.514 E0.02112 ; skirt
G1 X5.517 Y14.898 E0.02123 ; skirt
G1 X5.436 Y14.339 E0.01771 ; skirt
G1 X5.439 Y9.171 E0.16202 ; skirt
G1 X5.685 Y8.287 E0.02878 ; skirt
G1 X6.234 Y7.628 E0.02690 ; skirt
; printing object cube.stl id:0 copy 0
G1 F8640
G1 X7.069 Y7.186 E-0.21806 ; wipe and retract
G1 X8.055 Y7.114 E-0.22834 ; wipe and retract
G1 X8.741 Y7.226 E-0.16050 ; wipe and retract
G1 X9.329 Y7.533 E-0.15309 ; wipe and retract
G1 E-0.04000 F2100.00000 ; retract
G1 Z0.800 F10800.000 ; lift Z
G1 X7.834 Y9.513 ; move to first perimeter point
G1 Z0.200 ; restore layer Z
G1 E0.80000 F2100.00000 ; unretract
G1 F900
G1 X7.841 Y14.097 E0.14374 ; perimeter
G1 X7.834 Y9.573 E0.14186 ; perimeter
G1 X7.834 Y9.913 F10800.000 ; move inwards before travel
; stop printing object cube.stl id:0 copy 0
G1 F8640;_WIPE
G1 X7.839 Y12.864 E-0.76000 ; wipe and retract
G1 E-0.04000 F2100.00000 ; retract
G1 Z0.800 F10800.000 ; lift Z
; Filament-specific end gcode
G4 ; wait
M221 S100
M104 S0 ; turn off temperature
M140 S0 ; turn off heatbed
M107 ; turn off fan
G1 Z30.8 ; Move print head up
G1 X0 Y200 F3000 ; home X axis
M84 ; disable motors
M73 P100 R0
M73 Q100 S0
; filament used [mm] = 4.9
; filament used [cm3] = 0.0
; filament used [g] = 0.0
; filament cost = 0.0
; total filament used [g] = 0.0
; total filament cost = 0.0
; estimated printing time (normal mode) = 24s
; estimated printing time (silent mode) = 25s

; avoid_crossing_perimeters = 0
; bed_custom_model = 
; bed_custom_texture = 
; bed_shape = 0x0,250x0,250x210,0x210
; bed_temperature = 60
; before_layer_gcode = ;BEFORE_LAYER_CHANGE\nG92 E0.0\n;[layer_z]\n\n
; between_objects_gcode = 
; bottom_fill_pattern = rectilinear
; bottom_solid_layers = 5
; bridge_acceleration = 1000
; bridge_angle = 0
; bridge_fan_speed = 100
; bridge_flow_ratio = 0.8
; bridge_speed = 30
; brim_width = 0
; clip_multipart_objects = 1
; compatible_printers_condition_cummulative = "printer_notes=~/.*PRINTER_VENDOR_PRUSA3D.*/ and printer_notes=~/.*PRINTER_MODEL_MK3.*/ and nozzle_diameter[0]==0.4";"! (printer_notes=~/.*PRINTER_VENDOR_PRUSA3D.*/ and printer_notes=~/.*PRINTER_MODEL_MK(2.5|3).*/ and single_extruder_multi_material)"
; complete_objects = 0
; cooling = 1
; cooling_tube_length = 5
; cooling_tube_retraction = 91.5
; default_acceleration = 1000
; default_filament_profile = "Prusament PLA"
; default_print_profile = 0.15mm QUALITY MK3
; deretract_speed = 0
; disable_fan_first_layers = 1
; dont_support_bridges = 1
; duplicate_distance = 6
; elefant_foot_compensation = 0
; end_filament_gcode = "; Filament-specific end gcode"
; end_gcode = G4 ; wait\nM221 S100\nM104 S0 ; turn off temperature\nM140 S0 ; turn off heatbed\nM107 ; turn off fan\n{if layer_z < max_print_height}G1 Z{z_offset+min(layer_z+30, max_print_height)}{endif} ; Move print head up\nG1 X0 Y200 F3000 ; home X axis\nM84 ; disable motors
; ensure_vertical_shell_thickness = 1
; external_perimeter_extrusion_width = 0.45
; external_perimeter_speed = 25
; external_perimeters_first = 0
; extra_loading_move = -2
; extra_perimeters = 0
; extruder_clearance_height = 25
; extruder_clearance_radius = 45
; extruder_colour = ""
; extruder_offset = 0x0
; extrusion_axis = E
; extrusion_multiplier = 1
; extrusion_width = 0.45
; fan_always_on = 1
; fan_below_layer_time = 100
; filament_colour = #FF8000
; filament_cooling_final_speed = 3.4
; filament_cooling_initial_speed = 2.2
; filament_cooling_moves = 4
; filament_cost = 25.4
; filament_density = 1.24
; filament_diameter = 1.75
; filament_load_time = 0
; filament_loading_speed = 28
; filament_loading_speed_start = 3
; filament_max_volumetric_speed = 15
; filament_minimal_purge_on_wipe_tower = 15
; filament_notes = "List of materials tested with standard PLA print settings:\n\nDas Filament\nEsun PLA\nEUMAKERS PLA\nFiberlogy HD-PLA\nFiberlogy PLA\nFloreon3D\nHatchbox PLA\nPlasty Mladec PLA\nPrimavalue PLA\nProto pasta Matte Fiber\nAmazonBasics PLA"
; filament_ramming_parameters = "120 100 6.6 6.8 7.2 7.6 7.9 8.2 8.7 9.4 9.9 10.0| 0.05 6.6 0.45 6.8 0.95 7.8 1.45 8.3 1.95 9.7 2.45 10 2.95 7.6 3.45 7.6 3.95 7.6 4.45 7.6 4.95 7.6"
; filament_settings_id = "Prusa PLA"
; filament_soluble = 0
; filament_toolchange_delay = 0
; filament_type = PLA
; filament_unload_time = 0
; filament_unloading_speed = 90
; filament_unloading_speed_start = 100
; fill_angle = 45
; fill_density = 15%
; fill_pattern = gyroid
; first_layer_acceleration = 1000
; first_layer_bed_temperature = 60
; first_layer_extrusion_width = 0.42
; first_layer_height = 0.2
; first_layer_speed = 20
; first_layer_temperature = 215
; gap_fill_speed = 40
; gcode_comments = 1
; gcode_flavor = marlin
; gcode_label_objects = 1
; high_current_on_filament_swap = 0
; host_type = octoprint
; infill_acceleration = 1000
; infill_every_layers = 1
; infill_extruder = 1
; infill_extrusion_width = 0.45
; infill_first = 0
; infill_only_where_needed = 0
; infill_overlap = 25%
; infill_speed = 80
; inherits_cummulative = "0.15mm QUALITY MK3";;
; interface_shells = 0
; layer_gcode = ;AFTER_LAYER_CHANGE\n;[layer_z]
; layer_height = 0.15
; machine_max_acceleration_e = 5000,5000
; machine_max_acceleration_extruding = 1250,1250
; machine_max_acceleration_retracting = 1250,1250
; machine_max_acceleration_x = 1000,960
; machine_max_acceleration_y = 1000,960
; machine_max_acceleration_z = 1000,1000
; machine_max_feedrate_e = 120,120
; machine_max_feedrate_x = 200,100
; machine_max_feedrate_y = 200,100
; machine_max_feedrate_z = 12,12
; machine_max_jerk_e = 4.5,4.5
; machine_max_jerk_x = 8,8
; machine_max_jerk_y = 8,8
; machine_max_jerk_z = 0.4,0.4
; machine_min_extruding_rate = 0,0
; machine_min_travel_rate = 0,0
; max_fan_speed = 100
; max_layer_height = 0.25
; max_print_height = 210
; max_print_speed = 200
; max_volumetric_speed = 0
; min_fan_speed = 100
; min_layer_height = 0.07
; min_print_speed = 15
; min_skirt_length = 4
; notes = 
; nozzle_diameter = 0.4
; only_retract_when_crossing_perimeters = 0
; ooze_prevention = 0
; output_filename_format = {input_filename_base}_{layer_height}mm_{filament_type[0]}_{printer_model}_{print_time}.gcode
; overhangs = 0
; parking_pos_retraction = 92
; perimeter_acceleration = 800
; perimeter_extruder = 1
; perimeter_extrusion_width = 0.45
; perimeter_speed = 45
; perimeters = 2
; post_process = 
; print_host = 
; print_settings_id = cube.3mf (0.15mm QUALITY MK3)
; printer_model = MK3S
; printer_notes = Don't remove the following keywords! These keywords are used in the "compatible printer" condition of the print and filament profiles to link the particular print and filament profiles to this printer profile.\nPRINTER_VENDOR_PRUSA3D\nPRINTER_MODEL_MK3\n
; printer_settings_id = Original Prusa i3 MK3S
; printer_technology = FFF
; printer_variant = 0.4
; printer_vendor = 
; printhost_apikey = 
; printhost_cafile = 
; raft_layers = 0
; remaining_times = 1
; resolution = 0
; retract_before_travel = 1
; retract_before_wipe = 0%
; retract_layer_change = 1
; retract_length = 0.8
; retract_length_toolchange = 4
; retract_lift = 0.6
; retract_lift_above = 0
; retract_lift_below = 209
; retract_restart_extra = 0
; retract_restart_extra_toolchange = 0
; retract_speed = 35
; seam_position = nearest
; serial_port = 
; serial_speed = 250000
; silent_mode = 1
; single_extruder_multi_material = 0
; single_extruder_multi_material_priming = 0
; skirt_distance = 2
; skirt_height = 3
; skirts = 1
; slice_closing_radius = 0.049
; slowdown_below_layer_time = 20
; small_perimeter_speed = 25
; solid_infill_below_area = 0
; solid_infill_every_layers = 0
; solid_infill_extruder = 1
; solid_infill_extrusion_width = 0.45
; solid_infill_speed = 80
; spiral_vase = 0
; standby_temperature_delta = -5
; start_filament_gcode = "M900 K{if printer_notes=~/.*PRINTER_MODEL_MINI.*/ and nozzle_diameter[0]==0.6}0.12{elsif printer_notes=~/.*PRINTER_MODEL_MINI.*/}0.2{elsif printer_notes=~/.*PRINTER_HAS_BOWDEN.*/}200{elsif nozzle_diameter[0]==0.6}18{else}30{endif} ; Filament gcode"
; start_gcode = M862.3 P "[printer_model]" ; printer model check\nM862.1 P[nozzle_diameter] ; nozzle diameter check\nM115 U3.8.1 ; tell printer latest fw version\nG90 ; use absolute coordinates\nM83 ; extruder relative mode\nM104 S[first_layer_temperature] ; set extruder temp\nM140 S[first_layer_bed_temperature] ; set bed temp\nM190 S[first_layer_bed_temperature] ; wait for bed temp\nM109 S[first_layer_temperature] ; wait for extruder temp\nG28 W ; home all without mesh bed level\nG80 ; mesh bed leveling\nG1 Y-3.0 F1000.0 ; go outside print area\nG92 E0.0\nG1 X60.0 E9.0 F1000.0 ; intro line\nG1 X100.0 E12.5 F1000.0 ; intro line\nG92 E0.0\nM221 S{if layer_height<0.075}100{else}95{endif}
; support_material = 0
; support_material_angle = 0
; support_material_auto = 1
; support_material_buildplate_only = 0
; support_material_contact_distance = 0.1
; support_material_enforce_layers = 0
; support_material_extruder = 0
; support_material_extrusion_width = 0.35
; support_material_interface_contact_loops = 0
; support_material_interface_extruder = 0
; support_material_interface_layers = 2
; support_material_interface_spacing = 0.2
; support_material_interface_speed = 100%
; support_material_pattern = rectilinear
; support_material_spacing = 2
; support_material_speed = 50
; support_material_synchronize_layers = 0
; support_material_threshold = 55
; support_material_with_sheath = 0
; support_material_xy_spacing = 50%
; temperature = 210
; thin_walls = 0
; threads = 8
; thumbnails = 
; toolchange_gcode = 
; top_fill_pattern = rectilinear
; top_infill_extrusion_width = 0.4
; top_solid_infill_speed = 40
; top_solid_layers = 7
; travel_speed = 180
; use_firmware_retraction = 0
; use_relative_e_distances = 1
; use_volumetric_e = 0
; variable_layer_height = 1
; wipe = 1
; wipe_into_infill = 0
; wipe_into_objects = 0
; wipe_tower = 1
; wipe_tower_bridging = 10
; wipe_tower_rotation_angle = 0
; wipe_tower_width = 60
; wipe_tower_x = 170
; wipe_tower_y = 125
; wiping_volumes_extruders = 70,70
; wiping_volumes_matrix = 0
; xy_size_compensation = 0
; z_offset = 0
