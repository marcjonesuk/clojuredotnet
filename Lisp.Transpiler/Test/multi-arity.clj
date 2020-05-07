

(defn multifn
	([] (+ 1 2))
	([x] (* x x))
)

; ((x, y) => (+ x 1))


(print (multifn))
(print (multifn 5))
(print "end")
; (print (= 2 (multifn 0 0)))
; (print (= 3 (multifn 0 0 0)))


(loop [last nil] 
	(do
		(print "hello world")
		(if (= last "exit")
			nil
			(recur (read-line)))))
