using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Transpiler
{
	public class RT
	{
		public static object Add(object x, object y) => (dynamic)x + (dynamic)y;

		public static Fn Print(object obj)
		{
			Console.WriteLine(obj);
			return null;
		}


		public static bool Truthy(object value)
		{
			return value switch
			{
				null => false,
				false => false,
				_ => true
			};
		}

		public static object If(Fn condition, Fn branch1)
		{
			if (Truthy(condition(null)))
				return branch1(null);
			else
				return null;
		}

		public static object If(Fn condition, Fn branch1, Fn branch2)
		{
			if (Truthy(condition(null)))
				return branch1(null);
			else
				return branch2(null);
		}

		public static string Str(object o, bool quoteStrings = false)
		{
			if (o == null) return "nil";
			if (o.Equals(true)) return "true";
			if (o.Equals(false)) return "false";

			var s = o switch
			{
				bool b => b.ToString(),
				Vector vector => vector.ToString(),
				IEnumerable<object> list => "(" + string.Join(' ', list.Select(item => Str(item, true))) + ")",
				string strng => quoteStrings ? "'" + strng + "'" : strng,
				// IStringify str => str.Stringify(quoteStrings),
				// ImmutableDictionary<object, object> arr => "{" + string.Join(' ', arr.Select(item => item.Key.Stringify(quoteStrings) + " " + item.Value.Stringify(quoteStrings))) + "}",
				// ImmutableHashSet<object> arr => "#{" + string.Join(' ', arr.Select(item => item.Stringify(quoteStrings))) + "}",
				// object[] array => "[" + string.Join(' ', array.Select(item => item.Stringify(quoteStrings))) + "]",
				// ImmutableArray<object> arr => "[" + string.Join(' ', arr.Select(item => item.Stringify(quoteStrings))) + "]",
				// IEnumerable<object> list => "(" + string.Join(' ', list.Select(item => item.Stringify(quoteStrings))) + ")",
				_ => o.ToString()
			};
			return s;
		}

		public static object Conj(object coll, object item)
		{
			if (coll is IList<object> list)
			{
				list.Add(item);
				return list;
			}
			throw new Exception();
		}

		public static object Get(object coll, object index)
		{
			if (coll is IList<object> list)
			{
				return list[(dynamic)index];
			}
			throw new Exception();
		}

		public static bool Eq(object arg1, object arg2) => (dynamic)arg1 == (dynamic)arg2;
		public static bool Gt(object arg1, object arg2) => (dynamic)arg1 > (dynamic)arg2;
		public static bool Lt(object arg1, object arg2) => (dynamic)arg1 < (dynamic)arg2;
		public static bool Gte(object arg1, object arg2) => (dynamic)arg1 >= (dynamic)arg2;
		public static bool Lte(object arg1, object arg2) => (dynamic)arg1 <= (dynamic)arg2;

		public static object Eval(Fn fn) => fn(null);
		public static object Eval(Fn fn, object arg1) => fn(new object[] { arg1 });
		public static object Eval(Fn fn, object arg1, object arg2) => fn(new object[] { arg1, arg2 });
		public static object Eval(Fn fn, object arg1, object arg2, object arg3) => fn(new object[] { arg1, arg2, arg3 });

		public static IEnumerable<object> Seq(object o)
		{
			if (o == null) return new object[] { };
			if (o is IDictionary<object, object> dict) return dict.Select(kvp => (object)ImmutableArray.Create(kvp.Key, kvp.Value));
			if (o is string s) return s.Select(item => (object)item.ToString());
			return ((IEnumerable<object>)o).Select(i => i);
		}
	}
}