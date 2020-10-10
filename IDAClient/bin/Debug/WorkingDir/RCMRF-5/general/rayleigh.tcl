#----------------------------Define and apply Damping -----------------------------------------
if {$nflrs > 1} {
	set lambda [eigen [expr $nEigenJ+1]]
	set lambda1 [lindex $lambda [expr $nEigenI-1]]
	set lambda2 [lindex $lambda [expr $nEigenJ-1]]
	set omegaI [expr $lambda1**0.5];
	set omegaJ [expr $lambda2**0.5];
	set alphaM [expr $MpropSwitch*$xDamp*(2.*$omegaI*$omegaJ)/($omegaI+$omegaJ)];	# M-prop. damping; D = alphaM*M
	set betaKcurr [expr $KcurrSwitch*2.*$xDamp/($omegaI+$omegaJ)];         		# current-K;      +beatKcurr*KCurrent
	set betaKcomm [expr $KcommSwitch*2.*$xDamp/($omegaI+$omegaJ)];   		# last-committed K;   +betaKcomm*KlastCommitt
	set betaKinit [expr $KinitSwitch*2.*$xDamp/($omegaI+$omegaJ)];         			# initial-K;     +beatKinit*Kini
	rayleigh $alphaM $betaKcurr $betaKinit $betaKcomm;
}
