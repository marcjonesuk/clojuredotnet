
(defn inc2 [x] (+ x 2))

(print (inc2 (inc2 0)))



; (.ToString [1 2] "") - does not work
; this is a comment
(def x 5)

(defn fact [n] (loop [current n next (dec current) total 1] (if (> current 1) (recur next (dec next) (+ total current)) total))) 

(time (print "Running"))
(print (time (fact 1000000)))

	(time 
		(loop [i 0 v 0] 
			(if (> i 1000000) 
				(+ 10 20)
				(recur (inc i) (+ 10 20))
			)
	))

	(time 
		(loop [i 0 v 0] 
			(if (> i 1000000) 
				(+ 10 20)
				(recur (inc i) (+ 10 20))
			)
	))

	(time 
		(loop [i 0 v 0] 
			(if (> i 1000000) 
				(+ 10 20)
				(recur (inc i) (+ 10 20))
			)
	))

; V0.1
; Hashmaps
; Math
; Multi-arity functions





