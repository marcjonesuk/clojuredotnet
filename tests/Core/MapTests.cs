using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class MapTests : BaseTest
	{
		[TestMethod] public void Map_1() { AssertThat("(reduce + (map inc [1 2 3 4 5]))", 20.0); }
		[Ignore] [TestMethod] public void Map_2() { AssertThat("(reduce + (map * [1 2 3] [4 5 6]))", 32.0); }
		// [TestMethod] public void Map_3() { Assert("", 0.0); }
		// [TestMethod] public void Map_4() { Assert("", 0.0); }
		// [TestMethod] public void Map_5() { Assert("", 0.0); }
		// [TestMethod] public void Map_6() { Assert("", 0.0); }
		// [TestMethod] public void Map_7() { Assert("", 0.0); }
		// [TestMethod] public void Map_8() { Assert("", 0.0); }
	}
} 