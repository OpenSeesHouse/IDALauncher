#-------------------------------------------------------------------------------------------------------
#                    analyze frame 2D modl under UniformExcitation grounmotion 
#-------------------------------------------------------------------------------------------------------
# input parameters :
# GMfilePath  :  ground motion records file path
# deltaT
# fac         : scaling factor
#--------------------------------Define Load Pattern----------------------------------------------------
pattern UniformExcitation 2 1 -accel "Series -dt $deltaT -filePath $GmFile -factor $fac"
#--------------------------------Set Analysis options---------------------------------------------------
set Tol 1.e-7
constraints Transformation
numberer RCM
system BandGeneral
algorithm Newton
integrator Newmark 0.5 0.25 
analysis Transient
test NormDispIncr 1.e-7 30 0
set dvrgFlag 0
set contin YES
set algoType "Newton"
set testType "NormDispIncr"
puts $logfileId "Tmax= $Tmax"
set dt1 $deltaT
set test1 1.0e-7
set ok 0
set minStepRat 1.e-3
set curT [getTime]
set DT [expr $Tmax - $curT]
while {$DT > $deltaT} {
	set dt $dt1
	set curT [getTime]
	set DT [expr $Tmax - $curT]
	set num [expr int($DT / $dt) + 1]
	set test $test1
	test $testType $test 30 0
	puts $logfileId "-----------Running: algoType:$algoType; testType:$testType; curT=$curT, DT=$DT, dt=$dt, test= $test-----------"
	set ok [analyze $num $dt]
	set contin YES
	set curT [getTime]
	set DT [expr $Tmax - $curT]
	while {$ok != 0 && $DT > $deltaT} {
		set curT [getTime]
		# set DT [expr $Tmax - $curT]
		puts $logfileId "-------------failure at: $curT-------------"
		puts $logfileId "-------------trying alternative algorithms-------------"
		foreach algoType {NewtonLineSearch ModifiedNewton KrylovNewton BFGS Broyden} param {0.65 "" "" "" ""} {
			algorithm $algoType $param
			# puts $logfileId "-------------trying alternative testTypes-------------"
			foreach testType {NormDispIncr} {
			# NormUnbalance EnergyIncr
				set curT [getTime]
				# set DT [expr $Tmax - $curT]
				set num [expr int(0.1/$dt) + 1]
				test $testType $test 10 0
				puts $logfileId "-----------Trying: algoType:$algoType; testType:$testType; curT=$curT, dt=$dt, test= $test-----------"
				set ok [analyze $num $dt]
				if {$ok == 0} {
					set dt $dt1
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
		if {$ok != 0} {
			set dt [expr $dt/3.1623]
			set test [expr $test*3.1623]
			if {[expr $dt/$dt1] < $minStepRat} {
				puts $logfileId "----------- minStepRat= $minStepRat minimum step size reached-----------\n"
				set dvrgFlag 1
				set contin NO
				break
			}
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
set endTime [getTime]
puts $logfileId "current time: [getTime]"
puts $logfileId "-----------------------------------------------------------------"
puts "current time: [getTime]"
puts "-----------------------------------------------------------------"
wipe