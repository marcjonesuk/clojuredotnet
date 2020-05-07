using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class IfExpression : IExpression
	{
		public ReaderItem Source { get; }
		public List<IExpression> Items { get; }

		public IfExpression()
		{
		}

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol symbol && symbol.Name == "if")
				return new IfExpression(list);
			else
				return null;
		}

		private IfExpression(ReaderList list)
		{
			Items = list.Select(item => item.BuildExpressionTree()).ToList();
		}

		public string Transpile()
		{
			// var args = string.Join(", ", Items.Skip(2).Select(i => $"(Fn)(_ => {i.Transpile()})"));
			// var condition = Items[1].Transpile();
			// var eval = $"RT.If({condition}, {args})";
			// return eval;

			var trueBranch = Items[1].Transpile();
			var falseBranch = Items[1].Transpile();
			return $"RT.Truthy({Items[1].Transpile()}) ? {Items[2].Transpile()} : {Items[3].Transpile()}";
		}
	}
}