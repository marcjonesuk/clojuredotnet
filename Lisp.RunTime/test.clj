; calculates the factorial for n
(defn fact [n] 
	(loop [current n next (dec current) total 1] 
		(if (> current 1) 
			(recur next (dec next) (* total current)) total))) 
			
(deftest 'factorial' 
	(= (fact 10) 3628800))

(defn test [name expr expected] 
	(deftest name 
		(= expr expected)))

(def data { :name "marc" :age 38 })

; first tests
(test "nil" 
	(first nil) nil)

(test "empty" 
	(first []) nil)

(test "empty quoted list" 
	(first `()) nil)

(test "single item list" 
	(first [1]) 1)

(test "list" 
	(first [1 2]) 1)

(test "list of lists" 
	(first [[1 2] [3 4]]) [1 2])

(test "quoted list" 
	(first `(1 2)) 1)
  
(test "string" 
	(first "abc") "a")

(test "hashmap" 
	(first {:key 1}) [:key 1])

(test "range" 
	(first (range)) 0)

; more tests
(test "x"
	(next [1 2]) [2])



(defn take2 [coll, count] 
	(System.Linq.Enumerable/Take (seq x) count)
)

(print (take2 [1 2 3] 2))

