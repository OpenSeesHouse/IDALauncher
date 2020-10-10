#procedure for finding sa
proc spectrum {inspec	Tperiod} {
set in [open $inspec r]
set time 0
set state 0
set n 1
while {$time <= 4 && $state != -1} {
	set state [gets $in list]
	if {$state == -1} continue
	set t($n) [lindex $list 0]
	set acc($n) [lindex $list 1]
	incr n
}
set i 0
set ok "no"
set y 0.
while {$ok == "no"} {
	incr i
	if {$i >= $n} break
	if {$Tperiod > $t($i)} continue
	# puts "$t($n)	$Tperiod	$t([expr $n-1])"
	set x $Tperiod
	set x2 $t($i) ; set x1 $t([expr $i-1])
	set y2 $acc($i) ; set y1 $acc([expr $i-1])# puts "1- x=$x, x1=$x1, x2= $x2"

	set m [expr ($y2-$y1)/($x2-$x1)]
	set y [expr $y1+$m*($x-$x1)]
	set ok "yes"
}
if {$y == 0} {
	set n [expr $n-1]
	set x $Tperiod
	set x2 $t($n) ; set x1 $t([expr $n-1])
	set y2 $acc($n) ; set y1 $acc([expr $n-1])

	set m [expr ($y2-$y1)/($x2-$x1)]
	set y [expr $y1+$m*($x-$x1)]
# puts "2- x=$x, x1=$x1, x2= $x2, y= $y"
	
}
return	$y
}