using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Transpiler.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Run_And_Compare(string code, object expected)
        {
			var r = new Reader.Rdr();
			var core = @"
(defn print [v] (RT.Print (RT.Str v)))
(defn str [x] (#RT.Str x))
(defn + [x y] (RT.Add x y))
(defn * [x y] (RT.Mult x y))
(defn = [x y] (RT.Eq x y))
(defn > [x y] (RT.Gt x y))
(defn < [x y] (RT.Lt x y))
(defn inc [x] (RT.Inc x))
(defn dec [x] (RT.Dec x))
(defn take [coll count] (System.Linq.Enumerable.Take (RT.Seq coll) count))
(defn first [coll] (System.Linq.Enumerable.FirstOrDefault (RT.Seq coll)))
(defn conj [coll item] (RT.Conj coll item))
(defn get [coll index] (RT.Get coll index))
(defn reduce [fn values] (RT.Reduce fn values))
(defn apply [fn values] (RT.Apply fn values))

";
			var tree = r.Read(core + code);
			var c = Lisp.Transpiler.Compiler.Compile(tree);
			Assert.AreEqual(expected, c);
        }

		[DataTestMethod]
		[DataRow("(+ 1 2)", 3)]
		// [DataRow("(def p (fn [x] x)) (p 10)", 3)]
		public void Add_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(def a 5) a", 5)]
		[DataRow("(def a nil) a", null)]
		[DataRow("(def a true) a", true)]
		[DataRow("(def a false) a", false)]
		public void Def_Tests(string code, object expected) =>  Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("((fn [] (+ 1 2)))", 3)]
		[DataRow("((fn [y] y) 5)", 5)]
		[DataRow("((fn [y] (= y 5)) 5)", true)]
		// [DataRow("(def my_add (fn [& args] (reduce RT/Add args))) (my_add 5 6)", 11)]
		[DataRow("((fn [y] (+ 1 y)) 5)", 6)]
		[DataRow("((fn [y z] (+ y z)) 5 6)", 11)]
		// [DataRow("((fn [a b c d e f g h] (reduce + [a b c d e f g h])) 1 2 3 4 5 6 7 8)", 36)]
		[DataRow("((fn [] nil))", null)]
		// [DataRow("((fn [] (+ 1 2)) 1)", typeof(ArityException))]
		// [DataRow("((fn [y] (+ 1 y)))", typeof(ArityException))]
		// [DataRow("((fn [x & args] x) 1 2 3)", 1)]
		// [DataRow("((fn [x y & args] y) 1 2 3)", 2)]
		// [DataRow("((fn [x & args] (str args)) 1 2 3)", "[2 3]")]
		// [DataRow("((fn [x y & args] (str args)) 1 2 3)", "[3]")]
		// [DataRow("((fn [x y z & args] (str args)) 1 2 3)", "[]")]
		// [DataRow("((fn [x y z & args] (str args)) 1 2 3 nil)", "[nil]")]
		// [DataRow("((fn [& args] (str args)))", "[]")]
		// [DataRow("((fn [x & args] (str args)))", typeof(ArityException))] 
		// [DataRow("((fn [x & args] x) 1)", 1)] 
		// [DataRow("((fn 1) 1)", typeof(ArityException))] 
		// [DataRow("((fn 1 1) 1)", typeof(InvalidCastException))] 
		// [DataRow("(let [y 1] (fn [] (+ 1 y))) nil", null)] // should not throw as y is in scope
		// [DataRow("(defn outer [] (+ 1 2 x)) (let [x 1] (outer))", typeof(System.Exception))]
		public void Fn_Tests(string code, object expected) => Run_And_Compare(code, expected);
		
		[DataTestMethod]
		// [DataRow("(defn outer [] (let [x 1] (+ 1 2 x))) (outer)", 4)]
		[DataRow("((fn [] (let [x 3] (+ 1 x))))", 4)]
		public void Fn_Scope_Tests(string code, object expected) => Run_And_Compare(code, expected);	

		[DataTestMethod]
		[DataRow("(((fn [y1] (fn [] 5)) 10) 12)", 5)]
		[DataRow("(((fn [y1] (fn [] y1)) 5))", 5)]
		[DataRow("(((fn [y1] (fn [z1] (+ y1 z1))) 5) 10)", 15)]
		public void Closure_Tests(string code, object expected) => Run_And_Compare(code, expected);		

		[DataTestMethod]
		[DataRow("(#(+ 1 2))", 3)]
		[DataRow("(#(+ 1 %) 5)", 6)]
		[DataRow("(#((+ %1 %2)) 5 6)", 11)]
		[DataRow("(#((reduce + [%1 %2 %3 %4 %5 %6 %7 %8])) 1 2 3 4 5 6 7 8)", 36)]
		[DataRow("(#(nil))", null)]
		[DataRow("((fn [] (+ 1 2)) 1)", typeof(ArityException))]
		[DataRow("((fn [y] (+ 1 y)))", typeof(ArityException))]
		[DataRow("((fn [x & args] x) 1 2 3)", 1)]
		[DataRow("((fn [x y & args] y) 1 2 3)", 2)]
		[DataRow("((fn [x & args] (str args)) 1 2 3)", "[2 3]")]
		[DataRow("((fn [x y & args] (str args)) 1 2 3)", "[3]")]
		[DataRow("((fn [x y z & args] (str args)) 1 2 3)", "[]")]
		[DataRow("((fn [x y z & args] (str args)) 1 2 3 nil)", "[nil]")]
		[DataRow("((fn [& args] (str args)))", "[]")]
		[DataRow("((fn [x & args] (str args)))", typeof(ArityException))] 
		[DataRow("((fn [x & args] x) 1)", 1)] 
		[DataRow("((fn [] 1) 1)", typeof(ArityException))] 
		// [DataRow("((fn [] 1 1) 1)", typeof(InvalidCastException))] 
		public void AnonFn_Tests(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(defn x [] (+ 1 2)) (x)", 3)]
		public void Defn_Single_Arity_Tests(string code, object expected) => Run_And_Compare(code, expected);
		
		[DataTestMethod]
		// [DataRow("(defn x ([] [] (+ 1 2)))", typeof(ArityException))]
		[DataRow("(defn x ([] (+ 1 2))) (x)", 3)]
		// [DataRow("(defn x ([] (+ 1 2)) ([a] (+ 2 3))) (x)", 3)]
		// [DataRow("(defn x ([] (+ 1 2)) ([a] (+ 2 3))) (x 1)", 5)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3)) (x)", 0)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3)) (x 1)", 1)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3)) (x 1 1)", 2)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3)) (x 1 1 1)", 3)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3)) (x 1 1 1 1)", typeof(ArityException))]
		// [DataRow("(defn x ([a] 0)) (x)", typeof(ArityException))]
		// [DataRow("(defn x ([] 0) ([& args] (1))) (x)", 0)]
		// [DataRow("(defn x ([] 0) ([& args] (1))) (x 1)", 1)]
		// [DataRow("(defn x ([] 0) ([& args] (count args))) (x 1)", 1)]
		// [DataRow("(defn x ([] 0) ([& args] (count args))) (x 1 1 1)", 3)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3) ([& args] 10)) (x)", 0)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3) ([& args] 10)) (x 1)", 1)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3) ([& args] 10)) (x 1 1)", 2)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3) ([& args] 10)) (x 1 1 1)", 3)]
		// [DataRow("(defn x ([] 0) ([a] 1) ([a b] 2) ([a b c] 3) ([& args] 10)) (x 1 1 1 1)", 10)]
		// [DataRow("(defn x ([] 0) ([a b & args] '&')) (x 1)", typeof(ArityException))]
		public void Defn_Multi_Arity_Tests(string code, object expected) => Run_And_Compare(code, expected);
		
		// // // // [DataRow("(RT/str (RT/conj `(1 2) 3))", "(1 2 3)")]
		[DataTestMethod]
		[DataRow("(RT/Add 5 6)", 11)]
		[DataRow("(RT/Str 'a' 'b' 'c')", "abc")]
		[DataRow("(RT/Str)", "")]
		[DataRow("(RT/Apply RT/Str ['a' 'b' 'c'])", "abc")]
		[DataRow("(RT/Add 'abc' 'def')", "abcdef")]
		[DataRow("(def str (fn [& x] (apply RT/Str x))) (str 'a' 'b' 'c')", "abc")]
		[DataRow("(RT/Def `def `RT/Def) (def x 5) x", 5)]
		[DataRow("((fn [x y] (RT/Add x y)) 1 2)", 3)]
		[DataRow("(defn + [& x] (reduce RT/Add x)) (+ 1 2 3 4)", 10)]
		[DataRow("(RT/Unknown x y)", typeof(InteropException))]
		[DataRow("(Unknown/unknown x y)", typeof(InteropException))]
		public void Interop_Tests(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(RT/Add)", 0)]
		[DataRow("(RT/Add 1)", 1)]
		[DataRow("(RT/Add 1 2)", 3)]
		[DataRow("(RT/Add 1 2 3)", typeof(ArityException))]
		public void Interop_Multi_Arity_Tests(string s, object expected) => Run_And_Compare(s, expected);

		[DataTestMethod]
		// [DataRow("(reduce RT/Add)", typeof(ArityException))]
		// [DataRow("(reduce RT/Add [1])", 1)]
		// [DataRow("(reduce RT/Add [1 2])", 3)]
		// [DataRow("(reduce RT/Add [1 2 3])", 6)]
		// [DataRow("(reduce RT/Add [1 2 3 4])", 10)]
		// [DataRow("(reduce RT/Add `(1 2 3 4))", 10)]
		[DataRow("(reduce + [1 2 3 4 5])", 15)]
		[DataRow("(reduce * [1 2 3 4 5])", 120)]
		public void Reduce_Tests(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		// [DataRow("(if true true)", true)]
		// [DataRow("(if true false)", false)]
		[DataRow("(if true 1 2)", 1)]
		[DataRow("(if false 1 2)", 2)]
		[DataRow("(if (> 3 2) 1 2)", 1)]
		// [DataRow("(RT/str (if true `x `y))", "x")]
		// [DataRow("(RT/str (if false `x `y))", "y")]
		public void If_Tests(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(str [1 2 3])", "[1 2 3]")]
		[DataRow("(apply if [true false true])", false)]
		// [DataRow("(apply if [true true false])", true)]
		// [DataRow("(apply RT/str ['a' 'b' 'c'])", "abc")]
		// [DataRow("(apply RT/str)", typeof(ArityException))]
		// [DataRow("(apply)", typeof(ArityException))]
		// [DataRow("(apply RT/str 1)", typeof(InvalidCastException))]
		// [DataRow("(apply true [1])", typeof(InvalidOperationException))]
		public void Apply_Tests(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(str nil)", "nil")]
		// [Ignore] [DataRow("(str 'a' nil)", "a")]
		// [Ignore] [DataRow("(str nil 'a')", "a")]
		public void Str_Tests(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(+ 1 2)", 3)]
		public void Simple_Tests(string code, object expected) => Run_And_Compare(code, expected);

		// [DataTestMethod]
		// // [Ignore] [DataRow("(true)", typeof(InvalidOperationException))]
		// public void Symbolic_Expression_Tests(string code, object expected) => Run_And_Compare(code, expected);

		[DataTestMethod]
		[DataRow("(defn fact [n] (loop [current n next (dec current) total 1] (if (> current 1) (recur next (dec next) (* total current)) total))) (fact 10)", 3628800)]
		public void Factorial_loop(string code, object expected) => Run_And_Compare(code, expected);

    }
}