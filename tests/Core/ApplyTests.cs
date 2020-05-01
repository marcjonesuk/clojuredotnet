using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class ApplyTests : BaseTest
	{
		[TestMethod]
		public void Apply_1() { AssertThat(@"
		(def *strings* ['str1' 'str2' 'str3'])
		(apply str *strings*)", "str1str2str3"); }

		[TestMethod] public void Apply_2() { AssertThat("(apply str [])", ""); }
		[TestMethod] public void Apply_3() { AssertThat("(apply str nil)", ""); }
		[TestMethod] public void Apply_4() { AssertThat("(apply + [1 2 3])", 6.0); }
		[TestMethod] public void Apply_5() { AssertThat("(apply str 'a' 'b' 'c' ['d' 'e'])", "abcde"); }
		[TestMethod] public void Apply_6() { AssertThat("(str (apply conj [0] 1 2 [3 4]))", "[0 1 2 3 4]"); }
	}
}