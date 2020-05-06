using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class LetExpression : IExpression
	{
		private List<IExpression> arguments = new List<IExpression>();

		public LetExpression()
		{
		}

		public LetExpression(ReaderList list)
		{
			var children = list.Children.ToList();
			var bindings = children[1].BuildExpressionTree();
			if (!(bindings is VectorExpression)) throw new Exception("Expected vector expression");
			var body = children[2].BuildExpressionTree();

		}

		public ReaderItem Source { get; }

		public bool Build(ReaderItem item)
		{
			return (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "let");
		}

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
			return "var ";
		}
	}
}