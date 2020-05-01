using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class InteropTests : BaseTest
	{
		[Ignore] [TestMethod] public void Interop_1() { AssertThat("(def my-vector [1 2 3])(.ToString my-vector)", "[1 2 3]"); }
		[Ignore] [TestMethod] public void Interop_2() { AssertThat("(def my-vector [1 2 3])(.-Count my-vector)", 3); }
	}
}