using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class ConjTests : BaseTest
	{
		[TestMethod] public void Conj_1() { AssertThat("(str (conj [1 2 3] 4))", "[1 2 3 4]"); }
		[TestMethod] public void Conj_1_1() { AssertThat("(str (conj [1] 2 3 4))", "[1 2 3 4]"); }
		[TestMethod] public void Conj_2() { AssertThat("(def a [1 2 3])(conj a 4)(str a)", "[1 2 3]"); }
		[TestMethod] public void Conj_3() { AssertThat("(str (conj [1 2] 3 4))", "[1 2 3 4]"); }
		[TestMethod] public void Conj_4() { AssertThat("(str (conj [[1 2] [3 4]] [5 6]))", "[[1 2] [3 4] [5 6]]"); }
		[Ignore] [TestMethod] public void Conj_5() { AssertThat("(str (conj {1 2, 3 4} [5 6]))", "{5 6, 1 2, 3 4}"); }
		[TestMethod] public void Conj_6() { AssertThat("(str (conj {:firstname 'John' :lastname 'Doe'} {:age 25 :nationality 'Chinese'}))", "{:firstname 'John', :lastname 'Doe', :age 25, :nationality 'Chinese'}"); }
		[TestMethod] public void Conj_7() { AssertThat("(str (conj {:a 1} {:b 2}))", "{:a 1, :b 2}"); }
		[TestMethod] public void Conj_8() { AssertThat("(str (conj {:a 1} {:a 2}))", "{:a 2}"); }
		// [TestMethod] public void Conj_6() { AssertThat("", null); }
		// [TestMethod] public void Conj_7() { AssertThat("", null); }
		// [TestMethod] public void Conj_8() { AssertThat("", null); }
		// [TestMethod] public void Conj_1() { AssertThat("", null); }
	}
}