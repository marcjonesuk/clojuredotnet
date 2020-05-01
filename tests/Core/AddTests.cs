using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class AddTests : BaseTest
	{
		[TestMethod] public void Add_1() { AssertThat("(+)", 0.0); }
		[TestMethod] public void Add_2() { AssertThat("(+ 1)", 1.0); }
		[TestMethod] public void Add_3() { AssertThat("(+ -10)", -10.0); }
		[TestMethod] public void Add_4() { AssertThat("(+ 1 2)", 3.0); }
		[TestMethod] public void Add_5() { AssertThat("(+ 1 2 3)", 6.0); }
		[TestMethod] public void Add_6() { AssertThat("(+ 's' 'a')", "sa"); }
	}
}