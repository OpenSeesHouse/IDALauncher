#              ------------------------------------------------------------------------------
#                                    2D Frame --  analyze gravity
#              ------------------------------------------------------------------------------


#Distributed Gravity Forces have been applied through stripModel command and no additional loading is required here
#--------------------Gravity-analysis Parameters(load-controlled static analysis)-------------
pattern Plain 1 Linear {
	for {set j 1} {$j <= $nflrs} {incr j} {
		for {set i 1} {$i <= [expr $nbays]} {incr i} {
			set beamTag [expr ($j*100+$i)*100+3]
			if {$j==$nflrs} {
				set dead $Deadroof ; set live $Liveroof
			} else {
				set dead $Deadfloor ; set live $Livefloor
			}
			set load [expr $deadSeisRat*$dead+$liveSeisRat*$live]
			eleLoad -ele $beamTag -type -beamUniform [expr -$load*$perpWidth]
			# puts "[expr -$load*$perpWidth]"
		}
	}
}

constraints Transformation
numberer RCM
system BandGeneral
test NormDispIncr 1.0e-8 100 0
algorithm Newton
integrator LoadControl 0.1
analysis Static
analyze 10
#--------------------Maintain Constant Gravity Loads and Reset Time to Zero-------------------
loadConst -time 0.0
puts "gravity done"
recorder Drift -file $folder/globalDrift.txt -time -iNode $basenode -jNode $roofNode -dof 1 -perpDirn 2
# recorder plot $folder/globaldrift.out globalDrift 500 300 500 400 -columns 2 1
#-----------------------------Define Strips------------------------------------------
