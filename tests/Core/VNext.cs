using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class VNextTests : BaseTest
	{
		// 			var code = @"
		// (defn *
		//   ([] 1)
		//   ([x] x)
		//   ([x y] (clojure.lang.Numbers/Multiply x y))
		//   ([x y & more]
		//      (reduce * (* x y) more)))
		// ";
		// var code = @"
		// (defn conj
		//   ([coll item] (clojure.lang.Util/Conj coll item))
		//   ([coll item & more] (apply conj (conj coll item) more)))
		// ";

		[TestMethod]
		public void Test_1()
		{
			var code = @"
(defn + [x y] (clojure.lang.Numbers/Add x y))
(+ 5 6)
";
			var reader = new Reader();
			var read = reader.Compile(new Tokeniser().Tokenise(code));
			var result = read.Invoke();
		}

		[TestMethod]
		public void Test_2()
		{
			var reader = new Reader();
			var read = reader.Compile("(list 1 2 3)");
			var result = read.Invoke();
		}

		[TestMethod]
		public void Test_3()
		{
			var reader = new Reader2();
			var read = reader.Read("(+ 1 2)");
			var compiler = new Compiler2();
			var fn = compiler.Compile(read);
			var result = fn.Invoke();
		}
		
		[TestMethod]
		public void Test_4()
		{
			var reader = new Reader2();
			var read = reader.Read(@"
(defn =
  ([x] true)
  ([x y] (clojure.lang.Util/equiv x y)))(= true false)
			");
			var compiler = new Compiler2();
			var fn = compiler.Compile(read);
			var result = fn.Invoke();
		}
		
		[TestMethod]
		public void Macro_1()
		{
			var code = @"(defmacro infix [infixed] (list (second infixed) (first infixed) (last infixed)))";
			var result = new Reader().Compile(code).Invoke();
			code = @"(def x 5)"; 
			code += @" (* 2 (infix (x + 2)))";
			var fn = new Reader().Compile(code);
			result = fn.Invoke();
			Assert.Equals(6, result);
		}
	}
}