using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class StrTests : BaseTest
	{
		[TestMethod]
		public void Test_1() { AssertThat("'some string'", "some string"); }
		[TestMethod]
		public void Test_2() { AssertThat("(str)", ""); }
		[TestMethod]
		public void Test_3() { AssertThat("(str nil)", ""); }
		[TestMethod]
		public void Test_4() { AssertThat("(str 1)", "1"); }
		[TestMethod]
		[Ignore] 
		public void Test_5() { AssertThat("(str 1 'symbol :keyword)", "1symbol:keyword"); }
		[TestMethod]
		public void Test_6() { AssertThat("(apply str [1 2 3])", "123"); }
		[TestMethod]
		public void Test_7() { AssertThat("(str [1 2 3])", "[1 2 3]"); }
		[TestMethod]
		public void Test_8() { AssertThat("(str [])", "[]"); }
		[TestMethod]
		public void Test_9() { AssertThat("(str 'hello' ' ' 'world')", "hello world"); }
		[TestMethod]
		public void Test_10() { AssertThat("(str 'L' 5 'a')", "L5a"); }
		[TestMethod]
		public void Test_11() { AssertThat("(str (list 1 2 3))", "(1 2 3)"); }
		[TestMethod] public void Test_12() { AssertThat("(str {:a 1})", "{ :a 1 }"); }
		[TestMethod] public void Str_12() { AssertThat("(str [[1 2]])", "[[1 2]]"); }
	}
}