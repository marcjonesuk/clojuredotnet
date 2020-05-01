using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Let_Tests
	{
		public void Run_And_Compare(string code, object expected)
		{
			if (!(expected is Type t)) {
				var result = new Compiler().Compile(code).Invoke();
				Assert.AreEqual(expected, result, code);	
				return;
			}

			try
			{
				var result = new Compiler().Compile(code).Invoke();
				Assert.AreEqual(expected, result, code);
			}
			catch (Exception ex)
			{
				Assert.AreEqual(typeof(RuntimeException), ex.GetType(), code);
				Assert.IsNotNull(ex.InnerException, code);
				var inner = ex.InnerException;
				Assert.AreEqual(t, inner.GetType(), code);
			}
		}
		
		[DataTestMethod]
		[DataRow("(let [x 1] x)", 1)]
		[DataRow("(let [x1 1]) x1", typeof(System.Exception))]
		[DataRow("(let [y 1] (def z y)) z", 1)]
		[DataRow("(let [a 1 b 2] (+ a b))", 3)]
		[DataRow("(let [[g h] [1 2 3]] (* g h))", 2)]
		[DataRow("(let [[g h i] [1 2]] i)", null)]
		[DataRow("(let [a 1 b 2])", null)]
		[DataRow("(let [a 1 b a] b)", 1)]
		public void Let_Tests_1(string code, object expected) => Run_And_Compare(code, expected);
	}
}