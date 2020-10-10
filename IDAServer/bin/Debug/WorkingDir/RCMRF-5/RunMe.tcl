# source tmp_in.tcl
puts "Running at Core: $coreid"
puts "Model = $model"
puts "Record = $record"
puts "Sa = $sa"
source general/ReadSMDfile.tcl
# reads variables: model record sa
set folder $record/$sa
file mkdir $folder
set logfileId [open $folder/log.txt w+]
puts $logfileId "Running at Core: $coreid"
puts $logfileId "Model = $model"
puts $logfileId "Record = $record"
puts $logfileId "Sa = $sa"
source general/initialize.tcl
set inFile general/GMfiles/$record.AT2
set GmFile general/GMfiles/transformed/$record.txt
file mkdir "general/GMfiles/transformed"
ReadSMDFile $inFile $GmFile deltaT Tmax
set Tmax 5
puts "Tmax = $Tmax"
set inspec "general/GMfiles/spectra/$record.txt"
source general/spectrum.tcl
set sarec [spectrum $inspec	$Tperiod]
set fac [expr $sa/$sarec]
puts "fac = $fac"
puts $logfileId "fac = $fac"
set fac [expr $g*$fac]

source general/frame2D.analyze.gravity.tcl
puts "NTH Analysis Started"
source general/frame2D.analyze.GM.Uniform.tcl

close $logfileId

set tetaMax 0.
for {set i 1} {$i <= $nflrs} {incr i} {
	set i1 [open $folder/envelopdrifts/$i.txt r]
	gets $i1 a ; gets $i1 b ; gets $i1 c
	set teta [lindex $c 1]
	if {$teta > $tetaMax} {set tetaMax $teta}
	close $i1
}
set io [open tmp_out_[set coreid].txt w+]
puts $io $tetaMax
puts $io $endTime
puts $io $dvrgFlag
close $io
