using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class LetExpression : IExpression
	{
		private VectorExpression arguments;
		private IExpression body;
		private List<KeyValuePair<string, object>> values = new List<KeyValuePair<string, object>>();

		public LetExpression()
		{
		}

		public LetExpression(ReaderList list)
		{
			var children = list.Children.ToList();
			arguments = (VectorExpression)children[1].BuildExpressionTree();
			body = children[2].BuildExpressionTree();
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "let")
			{
				return new LetExpression(list);
			}
			return null;
		}

		public string Transpile()
		{
			var vars = "";
			for (var i = 0; i < arguments.Items.Count; i += 2)
			{
				var name = arguments.Items[i];
				var value = arguments.Items[i+1];
				vars += $" var {name.Transpile()} = {value.Transpile()};";
			}
			return $"((Fn)((_) => {{{vars} return {body.Transpile()}; }}))(null)";
		}
	}
}