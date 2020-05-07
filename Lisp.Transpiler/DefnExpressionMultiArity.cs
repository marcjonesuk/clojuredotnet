using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class FnDescriptor
	{
		public int Arity { get; set; }
		public string[] Locals { get; set; }
		public IExpression Body { get; set; }
	}

	public class DefnMultiArityExpression : IExpression
	{
		private List<string> arguments = new List<string>();
		Dictionary<int, FnDescriptor> arities = new Dictionary<int, FnDescriptor>();
		private IExpression body;
		private IExpression symbol;

		public DefnMultiArityExpression()
		{
		}

		public DefnMultiArityExpression(ReaderList list)
		{
			var children = list.Children.ToList();
			foreach (var item in list.Skip(2))
			{
				var f = (ReaderList)item;
				var fItems = f.ToList();
				var arguments = (VectorExpression)fItems[0].BuildExpressionTree();
				var body = fItems[1].BuildExpressionTree();
				arities[arguments.Items.Count] =
					new FnDescriptor()
					{
						Locals = arguments.Items.Select(a => ((SymbolExpression)a).Name).ToArray(),
						Body = body
					};
			}
			// var arguments = (VectorExpression)children[2].BuildExpressionTree();
			// Locals = arguments.Items.Cast<SymbolExpression>().Select(s => s.Name).ToArray();
			symbol = ((ReaderSymbol)children[1]).BuildExpressionTree();
			// body = children[3].BuildExpressionTree();
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "defn" && list.ToList()[2] is ReaderList)
			{
				return new DefnMultiArityExpression(list);
			}
			return null;
		}

		public string Transpile()
		{
			var cases = "";
			foreach (var a in arities.Keys)
			{
				var arity = arities[a];
				var values = string.Join(", ", Enumerable.Range(0, arity.Locals.Length).Select(l => $"args[{l}]"));
				var vars = "";
				for (var l = 0; l < arity.Locals.Length; l++)
				{
					vars += $" var {arity.Locals[l]} = (dynamic)args[{l}];";
				}
				var body = $"((Fn)((a) => {{{vars} return {arity.Body.Transpile()}; }}))(args)";
				if (a == 0)
					body = arity.Body.Transpile();
				cases += $"{a} => {body},\n";
			}

			//  ((Fn)((args) => {{{vars} return {body.Transpile()}; }}))

			// var cases = string.Join(",\n", arities.Keys.Select(a => $"{a} => { this.arities[a].Body.Transpile() }"));

			var fn = $@"
var {symbol.Transpile()} = ((Fn)((args) =>  
	args.Length switch {{
		{cases}
		_ => throw new ArityException(args.Length)
	}}))";
			// 0 => { this.arities[0].Body.Transpile() },
			// 		1 => null,
			// 		_ => throw new ArityException(args.Length),
			return fn;
			// ((Fn)((args) => {{{vars} return {body.Transpile()}; }}))";

			// var code = $@"
			// (args) => {
			// args.Length switch {
			// 	1 => {},
			//  2 => {},
			//  _ => {}
			// }
			// }"
		}
	}
}