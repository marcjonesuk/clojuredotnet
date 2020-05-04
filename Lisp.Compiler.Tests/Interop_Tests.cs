using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	[TestClass]
	public class Interop_Tests
	{
		public void Run_And_Compare(string code, object expected)
		{
			if (!(expected is Type t))
			{
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

		[TestMethod]
		public void Test1()
		{

		}


		[DataTestMethod]
		[DataRow("A/X", "A", "X")]
		[DataRow("A.B.C/X", "A.B.C", "X")]
		[DataRow("A.B.C/X<Y>", "A.B.C", "X", new string[] { "Y" })]
		[DataRow("A.B.C/X<Y1,Y2>", "A.B.C", "X", new string[] { "Y1", "Y2" })]
		[DataRow("A.B.C/X(Y1,Y2)", "A.B.C", "X", null, new string[] { "Y1", "Y2" })]
		[DataRow("A.B.C/X<T1,T2>(Y1,Y2)", "A.B.C", "X", new string[] { "T1", "T2" }, new string[] { "Y1", "Y2" })]
		[DataRow("A.B.C/X<T1,T2>(Y1<T1>,Y2<T2>)", "A.B.C", "X", new string[] { "T1", "T2" }, new string[] { "Y1<T1>", "Y2<T2>" })]
		[DataRow("A.B.C/X<T1,T2>(Y1,Y2)", "A.B.C", "X", new string[] { "T1", "T2" }, new string[] { "Y1", "Y2" })]
		// [DataRow("System.Linq.Enumerable/TakeWhile<System.Object>(System.Object,Func<bool,System.Object>)", "System.Linq.Enumerable", "TakeWhile", new string[] { "System.Object" }, new string[] { "System.Object", "Func<bool,System.Object>" })]
		public void Reader_Tests(string value, string typeName, string methodName, string[] genericTypes = null, string[] parameterTypes = null)
		{
			var result = InteropReader.Read(value);
			Assert.AreEqual(typeName, result.TypeName);
			Assert.AreEqual(methodName, result.MethodName);
			CollectionAssert.AreEqual(genericTypes, result.GenericTypeParameters);
			CollectionAssert.AreEqual(parameterTypes, result.ParameterTypes);
		}

		[DataTestMethod]
		[DataRow("A,B", new string[] { "A", "B" }, null)]
		[DataRow("A,B<C,D>", new string[] { "A", "B<C,D>" }, null)]
		[DataRow("A,B<C,D>,E", new string[] { "A", "B<C,D>", "E" }, null)]
		[DataRow("A,B<C,D<E>>,F", new string[] { "A", "B<C,D<E>>", "F" }, null)]
		public void TypeList_Reader_Tests(string value, string[] parameterTypes, object ignore = null)
		{
			var result = InteropReader.ReadTypeList(value).ToArray();
			CollectionAssert.AreEqual(parameterTypes, result);
		}

		[DataTestMethod]
		// [DataRow("(.ToUpper 'abc')", "ABC")]
		// [DataRow("(.StartsWith 'abc' 'a')", true)]

		// [DataRow("(defn take [coll, count] (System.Linq.Enumerable/Take<System.Object> (seq coll) count)) (first (take2 [1 2] 1))", 1)]
		// [DataRow("(defn takewhile ([coll, predicate] (System.Linq.Enumerable/TakeWhile<System.Object> (seq coll) predicate))) (takewhile [1 2 3] (fn [x] (< x 2)))", 1)]
		// [DataRow("(defn count [coll] (System.Linq.Enumerable/Count<System.Object> coll)) (count [1 2])", 2)]
		// [DataRow("(defn where [coll, predicate] (RT/MarcWhere (seq coll) predicate)) (str (where [1 2 3] (fn [x] (< x 3))))", "(1 2)")]
		// [DataRow("(defn takewhile [coll, predicate] ((interop 'System.Linq.Enumerable/TakeWhile<System.Object>(System.Collections.Generic.IEnumerable`1[System.Object],Func`2[System.Object,System.Boolean])') (seq coll) predicate)) (takewhile [1 2 3] (fn [item] true))", null)]
		// [DataRow("(defn log [msg] (System.Console/WriteLine msg)) (log 'hello, world') (log 5)", null)]
		[DataRow("(+ 1 2)", null)]
		public void _Tests_1(string code, object expected) => Run_And_Compare(code, expected);
	}
}
