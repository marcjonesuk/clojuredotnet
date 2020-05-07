using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Lisp.Transpiler
{
	public class RT
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static object Add(dynamic x, dynamic y) => x + y;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static object Sub(dynamic x, dynamic y) => x - y;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static object Mult(dynamic x, dynamic y) => x * y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static object Inc(dynamic x) => ++x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static object Dec(dynamic x) => --x;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static Fn Print(object obj)
		{
			Console.WriteLine(obj);
			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static bool Truthy(object value)
		{
			return value switch
			{
				null => false,
				false => false,
				_ => true
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static object If(Fn condition, Fn branch1)
		{
			if (Truthy(condition(null)))
				return branch1(null);
			else
				return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static object If(object condition, Fn branch1, Fn branch2)
		{
			if (Truthy(condition))
				return branch1(null);
			else
				return branch2(null);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
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

		public static object Reduce(object fn, IEnumerable<object> values)
		{
			var function = (Fn)fn;
			object value = null;

			// if (args.Length < 2 || args.Length > 3)
			//     throw new ArityException(args.Length);

			// if (args.Length == 1) {
			// target = (IEnumerable<object>)args[1];
			// }
			// else if (args.Length == 2)
			// {
			//     value = args[1];
			//     target = (IEnumerable<object>)args[2];
			// }

			// bool hasItems = false;
			foreach (var item in values)
			{
				// hasItems = true;
				if (value == null)
				{
					value = item;
					continue;
				}
				value = function(new object[] { value, item });
			}
			// if (!hasItems)
			// {
			//     if (args.Length == 3)
			//         return function.Eval(new object[] { value });
			//     else
			//         return function.Eval();
			// }
			return value;
		}

		public static object Time(Fn body)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			object result = body();
			sw.Stop();
			Print($"Time (ms): {sw.ElapsedMilliseconds}");
			return result;
		}

		public static bool Eq(object arg1, object arg2) => (dynamic)arg1 == (dynamic)arg2;
		public static bool Gt(object arg1, object arg2) => (dynamic)arg1 > (dynamic)arg2;
		public static bool Lt(object arg1, object arg2) => (dynamic)arg1 < (dynamic)arg2;
		public static bool Gte(object arg1, object arg2) => (dynamic)arg1 >= (dynamic)arg2;
		public static bool Lte(object arg1, object arg2) => (dynamic)arg1 <= (dynamic)arg2;

		public static object Eval(object fn) => ((Fn)fn)(null);
		public static object Eval(object fn, object arg1) => ((Fn)fn)(new object[] { arg1 });
		public static object Eval(object fn, object arg1, object arg2) => ((Fn)fn)(new object[] { arg1, arg2 });
		public static object Eval(object fn, object arg1, object arg2, object arg3) => ((Fn)fn)(new object[] { arg1, arg2, arg3 });

		public static IEnumerable<object> Seq(object o)
		{
			if (o == null) return new object[] { };
			if (o is IDictionary<object, object> dict) return dict.Select(kvp => (object)ImmutableArray.Create(kvp.Key, kvp.Value));
			if (o is string s) return s.Select(item => (object)item.ToString());
			return ((IEnumerable<object>)o).Select(i => i);
		}
	}

	public struct RecurSignal
	{
		public RecurSignal(params object[] args)
		{
			Args = args;
		}

		public object[] Args { get; }
	}

	// public class Recur : Fn
	// {
	// 	public object Invoke()
	// 	{
	// 		throw new System.Exception();
	// 	}

	// 	public object Invoke(object[] args)
	// 	{
	// 		return new RecurSignal(args);
	// 	}
	// }

}