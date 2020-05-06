using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class FnExpression : IExpression
	{
		private List<string> arguments = new List<string>();
		private IExpression body;
		public string[] Locals {get;}

		public FnExpression()
		{
		}

		public FnExpression(ReaderList list)
		{
			var children = list.Children.ToList();
			var arguments = (VectorExpression)children[1].BuildExpressionTree();
			Locals = arguments.Items.Cast<SymbolExpression>().Select(s => s.Name).ToArray();
			body = children[2].BuildExpressionTree();
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "fn")
			{
				return new FnExpression(list);
			}
			return null;
		}

		public string Transpile()
		{
			var locals = string.Join(", ", Locals);
			var values = string.Join(", ", Enumerable.Range(0, Locals.Length).Select(l => $"args[{l}]"));

			var vars = "";
			for(var l = 0; l < Locals.Length; l++) {
				vars += $" var {Locals[l]} = (dynamic)args[{l}];";
			}

			return $"((Fn)((args) => {{{vars} return {body.Transpile()}; }}))";
		}
	}
}