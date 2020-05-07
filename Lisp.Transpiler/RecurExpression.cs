using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class RecurExpression : IExpression
	{
		private IEnumerable<IExpression> valueExpressions;

		public RecurExpression()
		{
		}

		public RecurExpression(ReaderList list)
		{
			valueExpressions = list.Children.Skip(1).Select(ri => ri.BuildExpressionTree());
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "recur")
			{
				return new RecurExpression(list);
			}
			return null;
		}

		public string Transpile() {
			var values = valueExpressions.Transpile().CommaJoin();
			return $"new {nameof(Lisp.Transpiler.RecurSignal)}({values})";
		}
	}
}