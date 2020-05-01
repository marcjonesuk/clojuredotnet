using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Seq_Tests
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
		[DataRow("(first nil)", null)]
		[DataRow("(first [])", null)]
		[DataRow("(first [1])", 1)]
		[DataRow("(first [1 2])", 1)]
		[DataRow("(first [[1 2] [3 4]])", "[1 2]")]
		[DataRow("(first `(1 2))", 1)]
		[DataRow("(first 'abc')", "a")]
		[DataRow("(first {:key 5})", "[:key 5]")]
		public void First_Tests(string code, object expected) =>  Run_And_Compare(code, expected);
		
		[DataTestMethod]
		[DataRow("(next nil)", null)]
		[DataRow("(next [])", null)]
		[DataRow("(next [1])", null)]
		[DataRow("(next [1 2])", "(2)")]
		[DataRow("(next [1 2 3])", "(2 3)")]
		[DataRow("(first (next [1 2 3]))", 2)]
		[DataRow("(apply str (next 'abc'))", "bc")]
		public void Next_Tests(string code, object expected) =>  Run_And_Compare(code, expected);
		
		[DataTestMethod]
		[DataRow("(drop 1 nil)", "()")]
		[DataRow("(drop 1 [])", "()")]
		[DataRow("(drop 1 [1])", "()")]
		[DataRow("(first (drop -1 [1]))", 1)]
		[DataRow("(first (drop 1 [1 2]))", 2)]
		[DataRow("(drop 5 [1 2 3 4])", "()")]
		[DataRow("(take 3 (drop 5 (range 1 11)))", "(6 7 8)")]
		public void Drop_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(last nil)", null)]
		[DataRow("(last [])", null)]
		[DataRow("(last [1 2 3 4 5])", 5)]
		[DataRow("(last {:key 5})", "[:key 5]")]
		public void Last_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(distinct [1 2 3])", "(1 2 3)")]
		[DataRow("(distinct [true false true])", "(true false)")]
		[DataRow("(distinct [1 2 3 1 2 4])", "(1 2 3 4)")]
		[DataRow("(distinct [:key1 :key2 :key1])", "(:key1 :key2)")]
		public void Distinct_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(take 5 (range))", "(0 1 2 3 4)")]
		[DataRow("(range 5)", "(0 1 2 3 4)")]
		[DataRow("(range -2 2)", "(-2 -1 0 1)")]
		[DataRow("(range -2 2 1)", "(-2 -1 0 1)")]
		[DataRow("(range -100 100 10)", "(-100 -90 -80 -70 -60 -50 -40 -30 -20 -10 0 10 20 30 40 50 60 70 80 90)")]
		[DataRow("(range 0 4 2)", "(0 2)")]
		[DataRow("(range 0 5 2)", "(0 2 4)")]
		[DataRow("(range 0 6 2)", "(0 2 4)")]
		[DataRow("(range 0 7 2)", "(0 2 4 6)")]
		public void Range_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(odd? -1)", true)]
		[DataRow("(odd? 1)", true)]
		[DataRow("(even? -2)", true)]
		[DataRow("(even? 0)", true)]
		[DataRow("(even? 2)", true)]
		public void Odd_Even_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(defn tally [n] (apply str (concat (repeat (quot n 5) '卌') (repeat (mod n 5) '|')))) (map tally (range 1 11))", "(| || ||| |||| 卌 卌| 卌|| 卌||| 卌|||| 卌卌)")]
		public void Integration_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(concat [1 2])", "(1 2)")]
		[DataRow("(concat [1 2] [3 4])", "(1 2 3 4)")]
		[DataRow("(concat [:a :b] nil [1 [2 3] 4])", "(:a :b 1 [2 3] 4)")]
		[DataRow("(concat [1] [2] `(3 4) [5 6 7] #{9 10 8})", "(1 2 3 4 5 6 7 8 9 10)")]
		public void Concat_Tests(string code, object expected) =>  Run_And_Compare(code, expected);
	}
}