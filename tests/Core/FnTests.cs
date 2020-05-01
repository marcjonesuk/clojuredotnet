using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class FnTests : BaseTest
	{
		[TestMethod] public void Fn_1() { AssertThat("((fn [a b c] (+ a b c)) 2 4 6)", 12.0); }
		[TestMethod] public void Fn_2() { AssertThat("((fn [a] (+ a 2)) 1)", 3.0); }
		[TestMethod] public void Fn_3() { AssertThat(@"(def myfunc (fn [a] (+ a 2)))(myfunc 5)", 7.0); }
		[TestMethod] public void Fn_4() { AssertThrows("((fn [a] (+ a 2)) 1 2)", typeof(ArityException)); }
		[TestMethod] public void Fn_7() { AssertThat("(defn myfn [a] (+ 1 0))(myfn 0)", 1.0); }
		[TestMethod] public void Fn_7_1() { AssertThat("(defn myfn [a & more] (str more))(myfn 0 1 2)", "(1 2)"); }
		[TestMethod] public void Fn_8() { AssertThat("(defn myfn ([a] 1) ([a b] 2))(myfn 0)", 1.0); }
		[TestMethod] public void Fn_9() { AssertThat("(defn myfn ([a] 1) ([a b] 2))(myfn 0 1)", 2.0); }
		[TestMethod] public void Fn_9_1() { AssertThrows("(defn myfn ([a] 1) ([a b] 2))(myfn 0 1 2)", typeof(ArityException)); }
		[TestMethod] public void Fn_10() { AssertThat("(defn myfn ([a] 1) ([a b] 2))(+ (myfn 0 1) (myfn 0))", 3.0); }

		[TestMethod] public void Fn_11() { AssertThat(@"
(defn div1
  'If no denominators are supplied, returns 1/numerator,
  else returns numerator divided by all of the denominators.'
  ([x] (/ 1 x))
  ([x y] (clojure.lang.Numbers/Divide x y))
  ([x y & more]
   (reduce / (/ x y) more)))(div1 10 5)", 2.0); }
	}
} 