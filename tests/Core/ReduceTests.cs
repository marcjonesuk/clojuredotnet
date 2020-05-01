using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class ReduceTests : BaseTest
	{
		[TestMethod] public void Reduce_1() { AssertThat("(reduce + [1 2 3 4 5])", 15.0); }
		[TestMethod] public void Reduce_2() { AssertThat("(reduce + [])", 0.0); }
		[TestMethod] public void Reduce_3() { AssertThat("(reduce + [1])", 1.0); }
		[TestMethod] public void Reduce_4() { AssertThat("(reduce + [1 2])", 3.0); }
		[TestMethod] public void Reduce_5() { AssertThat("(reduce + 1 [])", 1.0); }
		[TestMethod] public void Reduce_6() { AssertThat("(reduce + 1 [2 3])", 6.0); }
		[TestMethod] public void Reduce_7() { AssertThat("(reduce (fn [a b] (+ a b 2)) 1 [2 3])", 10.0); }
		[TestMethod] public void Reduce_8() { AssertThat("(reduce + (list 1 2 3 4 5))", 15.0); }
		[TestMethod] public void Reduce_9() { AssertThat("(reduce + (list))", 0.0); }
		[TestMethod] public void Reduce_10() { AssertThat("(reduce str ['a' 'b' 'c'])", "abc"); }
	}
}