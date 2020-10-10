proc getMax {columnin indexin} {
	upvar $columnin column
	upvar $indexin ind
	set indices [lsort -integer [array names column]]
	set max -1.e30
	set ii 0
	foreach index $indices {
		if {$column($index) > $max} {
			set max $column($index)
			set ind $index
		}
	}
	return $max
}