using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class SpecTests : BaseTest
	{
		[TestMethod] public void Spec_1() { AssertThat("(s/def 5)", 15.0); }
	}
}