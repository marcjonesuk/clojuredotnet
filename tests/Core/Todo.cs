using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class Todo : BaseTest
	{
		[Ignore] [TestMethod] public void Todo_1() { AssertThat("(reduce + (take 5 (iterate inc 5)))", 35.0); }
	}
}