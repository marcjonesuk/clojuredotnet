

(defn multifn
	([] (+ 1 2))
	([x] (* x x))
)

(print (multifn))
(print (multifn 5))
(print "end")
; (print (= 2 (multifn 0 0)))
; (print (= 3 (multifn 0 0 0)))
