using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public static class SearchExtensions
	{
		public static void DeepFind(this object o, Func<object, bool> predicate, Action<object> action)
		{
			if (predicate(o))
				action(o);

			if (o is string) return;
			if (o is IEnumerable<object> e)
				foreach (var item in e)
					item.DeepFind(predicate, action);
		}

		public static ImmutableHashSet<Symbol> GetBindings(SymbolicExpression sexpr) {
			var sym = sexpr.Items[0] as Symbol;
			if (sym == null) return null;
			return sym.Name switch {
				"fn" => sexpr.Items[1].As<IEnumerable<object>>().Cast<Symbol>().ToImmutableHashSet(),
				"let" => null,
				_ => null
			};
		}

		public static ImmutableHashSet<Symbol> ValidateBodyVars(this object o, ImmutableHashSet<Symbol> knownArgs = null)
		{
			if (knownArgs == null) knownArgs = ImmutableHashSet<Symbol>.Empty;
			if (o is SymbolicExpression sexpr) {
				var bindings = GetBindings(sexpr);
				knownArgs = knownArgs.Union(bindings);
			}

			if (o is Symbol symbol && !symbol.IsInterop && !knownArgs.Contains(symbol) && !State.Current.Contains(symbol.Name))
				throw new System.Exception($"Use of undeclared Var {((Symbol)o).Name}");

			if (o is string) return knownArgs;
			if (o is IEnumerable<object> e)
				foreach (var item in e)
					knownArgs = item.ValidateBodyVars(knownArgs);

			return knownArgs;
		}
	}

	public static class Evaluator
	{
		public static object Eval(this object o, object[] args = null)
		{
			if (o == null) return null;
			if (args == null) args = new object[0];
			if (o is InteropDelegate d) return d(args);
			if (o is InteropDelegate0Arg interop0)
			{
				if (args.Length != 0) throw new ArityException(args.Length);
				return interop0();
			}
			if (o is InteropDelegate1Arg interop1)
			{
				if (args.Length != 1) throw new ArityException(args.Length);
				return interop1(args[0]);
			}
			if (o is InteropDelegate2Arg interop2)
			{
				if (args.Length != 2) throw new ArityException(args.Length);
				return interop2(args[0], args[1]);
			}
			if (o is InteropDelegate3Arg interop3) return interop3(args[0], args[1], args[2]);
			if (o is IFn fn) return fn.Invoke(args);
			if (o is ImmutableHashSet<object> set) return set.Select(i => i.Eval()).ToImmutableHashSet(new CustomComparer());
			if (o is ImmutableArray<object> array) return array.Select(i => i.Eval()).ToImmutableArray();
			if (o is ImmutableList<object> list) return list.Select(i => i.Eval()).ToImmutableList();
			if (o is IEnumerable<object> enumerable) return enumerable.Select(i => i.Eval());
			if (args != null && args.Length > 0) throw new InvalidOperationException($"Unable to invoke {o.Stringify(true)} ({o.GetType()}) as function");
			return o;
		}
	}
}
