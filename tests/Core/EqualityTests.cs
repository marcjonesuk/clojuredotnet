using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class EqualsTests : BaseTest
	{
		[TestMethod] public void Equality_1() { AssertThat("(= 2 (+ 1 1))", true); }
		[TestMethod] public void Equality_1_1() { AssertThat("(not= 2 (+ 2 1))", true); }
		[TestMethod] public void Equality_1_2() { AssertThat("(nil? 5)", false); }
		[TestMethod] public void Equality_1_3() { AssertThat("(nil? nil)", true); }
		[TestMethod] public void Equality_1_4() { AssertThat("(false? false)", true); }
		[TestMethod] public void Equality_1_5() { AssertThat("(false? true)", false); }
		[TestMethod] public void Equality_2() { AssertThat("(= (str 'fo' 'od') 'food')", true); }
		[Ignore] [TestMethod] public void Equality_3() { AssertThat("(= 2 2.0)", false); }
		[TestMethod] public void Equality_4() { AssertThat("(= [0 1 2] [0 1 2])", true); }
		[Ignore] [TestMethod] public void Equality_4_1() { AssertThat("(= [0 1 2] (range 3))", true); }
		[Ignore] [TestMethod] public void Equality_5() { AssertThat("(= [0 1 2] '(0 1 2))", true); }
		[TestMethod] public void Equality_6() { AssertThat("(= [0 1 2] [0 2 1])", false); }
		[TestMethod] public void Equality_7() { AssertThat("(= [0 1] [0 1 2])", false); }
		[Ignore] [TestMethod] public void Equality_8() { AssertThat("(= '(0 1 2) '(0 1 2.0))", false); }
		[TestMethod] public void Equality_9() { AssertThat("(= {:a 1} {:a 1})", true); }
		[TestMethod] public void Equality_10() { AssertThat("(= {:a 1} {:b 1})", false); }
		[TestMethod] public void Equality_11() { AssertThat("(= {:a 1} {:a 2})", false); }
		[TestMethod] public void Equality_12() { AssertThat("(first (next (next [1 2 3])))", 3.0); }
	}
}