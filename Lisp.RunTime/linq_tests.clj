(deftest "where test"
	(= [1] (where [1 2 3] (fn [x] (< x 2)))))
 
(deftest "first should on vector should return first item"
	(= 1 (first [1 2])))

; (deftest "first with no args should throw arity exception" 
; 	(= nil (first)))

(deftest "first on nil should return nil" 
	(= nil (first nil)))

(deftest "first on empty vector should return nil"
	(= nil (first [])))

(deftest "skip 1"
	(= [2 3] (skip [1 2 3] 1)))

(deftest "skip 2"
	(= [] (skip [1 2 3] 4)))

(deftest "skip 3"
	(= [1 2] (skip [1 2] 0)))

(deftest "skip with negative should return vector"
	(= [1 2] (skip [1 2] -5)))

