
#-----------------------------dimensions -----------------------------------------------------
set lcolumnbase 320. ; # in cm
set lcolumn 320. 
set lbeam 500. 
set nbays 5
set nflrs 5
set perpWidth $lbeam
set perpLength $lbeam		;#in case of perimeter moment frame system, set this equal to half of plan length in perp. direction
set targetDrift 0.1			;# for pushover
# set displayFrame "Yes"
set displayFrame "No"
# set doCheck "Yes"
set doCheck "No"
set designFile "5-st"
set nFactor 1				;#ratio between the stiffnesses of the springs and the beams in-between
set defLeanCol "No"

#settings used in rayleigh damping definition
set nEigenI 1;					# mode 1
set nEigenJ 2;					# mode 2                              
set xDamp 0.05;					# damping ratio
set MpropSwitch 1.0;
set KcurrSwitch 0.0;
set KcommSwitch 1.0;
set KinitSwitch 0.0;

set concDens 2500.e-6		;#KgF/cm3

#-----------------------------calculate distributed gravity Loads-----------------------------
#nominal gravity loadings: partition loads must be included as LIVE load
set Deadfloor [expr 550.e-4] ; # kgf/cm2 for floors
set Deadroof [expr 550.e-4]
set Livefloor [expr 200.e-4] 
set Liveroof [expr 200.e-4]
set deadSeisRat 1.0
set liveSeisRat 0.2
