using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class MathTests : BaseTest
	{
		[TestMethod] public void Math_1() { AssertThat("(+ 1 2)", 3.0); }
		[TestMethod] public void Math_2() { AssertThat("(/ 2)", 0.5); }
	}
}