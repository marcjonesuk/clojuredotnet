// using System;
// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Linq;
// using System.Threading.Tasks;

// namespace Lisp.Compiler 
// {
// 	public class DefMacro : IFn
// 	{
// 		public async Task<object> Invoke(object[] args)
// 		{
// 			var symbol = args[0] as Symbol;
// 			var macroArgs = args[1].As<IEnumerable<object>>();
// 			var macro = args[2] as SymbolicExpression;

// 			var fn = new Macro(macroArgs.Select(a => ((Symbol)a).Name), a =>
// 			{
// 				var x = macro.Invoke(null);
// 				return x;
// 			});

// 			State.Current.Macros.Add(symbol.Name, fn);
// 			return symbol;
// 		}
// 	}
// }