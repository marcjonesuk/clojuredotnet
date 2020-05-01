using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class LoopTests : BaseTest
	{
		[TestMethod] public void Loop_1() { AssertThat(@"
(def factorial
  (fn [n]
    (loop [cnt n
           acc 1]
       (if (zero? cnt)
            acc
          (recur (dec cnt) (* acc cnt))))))
(factorial 5)", 120.0); }
	}
}