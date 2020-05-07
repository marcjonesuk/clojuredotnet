using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class DefnExpression : IExpression
	{
		private List<string> arguments = new List<string>();
		private IExpression body;
		private IExpression symbol;
		public string[] Locals { get; }

		public DefnExpression()
		{
		}

		public DefnExpression(ReaderList list)
		{
			var children = list.Children.ToList();
			var arguments = (VectorExpression)children[2].BuildExpressionTree();
			Locals = arguments.Items.Cast<SymbolExpression>().Select(s => s.Name).ToArray();
			symbol = ((ReaderSymbol)children[1]).BuildExpressionTree();
			body = children[3].BuildExpressionTree();
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "defn")
			{
				return new DefnExpression(list);
			}
			return null;
		}

		public string Transpile()
		{
			var values = string.Join(", ", Enumerable.Range(0, Locals.Length).Select(l => $"args[{l}]"));
			var vars = "";
			for (var l = 0; l < Locals.Length; l++)
			{
				vars += $" var {Locals[l]} = (dynamic)args[{l}];";
			}
			return $"var {symbol.Transpile()} = ((Fn)((args) => {{{vars} return {body.Transpile()}; }}))";

			// (args) => {
			// args.Length switch {
			//	1 => {},
			//  2 => {},
			//  _ => {}
			// }
			// }
		}
	}
}