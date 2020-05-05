// using System;
// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Linq;

// namespace Lisp.Compiler
// {
// 	public class Functioniser
// 	{
// 		private readonly Symbol[] arguments;
// 		private object body;
// 		public bool IsVariadic { get; private set; }
// 		private Environment state;
// 		private IFn bodyFn = null;

// 		public Functioniser(IEnumerable<Symbol> arguments, object body)
// 		{
// 			this.arguments = arguments.ToArray();
// 			this.body = body;
// 			if (this.arguments.Length > 0 && this.arguments.Last().IsVariadic) IsVariadic = true;
// 			// this.body.ValidateBodyVars();
// 			Close();
// 			this.bodyFn = body as IFn;
// 			this.state = (body is SymbolicExpression symbolicExpression) ? symbolicExpression.Env : Environment.Current;


// 			if (body is SymbolicExpression se)
// 			{
// 				se.Locals = this.arguments;
// 			}
// 		}

// 		public static void Functionize(IEnumerable<Symbol> arguments, SymbolicExpression expression)
// 		{
// 			expression.Locals = arguments.ToArray();
// 			// // Localise local arguments
// 			// var locals = arguments.ToHashSet();
// 			// foreach(var item in expression.Items) {

// 			// }
// 		}

// 		private void ValidateBody()
// 		{
// 			var locals = this.arguments.ToHashSet();
// 			body.DeepFind(o => o is Symbol sym
// 				&& !sym.IsInterop
// 				&& !locals.Contains(sym)
// 				&& !Environment.Current.Contains(sym.Name),
// 				o => throw new System.Exception($"Use of undeclared Var {((Symbol)o).Name}"));
// 		}

// 		private void Close()
// 		{
// 			var locals = this.arguments.ToHashSet();
// 			if (body is SymbolicExpression symbolicExpression)
// 			{
// 				var state = symbolicExpression.Env;
// 				body.DeepFind(o => o is Symbol sym
// 					&& !sym.IsInterop
// 					&& !locals.Contains(sym)
// 					&& !Environment.Root.Contains(sym.Name),
// 					o =>
// 					{
// 						var name = ((Symbol)o).Name;
// 						if (Environment.Current.Contains(name))
// 							state[name] = Environment.Current[name];
// 					}
// 				);
// 			}
// 			else
// 			{
// 				if (body is Symbol sym && !sym.IsInterop
// 					&& !locals.Contains(sym)
// 					&& !Environment.Root.Contains(sym.Name))
// 					body = Environment.Current[sym.Name];
// 			}
// 		}

// 		private void BindArgumentValues(object[] args)
// 		{
// 			object[] values = new object[arguments.Length];
// 			for (var i = 0; i < arguments.Length; i++)
// 			{
// 				if (i == arguments.Length - 1 && arguments[i].IsVariadic)
// 				{
// 					state[arguments[i].Name] = args[i..];
// 				}
// 				else
// 				{
// 					state[arguments[i].Name] = args[i];
// 				}
// 			}
// 		}

// 		private void ValidateArity(int count)
// 		{
// 			if (IsVariadic)
// 			{
// 				if (count < arguments.Length - 1) throw new ArityException(count);
// 			}
// 			else
// 			{
// 				if (count != arguments.Length) throw new ArityException(count);
// 			}
// 		}

// 		public object Invoke(object[] args)
// 		{
// 			ValidateArity(args.Length);
// 			// BindArgumentValues(args);
// 			object result = null;
// 			while (true)
// 			{
// 				if (bodyFn != null)
// 					result = bodyFn.Invoke(args);
// 				else
// 					result = body.Eval();

// 				if (result is RecurSignal recur)
// 					BindArgumentValues(recur.Args);
// 				else
// 					break;
// 			}
// 			return result;
// 		}

// 		public override string ToString() => $"(fn [{arguments.Stringify()}] {body.Stringify()})";
// 	}
// }