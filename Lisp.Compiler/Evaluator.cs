using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public static class Evaluator
	{
		public static object Eval(this object o, object[] args = null)
		{
			// Primitives
			if (o == null) return null;
			else if (o is int) return o;
			else if (o is double) return o;
			else if (o is string) return o;
			else if (o is bool) return o;

			// Protect downstream fns from null args
			if (args == null) args = Array.Empty<object>();

			if (o is IFn fn) return fn.Invoke(args);
			else if (o is ImmutableHashSet<object> set) return set.Select(i => i.Eval()).ToImmutableHashSet(new CustomComparer());
			else if (o is ImmutableArray<object> array) return array.Select(i => i.Eval()).ToImmutableArray();
			else if (o is ImmutableList<object> list) return list.Select(i => i.Eval()).ToImmutableList();
			else if (o is IEnumerable<object> enumerable) return enumerable.Select(i => i.Eval());
			else if (args != null && args.Length > 0) throw new InvalidOperationException($"Unable to invoke {o.Stringify(true)} ({o.GetType()}) as function");
			else return o;
		}
	}
}