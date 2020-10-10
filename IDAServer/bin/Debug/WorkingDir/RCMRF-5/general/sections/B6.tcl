#Section dimensions and properties in cm:
set HSec 			40.
set BSec 			30.
set coverT 			5.
set coverB 			5.
set numBarsTop 		2
set barDTop			1.8
set barAreaTop 		[expr 3.14*$barDTop*$barDTop/4.]
set numBarsBot 		2
set barDBot			1.8
set barAreaBot 		[expr 3.14*$barDBot*$barDBot/4.]
#An even number providing the total number of intermediate bars (bars other than top and bottom)
set numBarsIntTot 	0
set barDInt			0.
set barAreaInt		[expr 3.14*$barDInt*$barDInt/4.]
set Area			[expr $HSec * $BSec]
set I33				[expr $BSec * pow($HSec, 3.)/12.]
