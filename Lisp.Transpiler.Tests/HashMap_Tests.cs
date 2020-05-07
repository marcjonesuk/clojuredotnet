using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Transpiler.Tests
{
    [TestClass]
    public class HashMap_Tests : TestBase
    {
		[DataTestMethod]
		[DataRow("{ 'key' 1}", 3)]
		public void Add_Tests(string code, object expected) =>  Run_And_Compare(code, expected);
	}
}