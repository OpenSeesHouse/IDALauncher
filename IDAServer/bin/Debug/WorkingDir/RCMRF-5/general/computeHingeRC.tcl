#a long procedure just to compute section My!
#for steel sections, My is simply computed by My = zfy

#UNITS: any consistent units can be used. Just provide the cunit for converting fc to MPa when required
# set cunit 0.1
set pi [expr 4.0*atan(1.)]
set Av [expr 2.0*$pi*pow($dv,2)/4.0];  #area of hoop bars
set Ec [expr 5000. * sqrt($fc*$cunit)/$cunit]
# puts "Ec= $Ec"
set n [expr $rebarE/$Ec]
set d [expr $HSec-$coverT-$dv-.5*$barDBot];  # effective depth of tensile steel
set d1 [expr $HSec-$d];  # effective depth of compr. steel
set delta [expr $d1/$d]
set esy [expr $rebarFy/$rebarE];  # long Steel yield strain
set ecu .003
set cb [expr $ecu*$d/($ecu+$esy)]; # depth of compression block at balanced
set As [expr $numBarsBot*$barAreaBot]; # tension steel without intermediate bars
set rhos [expr $As/($d*$BSec)]
set Asf  [expr $numBarsTop*$barAreaTop]; # compression steel without intermediate bars
set rhosf [expr $Asf/($d*$BSec)]
set Asw [expr $numBarsIntTot*$barAreaInt];  # intermediate steel
set rhosw [expr $Asw/($d*$BSec)]
set rhotot [expr $rhos+$rhosf+$rhosw];   # reinf.ratio for all longitudinal steel
if {($fc*$cunit) <= 27.6 } {set beta .85} else {set beta [expr 1.05-0.05*($fc*$cunit/6.9)]}
set c [expr ($As*$rebarFy-$Asf*$rebarFy+$N)/(.85*$fc*$BSec*$beta)] ; # estimate based on all assumptions
if {$c < $cb} {set A [expr $rhotot+($N/($BSec*$d*$rebarFy))]} else {set A [expr $rhotot-($N/(1.8*$n*$BSec*$d*$fc))]} ;   # tension control indicator to use from fardis
if {$c < $cb} {set B [expr $rhos+$rhosf*$delta+.5*$rhosw*(1+$delta)+($N/($BSec*$d*$rebarFy))]} else {set B [expr $rhos+$rhosf*$delta+.5*$rhosw*(1+$delta)]} ;  #tension control indicator to use from fardis
set ky [expr (($n**2)*($A**2)+2*$n*$B)**.5-$n*$A] ; #compression zone depth (norm by d)
if {$c<$cb} {set phiy [expr $rebarFy/($rebarE*(1-$ky)*$d)]} else {set phiy [expr 1.8*$fc/($Ec*$ky*$d)]}
set asl 1 ; #slip term
set v [expr $N/($Area*$fc)]; #axial load ratio
set rhosh [expr $Av/($s*$BSec)] ; #stirrup area ratio
set term1 [expr $Ec * (($ky**2)/2.) * (0.5*(1+$delta)-$ky/3.)]
set term2 [expr $rebarE/2. * ((1-$ky) * $rhos + ($ky-$delta)*$rhosf + $rhosw/6.*(1-$delta)) * (1-$delta)]

#and finally:
set my [expr $BSec*($d**3) * $phiy * ($term1 + $term2)]; 
set krat [expr 0.17+1.61*$v]
if {$krat < 0.35} {set krat 0.35}
if {$krat > 0.8} {set krat 0.8}
set mcRat [expr 1.25*(0.89**$v)*(0.91**(0.01*$cunit*$fc))]
# foreach var {d d1 delta esy cb As rhos Asf rhosf Asw rhosw rhotot beta c Ec n A B ky phiy v rhosh term1 term2 my} {
	# set value [set "$var"]
	# puts "$var = $value"
# }

set mc [expr $my*$mcRat]
set tetac [expr .14*(1+.4*$asl)*(pow(.19,$v))*(pow(.02+40*$rhosh,.54))*(pow(.62,.01*$fc*$cunit))]
set tetapc [expr .76*(pow(.031,$v))*(pow(.02+40*$rhosh,1.02))]
if {$tetapc > 0.1} {set tetapc 0.1}
set tetapc [expr $tetapc+$tetac]
set gama [expr 170.7*(pow(.27,$v))*(pow(.1,$s/$d))]
# puts "v= $v,	gama= $gama"

set EIg [expr $Ec*$I33]
set ke [expr $krat*$EIg*6./$ls]
set tetay [expr $my/$ke]
set alfah [expr ($mc-$my)/($tetac-$tetay)/$ke]
set alfac [expr -$mc/($tetapc-$tetac)/$ke]

set gama [expr $gama*($nFactor+1)]
set ke [expr ($nFactor+1)*$ke]
set alfah [expr $alfah/(1-($alfah-1)*$nFactor)]
set alfac [expr $alfac/(1-($alfac-1)*$nFactor)]

set tetac [expr ($my/$ke )+(($mc-$my)/($alfah*$ke))]
set R 0; #The residual moment
