#Update this file for your frames number of stories and designed data
#Mind the Coding: 11: Story 1 's Outer Column; 12: Story 1 's Inner Column; 13: Story 1 's Beam;

source general/DesignOutput/$designFile.tcl
set beamSecs [list ]
set indices [lsort -integer [array names memSec]]
foreach index $indices {
	set sec $memSec($index)
	if {[string index $sec 0] == "C"} {continue}
	lappend beamSecs $sec
}

#Materials

# An elastic rigid material for Constrained DOFs and panel zone deformations
set rigidMatTag 2
uniaxialMaterial Elastic 2 1.0e15

# set Ec [expr 51000.*sqrt(-1.*$UnconfFc/10); #concrete modulus
set rebarE 2.1e6
set rebarFy 4000.0
set Ec [expr 234000.]; #concrete modulus
set fc 250.0		;#concrete specified strength in Kgf/cm2
for {set j $nflrs} {$j >= 1} {set j [expr $j-1]} {
	set dead  $Deadfloor
	set live $Livefloor
	if {$j == $nflrs} {
		set dead  $Deadroof
		set live $Liveroof
	}
	set loadbayExt [expr $lbeam/2.]
	set loadbayInt $lbeam
	set loadareaExt [expr $loadbayExt*$perpWidth]
	set loadareaInt [expr $loadbayInt*$perpWidth]
	set loadExt [expr ($deadSeisRat*$dead+$liveSeisRat*$live)*$loadareaExt]
	set loadInt [expr ($deadSeisRat*$dead+$liveSeisRat*$live)*$loadareaInt]
	if {$j == $nflrs} {
		set columnForce([expr $j*10+1]) [expr $loadExt]
		set columnForce([expr $j*10+2]) [expr $loadInt]
	} else {
		set columnForce([expr $j*10+1]) [expr $columnForce([expr ($j+1)*10+1]) + $loadExt]
		set columnForce([expr $j*10+2]) [expr $columnForce([expr ($j+1)*10+2]) + $loadInt]
	}
}

#Define materials for lumped plastic hinges
#beam sections
set beamMatId(0) 0
set colMatId(0) 0
set id 3
set cunit 0.1		;#ratio for converting the current stress unit to MPa
set ls $lbeam

foreach sec $beamSecs {
	set beamMatId($sec) [incr id] 
	source "general/sections/$sec.tcl"
	set dv 1.0				;#Stirrup Bar Dia. (cm)
	set s 7.50				;#Stirrup Spacing (cm)
	set N 0.0
	source "general/computeHingeRC.tcl"
	uniaxialMaterial Clough $beamMatId($sec) $ke $my [expr -$my] $alfah $R $alfac $tetac [expr -$tetac] $gama 0 0 $gama 1 1 1 1
	# uniaxialMaterial Elastic $beamMatId($sec) $ke
}

for {set j 1} {$j <= $nflrs} {incr j} {
	set ls $lcolumn
	if {$j == 1} {set ls $lcolumnbase}
	foreach col {1 2} {					;#outer and inner columns
		#Outer Columns:
		set ID [expr $j*10+$col]
		set sec $memSec($ID)
		source general/sections/$sec.tcl
		set colMatId($ID) [incr id]
		set N $columnForce($ID)
		source "general/computeHingeRC.tcl"
		uniaxialMaterial Clough $colMatId($ID) $ke $my [expr -$my] $alfah $R $alfac $tetac [expr -$tetac] $gama 0 0 $gama 1 1 1 1
		# uniaxialMaterial Elastic $colMatId($ID) $ke
	}

}
