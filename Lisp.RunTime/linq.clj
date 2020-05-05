
(defn count
	[coll] (System.Linq.Enumerable/Count<System.Object> coll))

(defn first 
	[coll] (System.Linq.Enumerable/FirstOrDefault<System.Object> (seq coll)))

(defn skip
	[coll, count] (System.Linq.Enumerable/Skip<System.Object> (seq coll) count))

(defn where
	[coll, predicate] (RT/MarcWhere (seq coll) predicate))

(deftest "where test"
	(= [1] (where [1 2 3] (fn [x] (< x 2)))))

; (defn where	[coll, predicate] (RT/MarcWhere (seq coll) predicate)) (where [1 2 3] (fn [x] (< x 2)))

; (deftest "first should on vector should return first item"
; 	(= 1 (first [1 2])))

; (deftest "first with no args should throw arity exception" 
; 	(= nil (first)))

; (deftest "first on nil should return nil" 
; 	(= nil (first nil)))

; (deftest "first on empty vector should return nil"
; 	(= nil (first [])))

(deftest "skip 1"
	(= [2 3] (skip [1 2 3] 1)))

(deftest "skip 2"
	(= [] (skip [1 2 3] 4)))

(deftest "skip 3"
	(= [1 2] (skip [1 2] 0)))

(deftest "skip with negative should return vector"
	(= [1 2] (skip [1 2] -5)))

(defn take
	[coll, count] 
		(System.Linq.Enumerable/Take<System.Object> (seq coll) count))

(defn takewhile
	[coll, predicate] 
		((interop "System.Linq.Enumerable/TakeWhile<System.Object>(IEnumerable`1[System.Object],Func`2[System.Object,System.Boolean]") (seq coll) predicate)
)

; (takewhile [1 2 3] (fn [item] true))

(defn myprint 
	[msg] (System.Console/WriteLine msg))

(myprint "yoooo")

(defn +_ [x y] (RT/Add x y))

(print (+_ 10 20))

