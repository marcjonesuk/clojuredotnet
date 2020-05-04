using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	// User defined function with tail call recursion optimisation with recur
	// Multi arity functions are wrapped with MultiArityFunction
	public class Fn : IFn
	{
		private readonly Symbol[] arguments;
		private object body;
		public bool IsVariadic { get; private set; }

		public Fn(object arguments, object body)
		{
			this.arguments = arguments.As<IEnumerable<object>>().Cast<Symbol>().ToArray();
			this.body = body;
			if (this.arguments.Length > 0 && this.arguments.Last().IsVariadic) IsVariadic = true;
			// this.body.ValidateBodyVars(this.arguments.ToImmutableHashSet<Symbol>());
			Close();
		}

		public Fn(IEnumerable<Symbol> arguments, object body)
		{
			this.arguments = arguments.ToArray();
			this.body = body;
			if (this.arguments.Length > 0 && this.arguments.Last().IsVariadic) IsVariadic = true;
			// this.body.ValidateBodyVars();
			Close();
		}

		public static implicit operator Func<object, bool>(Fn fn)
		{
			return (obj) => (bool)fn.Invoke(new object[] { obj });
		}

		private void ValidateBody()
		{
			var locals = this.arguments.ToHashSet();
			body.DeepFind(o => o is Symbol sym
				&& !sym.IsInterop
				&& !locals.Contains(sym)
				&& !Environment.Current.Contains(sym.Name),
				o => throw new System.Exception($"Use of undeclared Var {((Symbol)o).Name}"));
		}

		private void Close()
		{
			var locals = this.arguments.ToHashSet();
			if (body is SymbolicExpression symbolicExpression)
			{
				var state = symbolicExpression.Env;
				body.DeepFind(o => o is Symbol sym
					&& !sym.IsInterop
					&& !locals.Contains(sym)
					&& !Environment.Root.Contains(sym.Name),
					o =>
					{
						var name = ((Symbol)o).Name;
						if (Environment.Current.Contains(name))
							state[name] = Environment.Current[name];
					}
				);
			}
			else
			{
				if (body is Symbol sym && !sym.IsInterop
					&& !locals.Contains(sym)
					&& !Environment.Root.Contains(sym.Name))
					body = Environment.Current[sym.Name];
			}
		}

		private void BindArgumentValues(object[] args)
		{
			var state = (body is SymbolicExpression symbolicExpression) ? symbolicExpression.Env : Environment.Current;
			object[] values = new object[arguments.Length];
			for (var i = 0; i < arguments.Length; i++)
			{
				if (i == arguments.Length - 1 && arguments[i].IsVariadic)
				{
					state[arguments[i].Name] = args[i..];
				}
				else
				{
					state[arguments[i].Name] = args[i];
				}
			}
		}

		private void ValidateArity(int count)
		{
			if (IsVariadic)
			{
				if (count < arguments.Length - 1) throw new ArityException(count);
			}
			else
			{
				if (count != arguments.Length) throw new ArityException(count);
			}
		}

		public object Invoke(object[] args)
		{
			ValidateArity(args.Length);
			BindArgumentValues(args);
			object result = null;
			while (true)
			{
				result = body.Eval();
				if (result is RecurSignal recur)
					BindArgumentValues(recur.Args);
				else
					break;
			}
			return result;
		}

		public override string ToString() => $"(fn [{arguments.Stringify()}] {body.Stringify()})";
	}
}