using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Lisp.Compiler
{
	public static class Evaluator
    {
		public static object Eval(this object o, object[] args = null)
        {
			return o switch {
				null => o,
				int i => o,
				double d => o,
				string s => o,
				bool b => o,
				IFn fn => fn.Invoke(args),
				ImmutableHashSet<object> set => set.Select(i => i.Eval()).ToImmutableHashSet(new CustomComparer()),
            	ImmutableArray<object> array => array.Select(i => i.Eval()).ToImmutableArray(),
 			 	ImmutableList<object> list => list.Select(i => i.Eval()).ToImmutableList(),
            	IEnumerable<object> enumerable => enumerable.Select(i => i.Eval()),
				_ => throw new Exception()	
			};

            // // Primitives
            // if (o == null) return null;
            // else if (o is int) return o;
            // else if (o is double) return o;
            // else if (o is string) return o;
            // else if (o is bool) return o;

            // // Protect downstream fns from null args
            // if (args == null) args = Array.Empty<object>();

			// if (o is IFn fn) return fn.Invoke(args);
            
			// else if (args != null && args.Length > 0) throw new InvalidOperationException($"Unable to invoke {o.Stringify(true)} ({o.GetType()}) as function");
            // else if (o is ImmutableHashSet<object> set) return set.Select(i => i.Eval()).ToImmutableHashSet(new CustomComparer());
            // else if (o is ImmutableArray<object> array) return array.Select(i => i.Eval()).ToImmutableArray();
            // else if (o is ImmutableList<object> list) return list.Select(i => i.Eval()).ToImmutableList();
            // else if (o is IEnumerable<object> enumerable) return enumerable.Select(i => i.Eval());
            // else return o;
        }

        public static object EvalOld(this object o, object[] args = null)
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
            
			else if (args != null && args.Length > 0) throw new InvalidOperationException($"Unable to invoke {o.Stringify(true)} ({o.GetType()}) as function");
            else if (o is ImmutableHashSet<object> set) return set.Select(i => i.Eval()).ToImmutableHashSet(new CustomComparer());
            else if (o is ImmutableArray<object> array) return array.Select(i => i.Eval()).ToImmutableArray();
            else if (o is ImmutableList<object> list) return list.Select(i => i.Eval()).ToImmutableList();
            else if (o is IEnumerable<object> enumerable) return enumerable.Select(i => i.Eval());
            else return o;
        }
    }
}