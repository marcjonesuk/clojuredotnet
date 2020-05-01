using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class IntoTests : BaseTest
	{
		[Ignore] [TestMethod] public void Into_2() { AssertThat("(str (into {:x 4} [{:a 1} {:b 2} {:c 3}])", 3.0); }
		[TestMethod] public void Into_3() { AssertThat("(str (into [0] [1 2]))", "[0 1 2]"); }
		[TestMethod] public void Into_4() { AssertThat("(defn my-into [target additions] (apply conj target additions))(str (my-into [0] [1 2 3]))", "[0 1 2 3]"); }
	}
}