using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class GetTests : BaseTest
	{
		[TestMethod] public void Get_1() { AssertThat("(get [1 2 3] 1)", 2.0); }
		[TestMethod] public void Get_2() { AssertThat("(get [1 2 3] 5)", null); }
		[TestMethod] public void Get_2_1() { AssertThat("(get [1 2 3] -5)", null); }
		[TestMethod] public void Get_3() { AssertThat("(get [1 2 3] 5 100)", 100.0); }
		[TestMethod] public void Get_4() { AssertThat("(get {:a 1 :b 2} :b)", 2.0); }
		[TestMethod] public void Get_5() { AssertThat("(get {:a 1 :b 2} :z 'missing')", "missing"); }
		[TestMethod] public void Get_6() { AssertThat("(get 'abc' 1)", "b"); }
		[TestMethod] public void Get_7() { AssertThat("(get [0 1 2] 'a')", null); }
	}
}