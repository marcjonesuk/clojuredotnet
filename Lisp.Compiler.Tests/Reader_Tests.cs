using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Reader_Tests
	{
		[DataTestMethod]
		[DataRow("0", 0)]
		[DataRow("1", 1)]
		[DataRow("-1", -1)]
		[DataRow("1.1", 1.1)]
		[DataRow(".1", 0.1)]
		[DataRow("2147483647", int.MaxValue)]
		[DataRow("-2147483648", int.MinValue)]
		[DataRow("99999999999.9", 99999999999.9)]
		[DataRow("-99999999999.9", -99999999999.9)]
		[DataRow("3.14", 3.14)]
		[DataRow("-3.14", -3.14)]
		public void Number_Tests(string s, object expected)
		{
			var read = new Reader().Read(s).First();
			Assert.AreEqual(expected, read);
		}

		[DataTestMethod]
		[DataRow("true", true)]
		[DataRow("false", false)]
		[DataRow("nil", null)]
		public void Special_Value_Tests(string s, object expected)
		{
			var read = new Reader().Read(s).First();
			Assert.AreEqual(expected, read);
		}

		[DataTestMethod]
		[DataRow("()")]
		// [Ignore][DataRow("( )")]
		[DataRow("(1)", 1)]
		[DataRow("(1 2)", 1, 2)]
		[DataRow("(1 2 3)", 1, 2, 3)]
		[DataRow("(1 '2' 3)", 1, "2", 3)]
		[DataRow("(true)", true)]
		[DataRow("(nil)", null)]
		[DataRow("(nil nil)", null, null)]
		// [DataRow("(reduce RT/add '(1 2 3 4)", 10)]
		public void List_Tests(string s, params object[] items)
		{
			var read = (IList<object>)(new Reader().Read(s).First());
			Assert.AreEqual(items.Length, read.Count);
			for (var i = 0; i < items.Length; i++)
				Assert.AreEqual(items[i], read[i]);

			s = s.Replace("(", "[").Replace(")", "]");
			read = (IList<object>)(new Reader().Read(s).First());
			Assert.AreEqual(items.Length, read.Count);
			for (var i = 0; i < items.Length; i++)
				Assert.AreEqual(items[i], read[i]);
		}

		[DataTestMethod]
		[DataRow("()")]
		[DataRow("(())")]
		[DataRow("((()))")]
		[DataRow("(() ())")]
		[Ignore]
		[DataRow("(()())")] // Failing
		[DataRow("(((1)))")]
		[DataRow("(1 (2 3))")]
		[DataRow("((1 2) (3 4))")]
		[DataRow("((1 2) ('three' 4))")]
		[DataRow("(nil)")]
		[DataRow("(nil ())")]
		public void List_Tests_2(string s)
		{
			var read = (IList<object>)(new Reader().Read(s).First());
			Assert.AreEqual(s, read.Stringify());
			Assert.IsInstanceOfType(read, typeof(ImmutableList<object>));

			s = s.Replace("(", "[").Replace(")", "]");
			read = (IList<object>)(new Reader().Read(s).First());
			Assert.AreEqual(s, read.Stringify());
			Assert.IsInstanceOfType(read, typeof(ImmutableArray<object>));
		}

		[DataTestMethod]
		[DataRow("#{1}")]
		[DataRow("#{1 1}", "#{1}")]
		[DataRow("#{a}")]
		[DataRow("#{a a}", "#{a}")]
		[DataRow("#{nil}")]
		[DataRow("#{nil nil}", "#{nil}")]
		[DataRow("#{true}")]
		[DataRow("#{}")]
		[DataRow("#{() ()}", "#{()}")]
		[DataRow("#{#{1}}")]
		public void Set_Tests(string s, string expected = null)
		{
			if (expected == null) expected = s;
			var read = (IEnumerable<object>)(new Reader().Read(s).First());
			Assert.AreEqual(expected, read.Stringify());
			Assert.IsInstanceOfType(read, typeof(ImmutableHashSet<object>));
		}

		[DataTestMethod]
		[DataRow("nil")]
		[DataRow("'nil'")]
		[DataRow("true")]
		[DataRow("'true'")]
		[DataRow("false")]
		[DataRow("'false'")]
		[DataRow("1")]
		[DataRow("-1")]
		[DataRow("1.1")]
		[DataRow("-1.1")]
		[DataRow("0")]
		[DataRow("[]")]
		[DataRow("[1 2 3]")]
		[DataRow("(1 2 3)")]
		[DataRow("'hello, world'")]
		[DataRow("`x")]
		[DataRow("~x")]
		[DataRow("`()")]
		[DataRow("`(~x)")]
		[DataRow("([])")]
		public void Stringify_Tests(string s)
		{
			var read = new Reader().Read(s).First();
			Assert.AreEqual(s, read.Stringify(true));
		}

		[DataTestMethod]
		[DataRow("a")]
		[DataRow("1", false)]
		[DataRow("nil", false)]
		[DataRow("true", false)]
		[DataRow("false", false)]
		[DataRow("e")]
		[DataRow("_helloworld")]
		public void Symbol_Tests(string s, bool expectedSymbol = true)
		{
			var read = (new Reader().Read(s).First());
			if (expectedSymbol)
			{
				Assert.IsTrue(typeof(Symbol) == read.GetType());
				Assert.AreEqual(s, ((Symbol)read).Name);
			}
			else
			{
				if (read == null) return;
				Assert.IsFalse(typeof(Symbol) == read.GetType());
			}
		}

		[DataTestMethod]
		// [DataRow("'hello, world'")]
		// [DataRow("'hello, world ;this is not a comment'")]
		[DataRow("(deftest 'Adding two numbers together' (let [result (add 1 2) (= 3 result)))")]
		public void String_Tests(string s)
		{
			var result = (new Reader().Read(s));
			var read = (string)result.First();
			Assert.AreEqual(s[1..^1], read);
		}

		[DataTestMethod]
		[DataRow("(#(+ 1 %) 5)", "((fn [%] (+ 1 %)) 5)")]
		public void Anonymous_Function_Tests(string code, string result)
		{
			var read = new Reader().Read(code).First();
			Assert.AreEqual(result, read.Stringify());
		}

		[DataTestMethod]
		[DataRow("s", 0, "s", 1, 1)]
		[DataRow("symbol", 0, "symbol", 1, 1)]
		[DataRow("\nsymbol", 0, "symbol", 2, 1)]
		[DataRow("x\n\n\ns", 4, "s", 4, 1)]
		[DataRow(";c\ns", 0, "s", 2, 1)]
		[DataRow("\n(s)\n", 1, "s", 2, 2)]
		[DataRow("\n\n(s)\n\n", 1, "s", 3, 2)]
		[DataRow("\n\nsymbol", 0, "symbol", 3, 1)]
		public void Tokeniser_Location_Tests(string code, int element, string value, int line, int col)
		{
			var tokeniser = new Tokeniser();
			var enumerator = tokeniser.Tokenise(code);
			var token = enumerator.Tokens[element];
			Assert.AreEqual(value, token.Value, "value");
			Assert.AreEqual(line, token.Line, "line");
			Assert.AreEqual(col, token.Column, "column");
		}

		[DataTestMethod]
		[DataRow(" (+ 1 3)", "(+ 1 3)")]
		[DataRow("(+ 1 3) ", "(+ 1 3)")]
		[DataRow("(+ 1 3 )", "(+ 1 3)")]
		public void Whitespace_Tests(string code, string result)
		{
			var read = new Reader().Read(code).First();
			Assert.AreEqual(result, read.Stringify());
		}


		[DataTestMethod]
		[DataRow("System.Linq.Enumerable/TakeWhile<System.Object>", "(+ 1 3)")]
		// [DataRow("(interop 'System.Linq.Enumerable/TakeWhile<System.Object>(IEnumerable<object>,Func<object,bool>)')", "(+ 1 3)")]
		public void Tokeniser_Tests(string code, string result)
		{
			var read = new Tokeniser().Tokenise(code);
			Assert.AreEqual(result, read.Stringify());
		}

		[TestMethod]
		public void Integration_Tests()
		{
			var s = "(defn add-new-paste 'Insert a new paste in the database, then return its UUID.' [store content] (let [uuid (.toString (java.util.UUID/randomUUID))] (swap! (:data store) assoc (keyword uuid) {:content content}) uuid))";
			var read = (new Reader().Read(s).First());
			Assert.AreEqual(s, read.Stringify());

			// s = "(defn render-form 'Render a simple HTML form page.' [] (html5 [:head [:meta {:charset 'UTF-8'}]] [:body (form-to [:post '/'] (text-area {:cols 80 :rows 10} 'content') [:div] (submit-button 'Paste!'))]))";
			// read = (new Reader().Read(s).First());
			// Assert.AreEqual(s, read.Stringify());
		}
	}
}
