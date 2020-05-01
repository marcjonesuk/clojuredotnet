using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Equality_Tests
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
		[DataRow("(= true true)", true)]
		[DataRow("(= true false)", false)]
		[DataRow("(= nil false)", false)]
		[DataRow("(= nil nil)", true)]
		[DataRow("(= 'hello' 'hello')", true)]
		[DataRow("(= 'hello' 'goodbye')", false)]
		// [DataRow("(= 5 5.0)", true)]
		[DataRow("(= 0 0)", true)]
		[DataRow("(= 1 1)", true)]
		[DataRow("(= [] [])", true)]
		[DataRow("(= () [])", true)]
		[DataRow("(= [1] `(1))", true)]
		[DataRow("(= nil [])", false)]
		[DataRow("(= (range 3) [0 1 2])", true)]
		[DataRow("(= #{1 2 3} [1 2 3])", false)]
		[DataRow("(= #{1 2 3} #{3 2 1})", true)]
		[DataRow("(= #{1 2 3} #{3 2 1 4})", false)]
		[DataRow("(= #{1 2 3} #{3 2})", false)]
		[DataRow("(= {:x 1 :y 2} {:y 2 :x 1})", true)]
		[DataRow("(= {:x 1 :y 2} {:y 2 :x 1 :z 0})", false)]
		[DataRow("(= {:x 1 :y 2} {:y 2 :x 3})", false)]
		[DataRow("(= [[1] #{1 {:a 1 :b 2}} nil] [[1] #{{:a 1 :b 2} 1} nil])", true)]
		[DataRow("(= [[1] #{1 {:a 1 :b 3}} nil] [[1] #{{:a 1 :b 2} 1} nil])", false)]
		[DataRow("(= [[] #{1 2} {:a {:b 4}}] `([] #{1 2} {:a {:b 4}}))", true)]
		[DataRow("(= [[] #{1 2} {:a {:b 3}}] `([] #{1 2} {:a {:b 4}}))", false)]
		public void Equality_Tests_1(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("true", "true", true)]
		[DataRow("true", "false", false)]
		[DataRow("nil", "nil", true)]
		[DataRow("'hello'", "'hello'", true)]
		[DataRow("'hello'", "(str 'h' 'e' 'l' 'l' 'o')", true)]
		[DataRow("'hello'", "'goodbye'", false)]
		[DataRow("0", "0", true)]
		[DataRow("1", "1", true)]
		[DataRow("[]", "[]", true)]
		[DataRow("()", "[]", true)]
		[DataRow("[1]", "`(1)", true)]
		[DataRow("(range 3)", "[0 1 2]", true)]
		[DataRow("#{1 2 3}", "#{3 2 1}", true)]
		[DataRow("#{1 2 3}", "#{3 2 1 4}", false)]
		[DataRow("#{1 2 3}", "#{3 2}", false)]
		[DataRow("{:x 1 :y 2}", "{:y 2 :x 1}", true)]
		[DataRow("{:x 1 :y 2}", "{:y 2 :x 1}", true)]
		[DataRow("{:x 1 :y 2}", "{:y 2 :x 1 :z 0}", false)]
		[DataRow("{:x 1 :y 2}", "{:y 2 :x 3}", false)]
		[DataRow("[[1] #{1 {:a 1 :b 2}} nil]", "[[1] #{{:a 1 :b 2} 1} nil]", true)]
		[DataRow("[[1] #{1 {:a 1 :b 3}} nil]", "[[1] #{{:a 1 :b 2} 1} nil]", false)]
		[DataRow("[[] #{1 2} {:a {:b 4}}]", "`([] #{1 2} {:a {:b 4}})", true)]
		[DataRow("[[] #{1 2} {:a {:b 3}}]", "`([] #{1 2} {:a {:b 4}})", false)]
		public void Hash_Tests(string value1, string value2, object expected) {
			var code = $"(= (hash {value1}) (hash {value2}))";
			var result = new Compiler().Compile(code).Invoke();
			Assert.AreEqual(expected, result, code);	
		}
	}
}