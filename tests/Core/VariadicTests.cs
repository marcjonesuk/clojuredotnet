using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class VariadicTests : BaseTest
	{
		[TestMethod] public void Add_1() { AssertThat("(defn x [a & more] (apply str more))(x 'a' 'b' 'c')", "bc"); }
		[TestMethod] public void Add_2() { AssertThat("(defn x ([a b] (+ a b)) ([a & more] (apply str a more)))(x 'a' 'b' 'c')", "abc"); }
		[TestMethod] public void Add_3() { AssertThat("(defn x ([a b] (+ a b)) ([a & more] (apply str more)))(x 'a' 'b')", "ab"); }



		// 		[TestMethod] public void Add_4() { AssertThat(@"(defn add ([] 0) ([x] (x))(add 1)
		// //   ([x y] (+ x y))
		// //   ([x y & more]
		// //      (reduce + (add x y) more)))
		// // 	 (x 1 2 3)
		// // 		", 6.0); }
		// // 	}


		[TestMethod] public void Not() { AssertThat(@"
	(defn not
  'Returns true if x is logical false, false otherwise.'
  {:tag Boolean
   :added '1.0'
   :static true}
  [x] (if x false true))(not 1)", false); }



		[TestMethod] public void Add_4() { AssertThat(@"
(defn add 
  ([] 0)
  ([x] (x))
  ([x y] (+ x y))
  ([x y & more]
     (reduce add (add x y) more)))
	 (add 1 2 3 4 5 6 7)
		", 28.0); }
	}




	// (defn true?
	//   [x] (clojure.lang.Util/identical x true))
}