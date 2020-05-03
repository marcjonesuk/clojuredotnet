using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public delegate object InteropDelegate(object[] args);
	public delegate object InteropDelegate0Arg();
	public delegate object InteropDelegate1Arg(object arg);
	public delegate object InteropDelegate2Arg(object arg1, object arg2);
	public delegate object InteropDelegate3Arg(object arg1, object arg2, object arg3);

	public static class RT
	{
		public static T Get<T>(this object[] args, int position)
		{
			return (T)args[position];
		}

		public static void Expect(this object[] args, int length)
		{
			if (args.Length != length) throw new ArityException(args.Length);
		}

		public static bool IsTruthy(object value)
		{
			return value switch
			{
				null => false,
				false => false,
				_ => true
			};
		}

		public static object Fn(object args, object body)
		{
			var arguments = args.As<IEnumerable<object>>().Cast<Symbol>();
			return new Fn(arguments, body);
		}

		public static object Apply(object[] args)
		{
			if (args.Length <= 1) throw new ArityException(args.Length);
			var last = args.Last().As<IEnumerable<object>>();
			var newArgs = args[1..^1].Concat(last).ToArray();
			return args[0].Eval(newArgs);
		}

		public static object Map(object[] args)
		{
			var target = args[1].AsSeq();
			return target.Select(item => args[0].Eval(new object[] { item }));
		}

		public static object Def(object symbol, object value)
		{
			var sym = (Symbol)symbol;
			Environment.Root[sym.Name] = value.Eval();
			return symbol;
		}

		// public static object Add(dynamic[] args) {
		// 	if (args.Length == 0) return null;
		// 	if (args.Length == 1) return args[0];
		// 	if (args.Length == 2) return args[0] + args[1];

		// 	dynamic total = args[0];
		// 	for(var i = 1; i < args.Length; i++) {
		// 		total += args[i];
		// 	}
		// 	return total;
		// }

		public static object Str(object[] args) => args == null ? "" : string.Join("", args.Select(a => a.Stringify()));

		public static object Add() => 0;
		public static object Add(dynamic x) => x;
		public static object Add(dynamic x, dynamic y) => x + y;
		public static object Subtract(dynamic x, dynamic y) => x - y;
		public static object Multiply(dynamic x, dynamic y) => x * y;
		public static object Divide(dynamic x, dynamic y) => x / y;

		public static object Inc(dynamic x) => ++x;
		public static object Dec(dynamic x) => --x;
		
		public static object Gt(dynamic x, dynamic y) => x > y;
		public static object Lt(dynamic x, dynamic y) => x < y;
		
		public static object Quot(object x, object y) => (int)x / (int)y;
		public static object Mod(object x, object y) => (int)x % (int)y;

		public static object Odd(object x) => (int)x % 2 != 0;
		public static object Even(object x) => (int)x % 2 == 0;

		public static object Max(dynamic x, dynamic y) => Math.Max(x, y);
		public static object Min(dynamic x, dynamic y) => Math.Min(x, y);
		public static object Sin(object[] args) => Math.Sin(args.Get<double>(0));
		public static object Tanh(object[] args) => Math.Tanh(args.Get<double>(0));

		public static object Conj(object[] args)
		{
			args.Expect(2);
			return args[0] switch
			{
				ImmutableArray<object> list => list.Add(args[1]),
				_ => throw new Exception()
			};
		}

		public static object IsString(object o) => o is string;
		public static object IsBool(object o) => o is Boolean;
		public static object IsInt(object o) => o is Int32;
		public static object IsDouble(object o) => o is double;
		public static object IsList(object o) => o is IList<object>;
		public static object IsHashmap(object o) => o is IDictionary<object, object>;
		public static object IsNil(object o) => o == null;

		public static object Hash(object x) => Hash_(x);
		public static object Equiv(object x, object y) => Equiv_(x, y);

		public static int Hash_(object x) {
			if (x == null) return 0;
			if (x is KeyValuePair<object, object> kvp)
				return HashCode.Combine(Hash_(kvp.Key), Hash_(kvp.Value));
			if (x is ISet<object> xset) {
				var hashcode = 0;
				foreach(var item in xset) {
					hashcode = hashcode ^ Hash_(item);
				}
				return hashcode;
			}
			if (x is IDictionary<object, object> xdict) {
				var hashcode = 0;
				foreach(var item in xdict) {
					hashcode = hashcode ^ Hash_(item);
				}
				return hashcode;
			}
			if (x is IEnumerable<object> xe) {
				var hashcode = 0;
				foreach(var item in xe) {
					hashcode = HashCode.Combine(hashcode, Hash_(item));
				}
				return hashcode;
			}
			return x.GetHashCode();
		}

		public static bool Equiv_(object x, object y)
		{
			if (x == null || y == null) return x == y;

			// Sets
			if (x is ISet<object> xset)
			{
				if (y is ISet<object> yset)
				{
					if (xset.Count != yset.Count) return false;
					foreach (var item in xset)
					{
						if (!yset.Contains(item, new CustomComparer()))
							return false;
					}
					return true;
				}
				else return false;
			}
			else if (y is ISet<object>) return false;

			// Hashmaps
			if (x is IDictionary<object, object> xhm)
			{
				if (y is IDictionary<object, object> yhm)
				{
					if (xhm.Count != yhm.Count) return false;
					foreach (var item in xhm)
					{
						if (yhm.TryGetValue(item.Key, out var yval))
						{
							if (!Equiv_(item.Value, yval)) return false;
						}
						else return false;
					}
					return true;
				}
				else return false;
			}
			else if (y is IDictionary<object, object>) return false;

			// Compare contents for all remaining enumerables
			if (x is IEnumerable<object> xe && y is IEnumerable<object> ye)
			{
				return xe.SequenceEqual(ye, new CustomComparer());
			}

			// If all else fails use default Equals 
			return x.Equals(y);
		}
	}
}