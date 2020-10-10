#Materials
#Unconfined Concrete material
set UnconfFc -250.0; 						#in Kgf/cm2
# set Ec [expr 51000.*sqrt(-1.*$UnconfFc/10); #concrete modulus
set Ec [expr 234000.]; #concrete modulus
set nu 0.2;
set Gc [expr $Ec/2./[expr 1+$nu]];  	# Torsional stiffness Modulus
set UnconfFcu -57.6 ;
set UnconfEc [expr 2.*$UnconfFc/$Ec];
set UnconfEcu -0.008;
#Confined Concrete material
set ConfFc [expr 1.09*$UnconfFc]
set ConfEc [expr 1.2*$UnconfEc] 
set ConfFcu [expr 1.86*$UnconfFcu]
set ConfEcu [expr 3.75*$UnconfEcu]

#Ultimate Tension Strain
set Etu 0.002

set rebarE 2.1e6 ; set rebarFy 4000.			;#in KgF/cm2 units

set rebarMatTag 1
uniaxialMaterial Steel02 1 $rebarFy $rebarE 0.003 18.5 .925 .15

# An elastic rigid material for Constrained DOFs and panel zone deformations
set rigidMatTag 2
uniaxialMaterial Elastic 2 1e16

#elastic material used for defining leaning columns
# set elasticMatTag 3
# uniaxialMaterial Elastic 3 $Ec

set confConcMat 4
set unconfConcMat 5
uniaxialMaterial Concrete01 4 $ConfFc $ConfEc $ConfFcu $ConfEcu
uniaxialMaterial Concrete01 5 $UnconfFc $UnconfEc $UnconfFcu $UnconfEcu
# uniaxialMaterial Concrete02 4 $ConfFc $ConfEc $ConfFcu $ConfEcu 0.1 [expr -0.1*$ConfFc] [expr -0.1*$ConfFc/$Etu]
# uniaxialMaterial Concrete02 5 $UnconfFc $UnconfEc $UnconfFcu $UnconfEcu 0.1 [expr -0.14*$UnconfFc] [expr -0.14*$UnconfFc/$Etu]


source ../general/DesignOutput/$designFile.tcl
source ../general/BuildRCrectSection.tcl

set SecList [list ]
set indices [lsort -integer [array names memSec]]
foreach index $indices {
	set sec $memSec($index)
	lappend SecList $sec
}

# Fiber sections for columns
set nfCoreY 5
set nfCoreZ 5
set nfCoverY 5
set nfCoverZ 5
set coreID $confConcMat
set coverID $unconfConcMat

set ID 0
set secID(0) 0
foreach sec $SecList {
	set secID($sec)  [incr ID]
	# I-section secID matID d bf tf tw nfdw nftw nfbf nftf
	source ../general/sections/[set sec].tcl
	set Ifac 0.35
	if {[string index $sec 0] == "C"} {
		set Ifac 0.7
	}
	set I22 [expr $HSec * pow($BSec, 3.)/12.]
	set JSec [expr 0.141*pow($HSec,4)]
	section Elastic [expr $ID*100+2] $Ec $Area [expr $Ifac*$I33] [expr $Ifac*$I22] $Gc $JSec
	# BuildRCrectSection [expr $ID*100+1] $HSec $BSec $coverT $coverB $coreID $coverID $rebarMatTag $numBarsTop $barAreaTop $numBarsBot $barAreaBot $numBarsIntTot $barAreaInt $nfCoreY $nfCoreZ $nfCoverY $nfCoverZ
	# section Aggregator $ID $rigidMatTag T -section [expr $ID*100+1]
	section Elastic $ID $Ec $Area [expr $Ifac*$I33] [expr $Ifac*$I22] $Gc $JSec
}
