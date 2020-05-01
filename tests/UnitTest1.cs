// using System.Collections.Generic;
// using System.Linq;
// using marcclojure;
// using Microsoft.VisualStudio.TestTools.UnitTesting;

// namespace tests
// {
// 	[TestClass]
// 	public class UnitTest1
// 	{
// 		[TestMethod]
// 		public void TestMethod1()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var result = tokeniser.Tokenise("()");
// 			Assert.AreEqual(2, result.Count);
// 		}

// 		// [TestMethod]
// 		// public void Tokeniser_2()
// 		// {
// 		// 	var tokeniser = new Tokeniser();
// 		// 	var result = tokeniser.Tokenise("'hello world'");
// 		// 	Assert.AreEqual("hello world", result.Current);
// 		// 	Assert.AreEqual(3, result.Count);
// 		// }

// 		[TestMethod]
// 		public void TestMethod2()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var result = tokeniser.Tokenise("(func)");
// 			Assert.AreEqual(3, result.Count);
// 		}

// 		[TestMethod]
// 		public void TestMethod3()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var result = tokeniser.Tokenise("(3)");
// 			Assert.AreEqual(3, result.Count);
// 		}

// 		[TestMethod]
// 		public void TestWithSingleConstant()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise("3");
// 			Assert.AreEqual(1, tokens.Count);
// 		}

// 		[TestMethod]
// 		[Ignore]
// 		public void FunctionNoCall()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise("(3)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 		}

// 		[TestMethod]
// 		public void FunctionCall2()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise("(inc 4)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(5.0, result);
// 		}

// 		[TestMethod]
// 		public void FunctionCall3()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise("(inc (inc 4))");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(6.0, result);
// 		}

// 		[TestMethod]
// 		public void FunctionCall4()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise("(inc (inc 4))");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(6.0, result);
// 		}

// 		[TestMethod]
// 		public void Multiline()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(@"
// 			3
// 			(inc 6)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(7.0, result);
// 		}

// 		[TestMethod]
// 		public void Def1()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(@"
// 			(def a 6)
// 			(inc a)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(7.0, result);
// 		}

// 		[TestMethod]
// 		public void Defn_1()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(@"
// 			(defn my_fn [a] (inc a))
// 			(my_fn 6)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(7.0, result);
// 		}

// 		[TestMethod]
// 		public void Defn_2()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(@"
// 			(defn my_fn [a b] (+ a b))
// 			(my_fn 6 12)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(18.0, result);
// 		}

// 		[TestMethod]
// 		public void Anon_1()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(@"
// 			(def an #(inc %))
// 			(an 6)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(7.0, result);
// 		}

// 		[TestMethod]
// 		public void Anon_2()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(@"
// 			(def add2 #(+ % 2))
// 			(add2 6)");
// 			var result = compiler.Compile(tokens).Invoke(new State(), null);
// 			Assert.AreEqual(8.0, result);
// 		}

// 		[TestMethod]
// 		public void Map_1()
// 		{
// 			var tokeniser = new Tokeniser();
// 			var compiler = new Compiler();
// 			var tokens = tokeniser.Tokenise(@"
// 			(map inc [1 2 3 4 5])");
// 			dynamic result = ((IEnumerable<object>)compiler.Compile(tokens).Invoke(new State(), null)).ToList().Count;
// 			Assert.AreEqual(5, result);
// 		}

// 		// [TestMethod]
// 		// public void Map_2()
// 		// {
// 		// 	var tokeniser = new Tokeniser();
// 		// 	var compiler = new Compiler();
// 		// 	var tokens = tokeniser.Tokenise(@"
// 		// 	(map #( ) [inc dec])");
// 		// 	dynamic result = ((IEnumerable<object>)compiler.Compile(tokens).Invoke()).ToList().Count;
// 		// 	Assert.AreEqual(5, result);
// 		// }
// 	}
// }