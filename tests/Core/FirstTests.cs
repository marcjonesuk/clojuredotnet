// using System.Collections.Generic;
// using System.Linq;
// using marcclojure;
// using Microsoft.VisualStudio.TestTools.UnitTesting;

// namespace tests
// {
// 	[TestClass]
// 	public class FirstTests
// 	{
// 		[TestMethod] public void Test_1() { RunTest("(count (list))", 0); }
	
// 		public void RunTest(string code, object expected)
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(code);
// 			var result = compiler.Compile(tokens).Invoke();
// 			Assert.AreEqual(expected, result);
// 		}
// 	}
// }