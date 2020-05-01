using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Math_Tests
	{
		public void Run_And_Compare(string code, object expected)
		{
			if (!(expected is Type t)) {
				var result = new Compiler().Compile(code).Invoke();
				if (expected is string)
					result = result.Stringify();
				Assert.AreEqual(expected, result, code);	
				return;
			}

			try
			{
				var result = new Compiler().Compile(code).Invoke();
				if (expected is string)
					result = result.Stringify();
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
		[DataRow("(* 2 3)", 6)]
		[DataRow("(* 2 3 4)", 24)]
		[DataRow("(* 2 3 'a')", typeof(RuntimeBinderException))]
		public void Math_Tests_1(string code, object expected) =>  Run_And_Compare(code, expected);
	}
}