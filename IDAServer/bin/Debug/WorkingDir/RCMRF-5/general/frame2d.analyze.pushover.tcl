#apply previously calculated pushover story loads to story nodes
pattern Plain 3 Linear {
	for {set j 1} {$j <= $nflrs} {incr j 1} {
		for {set i 1} {$i <= [expr $nbays+1]} {incr i} {
			set nodeid [expr ($j*100+$i)*100+3]
			set loadVal [expr $fi($j)/($nbays+1)]
			load $nodeid $loadVal 0.0 0.0
			# puts "$loadVal"
		}
	}
}

constraints Transformation
numberer RCM
system BandGeneral
analysis Static
algorithm Newton
test NormDispIncr 1.e-7 100 0

set contin YES
set algoType "Newton"
set testType "NormDispIncr"
set targetDisp [expr $targetDrift*$LBuilding]
puts $logfileId "targetDisp= $targetDisp"
set incr1 1.e-1
	# integrator DisplacementControl $roofNode 1 $incr1
	# set ok [analyze 1]
	# return
set test1 1.0e-7
set ok 0
set minStepRat 1.e-3
set curD [nodeDisp $roofNode 1]
set deltaD [expr $targetDisp - $curD]
while {$deltaD > 1.e-2} {
	set incr $incr1
	set num [expr int($deltaD / $incr) + 1]
	set test $test1
	test $testType $test 100 0
	puts $logfileId "-----------Running: algoType:$algoType; testType:$testType; curD=$curD, deltaD=$deltaD, incr=$incr, test= $test-----------"
	integrator DisplacementControl $roofNode 1 $incr
	set ok [analyze $num]
	set contin YES
	set curD [nodeDisp $roofNode 1]
	set deltaD [expr $targetDisp - $curD]
	while {$ok != 0 && $deltaD > 1.e-2} {
		set curD [nodeDisp $roofNode 1]
		set deltaD [expr $targetDisp - $curD]
		puts $logfileId "-------------failure at: $curD-------------"
		puts $logfileId "-------------trying alternative algorithms-------------"
		foreach algoType {NewtonLineSearch ModifiedNewton KrylovNewton BFGS Broyden} param {0.65 "" "" "" ""} {
			algorithm $algoType $param
			# puts $logfileId "-------------trying alternative testTypes-------------"
			foreach testType {NormDispIncr} {
			# NormUnbalance EnergyIncr
				set curD [nodeDisp $roofNode 1]
				set deltaD [expr $targetDisp - $curD]
				set num [expr int($deltaD/$incr) + 1]
				test $testType $test 100 0
				puts $logfileId "-----------Trying: algoType:$algoType; testType:$testType; curD=$curD, deltaD=$deltaD, incr=$incr, test= $test-----------"
				integrator DisplacementControl $roofNode 1 $incr
				set ok [analyze $num]
				if {$ok == 0} {
					set incr $incr1
					set test $test1
					break
				}
			}
			if {$ok == 0} {
				break
			}
		}
		if {$ok == 0} {
			break
		}
		set incr [expr $incr/2.]
		set test [expr $test*3.162278]
		if {[expr $incr/$incr1] < $minStepRat} {
			puts $logfileId "----------- minStepRat= $minStepRat minimum step size reached-----------\n"
			set contin NO
			break
		}
	}
	if {$contin == NO} {break}
}
## -----------------------------------------------------------------------------------------------------
if {$ok != 0 } {
	puts $logfileId "-------------------analysis INTERRUPTED-------------------"
	puts "-------------------analysis INTERRUPTED-------------------"
} else {
	puts $logfileId "-------------------analysis COMPLETED-------------------"
	puts "-------------------analysis COMPLETED-------------------"
}
puts $logfileId "current drift: [expr [nodeDisp $roofNode 1]/ $LBuilding]"
puts $logfileId "-----------------------------------------------------------------"
puts "current drift: [expr [nodeDisp $roofNode 1]/ $LBuilding]"
puts "-----------------------------------------------------------------"
wipe