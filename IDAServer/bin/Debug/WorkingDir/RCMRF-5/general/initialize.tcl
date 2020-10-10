#-----------------------------establish model--------------------------------------------------
model basic -ndm 2 -ndf 3
source initials.tcl
source general/memberSections.tcl
source general/frame2D.build.tcl
source general/rayleigh.tcl
puts "Model Built"
puts $logfileId "Model Built"