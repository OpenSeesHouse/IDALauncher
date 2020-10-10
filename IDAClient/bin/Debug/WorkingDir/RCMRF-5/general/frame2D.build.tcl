# -----------------------------------------------------------------------------------
# 2D Perimeter Frame with distributed plasticity beam-columns and panel zones
# -----------------------------------------------------------------------------------
#input parameters:

#lcolumn               	height of columns 
#lcolumnbase           	base floor columns height 
#lbeam              	length of beams
#nflrs            		number of floors
#nbays            		number of bays
#perpWidth				bay dimension in direction perpendicular to the frame plane
#perpLength				half of plan dimension in direction perpendicular to the frame plane
#Deadfloor				distributed dead load for floors
#Deadroof 				distributed dead load for roof
#Livefloor				distributed live load for floors
#Liveroof 				distributed live load for roof
#Units:					N, mm, s
#_______________________________________________________________________________________


#We need to destruct previous models if the file is called in a lopp-wise manner:
wipe
# create folders for recorded responses:
file mkdir $folder/drifts
file mkdir $folder/envelopdrifts
# file mkdir $folder/hinges
# file mkdir $folder/ColumnF
# file mkdir $folder/BaseReacts

# Gravity Coefficient:
set g 980.65 ; # cm/s2


#calculate locations of beam/column intersections (Grids)
# -----------------------------------------------------------------------------------
set y(0) 0 ; set y(1) $lcolumnbase
for {set j 2} { $j <= $nflrs } {incr j} {
	set y($j) [expr $y([expr $j-1])+$lcolumn]
} 

for { set i 1} {$i <= [expr $nbays+1]} {incr i} {
	set x($i) [expr ($i-1)*$lbeam] 
}

# -----------------------------------------------------------------------------------
#in case of irregular plan dimensions, the x/y arrays can be set out of the above loops.
#Example:
# set x(0) 0.; set x(1) 5850.; and so on.
# Dont forget to enter values as floating (with a point at the end if no floating digits)
# -----------------------------------------------------------------------------------

#some parameters for displacement control analysis
# -----------------------------------------------------------------------------------
set roofNode [expr ($nflrs*100+1)*100+3]
set basenode 1
set LBuilding $y($nflrs)

#report the model properties in the log file
# -----------------------------------------------------------------------------------
foreach var {lcolumn lcolumnbase lbeam nflrs nbays roofNode basenode LBuilding Deadfloor Deadroof Livefloor Liveroof folder} {
	set value [set "$var"]
	puts $logfileId "$var = $value"
}

#define nodes
# -----------------------------------------------------------------------------------
#________________________________________________________________
#						  column                                |
#		 				     |                                  |
#		 				     |                                  |
#						     |						            |
#						     |						            |
#						     |						            |
#						     |						            |
#		 				     |	 node counter->nodeTag = ji2	|
#                         __(3)__    /                          |
#                        |panel  |  /                           |
#jth Story------------(4)|zone(2)|(2)------(3)-------------beam |
#                        |_______|          ^                   |
#						    (1)             |                   |
#						     |              |                   |
#						     |				|		            |
#						    (1)	----->element counter			|
#						     |						            |
#						ith Grid                                |
#_______________________________________________________________|						  

#we will define the plastic hinges at the ground supports using zeroLength elements
#other springs are will be define using the springs embedded in joint2d element
#So, an extra node is required at the supports which be used later

set j 0
	for {set i 1} {$i <= [expr $nbays+1]} {incr i} {
		#the node which will be connected to the column
		node $i $x($i) $y($j)
		
		#the extra node which will be fixed to support
		node [expr $i*10+1] $x($i) $y($j)
	}
	
for {set j 1} {$j <= $nflrs} {incr j} {
	set beamId [expr $j*10 + 2]
	set beamSec $memSec($beamId)
	source "general/sections/$beamSec.tcl"
	set hb $HSec
	set colId [expr $j*10 + 1]
	set colSec $memSec($colId)
	source "general/sections/$colSec.tcl"
	set hc $HSec
	for {set i 1} {$i <= [expr $nbays+1]} {incr i} {
		# if {$i == 1 || $i == [expr $nbays + 1]} {
			# set colId [expr $j*10 + 1]
		# } else {
			# set colId [expr $j*10 + 2]
		# }
		# set colSec $memSec($colId)
		# source "general/sections/$colSec.tcl"
		# set hc $HSec
		node [expr ($i+$j*100)*100+1] $x($i)					[expr $y($j)-$hb/2.]
		node [expr ($i+$j*100)*100+2] [expr $x($i)+$hc/2.]		$y($j)
		node [expr ($i+$j*100)*100+3] $x($i)					[expr $y($j)+$hb/2.]
		node [expr ($i+$j*100)*100+4] [expr $x($i)-$hc/2.]		$y($j)

		node [expr ($i+$j*100)*100+5] $x($i)					[expr $y($j)-$hb/2.]
		if {$i != [expr $nbays+1]}	{
			node [expr ($i+$j*100)*100+6] [expr $x($i)+$hc/2.]		$y($j)
		}
		if {$j != $nflrs} {
			node [expr ($i+$j*100)*100+7] $x($i)					[expr $y($j)+$hb/2.]
		}
		if {$i != 1} {
			node [expr ($i+$j*100)*100+8] [expr $x($i)-$hc/2.]		$y($j)
		}
	}
}
#Define ELEMENTS
# -----------------------------------------------------------------------------------
set PDeltaTag 1
set LinearTag 2
geomTransf PDelta $PDeltaTag       ; # for columns
geomTransf Linear $LinearTag       ; # for beams

# set K44 [expr 6*(1.+$nFactor)/(2.+3.*$nFactor)]
# set K33 [expr (1.+2.*$nFactor)*$K44/(1.+$nFactor)]
for {set j 1} {$j <= $nflrs } {incr j} {
	# set beamSec $memSec([expr $j*10+3])
	set beamSec $memSec([expr $j*10+2])
	for {set i 1} { $i <= [expr $nbays+1] } {incr i} {	
		set ND1	[expr ($i+$j*100)*100+1]
		set ND2	[expr ($i+$j*100)*100+2]
		set ND3	[expr ($i+$j*100)*100+3]
		set ND4	[expr ($i+$j*100)*100+4]
		set ND5	[expr ($i+$j*100)*100+5]
		set ND6	[expr ($i+$j*100)*100+6]
		set ND7	[expr ($i+$j*100)*100+7]
		set ND8	[expr ($i+$j*100)*100+8]
		set NDC	[expr ($i+$j*100)*100+9]
		if {$i == 1 || $i == [expr $nbays+1]} {
			set colID1 [expr $j*10+1]
			set colID2 [expr ($j+1)*10+1]
		} else {
			set colID1 [expr $j*10+2]
			set colID2 [expr ($j+1)*10+2]
		}
		if {$j == $nflrs} {set colID2 0}
		set colSec $memSec($colID1)
		set MAT1 $colMatId($colID1)
		set MAT3 $colMatId($colID2)
		set MAT2 $beamMatId($beamSec)
		set MAT4 $MAT2
		set MATC $rigidMatTag
		element Joint2D [expr ($j*100+$i)*100+2] $ND1 $ND2 $ND3 $ND4 $NDC $MATC 0
		#zeroLength elements
		element zeroLength [expr ($j*100+$i)*100+4] $ND5 $ND1 -mat $rigidMatTag $rigidMatTag $MAT1 -dir 1 2 6
		if {$i != [expr $nbays+1]}	{
			element zeroLength [expr ($j*100+$i)*100+5] $ND6 $ND2 -mat $rigidMatTag $rigidMatTag $MAT2 -dir 1 2 6
		}
		if {$j != $nflrs} {
			element zeroLength [expr ($j*100+$i)*100+6] $ND7 $ND3 -mat $rigidMatTag $rigidMatTag $MAT3 -dir 1 2 6
		}
		if {$i != 1} {
			element zeroLength [expr ($j*100+$i)*100+7] $ND8 $ND4 -mat $rigidMatTag $rigidMatTag $MAT4 -dir 1 2 6
		}

		# columns
		source "general/sections/$colSec.tcl"
		set Ic [expr ($nFactor+1)*$I33/$nFactor]
#		the column
		set nd1 [expr (($j-1)*100+$i)*100+7]
		if {$j == 1} {set nd1 $i}
		set nd2 [expr ($j*100+$i)*100+5]
		# element ModElasticBeam2d [expr ($j*100+$i)*100+1] $nd1 $nd2 $Area $e $Ic $K33 $K33 $K44 $PDeltaTag
		element elasticBeamColumn [expr ($j*100+$i)*100+1] $nd1 $nd2 $Area $Ec $Ic $PDeltaTag
	} 
	# beams 
	source "general/sections/$beamSec.tcl"
	set Ib [expr ($nFactor+1)*$I33/$nFactor]
	for {set i 1} { $i <= $nbays} {incr i} {
		set nd1 [expr ($j*100+$i)*100+6]
		set nd2 [expr ($j*100+$i+1)*100+8]
		# element ModElasticBeam2d [expr ($j*100+$i)*100+3] $nd1 $nd2 $Area $e $Ib $K33 $K33 $K44 $LinearTag
		element elasticBeamColumn [expr ($j*100+$i)*100+3] $nd1 $nd2 $Area $Ec $Ib $LinearTag
	}
}

#Forming the plastic hinge at the base of first story columns
set j 1
for {set i 1} {$i <= [expr $nbays+1]} {incr i} {
	if {$i == 1 || $i == [expr $nbays + 1]} {
		set colID [expr $j*10+1]
	} else {
		set colID [expr $j*10+2]
	}
	set MAT $colMatId($colID)
 	# Define plastic hinges for top of the bases						
	element zeroLength $i [expr $i*10+1] $i -mat $rigidMatTag $rigidMatTag $MAT -dir 1 2 6
}

#Define Leaning columns
# -----------------------------------------------------------------------------------
# set defLeanCol "No"
if {$defLeanCol == "Yes"} {
	node [expr $nbays+2] -$lbeam 0.
	fix [expr $nbays+2]  1 1 0
	for {set j 1} {$j <= $nflrs} {incr j} {
		node [expr $j*10+$nbays+2] -$lbeam $y($j)
		
		set sec $memSec([expr $j*10+1])
		source "general/sections/$sec.tcl"
		
		#element elasticBeamColumn $eleTag $iNode $jNode $A $E $Iz $transfTag
		element elasticBeamColumn [expr $j*10+1] [expr ($j-1)*10+$nbays+2] [expr $j*10+$nbays+2] [expr $Area*100.] $e [expr $I33/100.] $PDeltaTag
		
		#element truss $eleTag $iNode $jNode $A $matTag
		element truss [expr $j*10+2] [expr $j*10+$nbays+2] [expr ($j*100+1)*100+4] [expr $Area * 100.] $elasticMatTag
	}

	#apply gravity frame weights to the leaning columns 

	pattern Plain 10 Linear {
		for {set j 1} {$j <=  $nflrs} {incr j} {
			if {$j==$nflrs} {set dead $Deadroof ; set live $Liveroof} else {set dead $Deadfloor ; set live $Livefloor}
			set loadbay [expr $lbeam * $nbays]
			set loadarea [expr $loadbay*$perpLength]
			load  [expr $j*10+$nbays+2] 0. [expr -($deadSeisRat*$dead+$liveSeisRat*$live)*$loadarea] 0.
		}	
	}
}

#Assign Masses
# -----------------------------------------------------------------------------------
set totalMass 0.0
for {set j 1} {$j <= $nflrs} {incr j} {
	for {set i 1} {$i <= [expr $nbays+1]} {incr i} {
		if {$j==$nflrs} {set dead $Deadroof ; set live $Liveroof} else {set dead $Deadfloor ; set live $Livefloor}
		if {$i==1 || $i==[expr $nbays+1]} {set loadbay [expr $lbeam/2.]} else {set loadbay $lbeam}
		set loadarea [expr $loadbay*$perpLength]
		set nodid [expr ($j*100+$i)*100+2]
		set massvalue [expr ($deadSeisRat*$dead+$liveSeisRat*$live)*$loadarea/($g)]
		set totalMass [expr $totalMass + $massvalue]
		mass $nodid $massvalue 0 0
		# puts "$nodid	$massvalue"
	}
}	
puts $logfileId "totalMass= $totalMass"

#Define Pushover Load Pattern Distribution Factors
# -----------------------------------------------------------------------------------
set width [expr $nbays*$lbeam]
set wfloor [expr ($width*$perpLength)*($deadSeisRat*$Deadfloor+$liveSeisRat*$Livefloor)]
set wroof [expr ($width*$perpLength)*($deadSeisRat*$Deadroof+$liveSeisRat*$Liveroof)]
set totalw [expr $wfloor*($nflrs-1)+$wroof]
set kpow 1.
set sumwiyi_k 0 
for {set j 1} {$j <= [expr $nflrs-1]} {incr j} {
	set sumwiyi_k [expr $sumwiyi_k+($y($j)**$kpow)*$wfloor]
}
set sumwiyi_k [expr $sumwiyi_k+($y($nflrs)**$kpow)*$wroof]
for {set j 1} {$j <= [expr $nflrs-1]} {incr j} {
	set fi($j) [expr  $wfloor*($y($j)**$kpow)*$totalw / $sumwiyi_k]
}
set fi($nflrs) [expr  $wroof*($y($nflrs)**$kpow)*$totalw / $sumwiyi_k]


#Soil-Structure Interaction/Base Support Modeling
# -----------------------------------------------------------------------------------
set baseNodes [list]
for {set i 1} {$i <= [expr $nbays+1]} {incr i} {
	set tag [expr $i*10 + 1]
	# set tag $i
	lappend baseNodes $tag
}
foreach tag $baseNodes {
	fix $tag 1 1 1
}

#Define RECORDERS
# -----------------------------------------------------------------------------------
for {set j 1} {$j <= $nflrs} {incr j} {
 	set nd1 [expr (($j-1)*100+1)*100+2]
	if {$j == 1} {set nd1 1}
	set nd2 [expr ($j*100+1)*100+2] 
	recorder Drift -file $folder/drifts/$j.txt -time -iNode $nd1 -jNode $nd2 -dof 1 -perpDirn 2
	recorder EnvelopeDrift -file $folder/envelopdrifts/$j.txt -time -iNode $nd1 -jNode $nd2 -dof 1 -perpDirn 2
	for {set i 1} {$i <= [expr $nbays + 1]} {incr i} {
		set tag [expr ($j*100+$i)*100+1]
		recorder Element -file $folder/ColumnF/$tag.txt -time -dT 0.02 -ele $tag globalForce
		for {set k 1} {$k <= 4} {incr k} {
			set eleTag [expr ($j*100+$i)*100+3+$k]
			set fileTag [expr ($j*10+$i)*10+$k]
			recorder Element -file $folder/Hinges/$fileTag.txt -time -ele $eleTag material 3 stressStrain
		}
	}
	recorder EnvelopeNode -file $folder/StoryDisps/$j.txt -time -node $nd2 -dof 1 disp
	
}
for {set i 1} {$i <= [expr $nbays + 1]} {incr i} {
	set eleTag $i
	set fileTag $i
	recorder Element -file $folder/Hinges/$fileTag.txt -time -ele $eleTag material 3 stressStrain
}
foreach tag $baseNodes {
	recorder Node -file $folder/BaseReacts/$tag.txt -time -dT 0.02 -node $tag -dof 1 2 3 reaction
}

if {$displayFrame == "Yes"} {
	recorder display Frame 10 10 500 500 -wipe 
	vup 0 1 0       
	prp 0 0 1000000
	display 1 1 1
}
# while 1 {}

#Print Natural Period of Model
# -----------------------------------------------------------------------------------
set lambda [eigen 3]
foreach i {0 1 2} {
	set lambdai [expr abs([lindex $lambda $i])]
	set omega [expr $lambdai**0.5]
	set T [expr 2*3.1416/$omega]
	puts $logfileId "T$i= $T"
	puts "T$i= $T"
}

#save first mode period for later use
set mode1 0
set lambda [expr abs([lindex $lambda $mode1])]
set omega [expr $lambda**0.5]
set Tperiod [expr 2*3.1416/$omega]
puts "Tperiod=	$Tperiod"
#End
# -----------------------------------------------------------------------------------

# exit