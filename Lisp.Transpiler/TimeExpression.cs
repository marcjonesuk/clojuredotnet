using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class TimeExpression : IExpression
	{
		public ReaderItem Source { get; }
		public List<IExpression> Items { get; }

		public TimeExpression()
		{
		}

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol symbol && symbol.Name == "time")
				return new TimeExpression(list);
			else
				return null;
		}

		private TimeExpression(ReaderList list)
		{
			Items = list.Select(item => item.BuildExpressionTree()).ToList();
		}

		public string Transpile()
		{
			var body = Items[1].Transpile();
			return $"RT.Time({body.Wrap()})"; 
		}
	}
}