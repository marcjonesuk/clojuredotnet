using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class DefExpression : IExpression
	{
		private List<IExpression> arguments = new List<IExpression>();
		private IExpression symbol;
		private IExpression value;

		public DefExpression()
		{
		}

		public DefExpression(ReaderList list)
		{
			var children = list.Children.ToList();
			symbol = ((ReaderSymbol)children[1]).BuildExpressionTree();
			value = children[2].BuildExpressionTree();
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "def")
				return new DefExpression(list);
			return null;
		}

		public string Transpile()
		{
			Console.WriteLine("sdfsdf");
			return $"var {symbol.Transpile()} = {value.Transpile()}";
		}
	}
}