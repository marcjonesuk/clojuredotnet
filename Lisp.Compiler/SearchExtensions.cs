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

			if (o is Symbol symbol && !symbol.IsInterop && !knownArgs.Contains(symbol) && !Environment.Current.Contains(symbol.Name))
				throw new System.Exception($"Use of undeclared Var {((Symbol)o).Name}");

			if (o is string) return knownArgs;
			if (o is IEnumerable<object> e)
				foreach (var item in e)
					knownArgs = item.ValidateBodyVars(knownArgs);

			return knownArgs;
		}
	}
}