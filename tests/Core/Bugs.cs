using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class Bugs : BaseTest
	{
		[Ignore] [TestMethod] public void Bug_1() { AssertThat("(let [y 1] (def z (+ y 1))y)", 1.0); }
		[Ignore] [TestMethod] public void Bug_2() { AssertThat("(let[a 1 b 2](+ a b))", 3.0); }
		[TestMethod] public void Bug_TooMuchWhiteSpace() { AssertThat("  (str  's' ) ", "s"); }

		[Ignore] [TestMethod] public void Bug_HashMapCanContainCommas() { AssertThat("(str (hash-map :key1 1, :key2 2))", "{ :key1 1, :key2 2 }"); }
		[Ignore] [TestMethod] public void Bug_IntegerIsNotADouble() { AssertThat("(+ 1)", 1); }

		[TestMethod] public void Bug_() { AssertThat(@"
(defn s
  ([] '')
  ([x] (clojure.lang.Util/Str x))
  ([x y] (clojure.lang.Util/Str x y))
  ([x y & more] (reduce str (str x y) more))
)(s)", ""); }
	}
}