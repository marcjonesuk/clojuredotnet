using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Destructuring_Tests
	{
		public void Run_And_Compare(string code, object expected)
		{
			if (!(expected is Type t)) {
				var result = new Compiler().Compile(code).Invoke();
				if (expected is string && !(result is string))
					result = result.Stringify(false);
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
		// [DataRow("(let [[a] [1]] a)", 1)]
		// [DataRow("(let [[a b] [1 2]] (+ a b))", 3)]
		// [DataRow("(let [[a b c] [1 2]] c)", null)]
		// [DataRow("(let [[a b c] [1 2 3 4]] c)", 3)]
		// [DataRow("(let [[a b c] [1 2 3 4]] c)", 3)]
		// [DataRow("(let [[a] [[1 2]]] (str a))", "[1 2]")]
		// [DataRow("(let [[a b] [[1 2]]] a)", "[1 2]")]
		// [DataRow("(let [[[a b] [c d] e] [[1 2] [3 4] 5]] (str a b c d e))", "12345")]

		[DataRow("(fn [] ((let [x 1] 1)))", "[1 2]")]

		// [DataRow("(def my-nested-vector [:a :b :c :d [:x :y :z]]) (let [[a _ _ d [x y z]] my-nested-vector] (str a d x y z))", ":a:d:x:y:z")]
		public void Destructuring_Tests_1(string code, object expected) => Run_And_Compare(code, expected);
	}
}