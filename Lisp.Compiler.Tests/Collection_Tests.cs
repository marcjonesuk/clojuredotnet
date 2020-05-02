using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Collection_Tests
	{
		public async Task Run_And_Compare(string code, object expected)
		{
			if (!(expected is Type t)) {
				var result = await new Compiler().Compile(code).Invoke();
				if (expected is string)
					result = result.Stringify();
				Assert.AreEqual(expected, result, code);	
				return;
			}

			try
			{
				var result = await new Compiler().Compile(code).Invoke();
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
		[DataRow("(get ['abc' false 99] 0)", "abc")]
		[DataRow("(get ['abc' false 99] 1)", false)]
		[DataRow("(get ['abc' false 99] 14)", null)]

		public void _Tests_1(string code, object expected) =>  Run_And_Compare(code, expected);
	}
}