using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class VectorTests : BaseTest
	{
		[TestMethod] public void Count_1() { AssertThat("(count (list))", 0); }
		[TestMethod] public void Count_2() { AssertThat("(count (list 1))", 1); }
		[TestMethod] public void Count_3() { AssertThat("(count (list 1 2))", 2); }
		[TestMethod] public void Count_4() { AssertThat("(count (list 1 '2'))", 2); }
		[TestMethod] public void First_1() { AssertThat("(first (list 1 '2'))", 1.0); }
		[TestMethod] public void First_2() { AssertThat("(first nil)", null); }
		[TestMethod] public void First_3() { AssertThat("(first (list))", null); }

		[Ignore] [TestMethod] public void Nth_1() { AssertThat("(nth (list 1 '2') 0)", 1.0); }
		[Ignore] [TestMethod] public void Nth_2() { AssertThat("(nth (list 1 '2') 1)", "2"); }
		[Ignore] [TestMethod] public void Nth_3() { AssertThat("(nth (list 1 '2') 2)", null); }
		[Ignore] [TestMethod] public void Nth_4() { AssertThat("(nth [] 0 'nothing found')", "nothing found"); }
		[Ignore] [TestMethod] public void Nth_5() { AssertThat("(nth ['last'] -1 'this is not perl')", "nothing found"); }
		[Ignore] [TestMethod] public void Nth_6() { AssertThat("(nth nil 0)", null); }
		[Ignore] [TestMethod] public void Nth_7() { AssertThat("(nth nil 0)", null); }   // This should throw out of bounds

		[TestMethod] public void Rest_1() { AssertThat("(count (rest nil))", 0); }   
		[TestMethod] public void Rest_2() { AssertThat("(count (rest (list)))", 0); }   
		[TestMethod] public void Rest_3() { AssertThat("(first (rest (list 1 2 3)))", 2.0); }
		[TestMethod] public void Rest_4() { AssertThat("(first (rest (rest (list 1 2 3))))", 3.0); }   
		[TestMethod] public void Rest_5() { AssertThat("(count (rest (rest (rest (list 1 2 3)))))", 0); }   
		[TestMethod] public void Rest_6() { AssertThat("(str (rest [1 2 3 4 5]))", "(2 3 4 5)"); }   

		[TestMethod] public void Vector_1() { AssertThat("([1 2 3] 1)", 2.0); }
		[TestMethod] public void Vector_2() { AssertThat("([1 2 3] 1.4)", 2.0); }
		[TestMethod] public void Vector_3() { AssertThat("([1 2 3] 4 'abc')", "abc"); }
		[TestMethod] public void Vector_4() { AssertThat("([1 2 3] -1 'abc')", "abc"); }
		[TestMethod] public void Vector_5() { AssertThat("([1 2 3] 0 'abc')", 1.0); }
	}
}