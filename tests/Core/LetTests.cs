using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class LetTests : BaseTest
	{
		[TestMethod] public void Let_1() { AssertThat("(let [x 1] x)", 1.0); }
		[TestMethod] public void Let_2() { AssertThat("(let [x 1 y 2] (+ x y))", 3.0); }
		[TestMethod] public void Let_3() { AssertThrows("(let [x 1])x", typeof(Exception)); }
		[TestMethod] public void Let_4() { AssertThat("(let [x 1])", null); }
		[TestMethod] public void Let_5() { AssertThat("(let [x 1] (let [x 2] x))", 2.0); }
		[TestMethod] public void Let_6() { AssertThat("(let [x 1 x 2] x)", 2.0); }
		[TestMethod] public void Let_7() { AssertThat("(let [x 2] (first (list x 3 4)))", 2.0); }
		[TestMethod] public void Let_8() { AssertThat("(let [y 1] (def z y))z", 1.0); }
		[TestMethod] public void Let_9() { AssertThat("(let [y 1] (def z (+ y 1)) y)", 1.0); }
		[TestMethod] public void Let_10() { AssertThat("(let [a 1 b 2] (+ a b))", 3.0); }
		[Ignore] [TestMethod] public void Let_11() { AssertThat("(let [[g h] [1 2]] g)", 1.0); }
		[Ignore] [TestMethod] public void Let_12() { AssertThat("(let [[g h] [1]] h)", null); }
	}
}
