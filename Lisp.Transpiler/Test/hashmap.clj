



; one key
(let [map1 { "key" 1 }]
	(print (= 1 (get map1 "key"))))

; two keys
(let [map2 { "key" 1 "key2" 2 }]
	(print (= 2 (get map2 "key2"))))

; later value should overwrite previous
(let [map { "key" 1 "key" 2 }]
	(print (= 2 (get map "key"))))

; cant escape double quote yet
(let [map { "key" 1 }]
	(print (= 1 (str map))))

(let [map { "key" 1 "key" 2 }]
	(print (= 1 (count map))))

(let [map { "key" 1 }] 
	(print (type map)))

