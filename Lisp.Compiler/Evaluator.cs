using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Lisp.Compiler
{
	public static class Evaluator
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		}
    }
}