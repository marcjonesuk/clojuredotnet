using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class OperatorExpression : IExpression
	{
		public List<IExpression> Items { get; }
		private Dictionary<string, string> operatorMap = new Dictionary<string, string>();
		public string Operator { get; }

		public OperatorExpression()
		{
			operatorMap["+"] = "";
		}

		public OperatorExpression(ReaderList list)
		{
			Items = list.Select(item => item.BuildExpressionTree()).ToList();
			Operator = Items[0].Transpile();
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && (sym.Name == "+" || sym.Name == ">" || sym.Name == "inc"))
			{
				return new OperatorExpression(list);
			}
			return null;
		}

		public string Transpile()
		{
			// TODO the list below doesnt match the list above
			var left = Items[1].Transpile();
			var right = Items.Count > 2 ? Items[2].Transpile() : null;
			return Operator switch
			{
				"add" => $"((dynamic){left} + (dynamic){right})",
				"sub" => $"((dynamic){left} - (dynamic){right})",
				"mult" => $"((dynamic){left} * (dynamic){right})",
				"inc" => $"((dynamic){left} + 1)",
				"gt" => $"((dynamic){left} > (dynamic){right})",
				"lt" => $"((dynamic){left} < (dynamic){right})",
				"gte" => $"((dynamic){left} >= (dynamic){right})",
				"lte" => $"((dynamic){left} <= (dynamic){right})",
				_ => throw new Exception($"Unknown operator {Operator}")
			};
		}
	}
}