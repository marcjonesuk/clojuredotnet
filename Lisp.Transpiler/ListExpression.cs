using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class ListExpression : IExpression
	{
		public ReaderItem Source { get; }
		public List<IExpression> Items { get; }

		public ListExpression()
		{
		}

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list)
				return new ListExpression(list);
			else
				return null;
		}

		private ListExpression(ReaderList list)
		{
			Items = list.Select(item => item.BuildExpressionTree()).ToList();
		}

		public string Transpile()
		{
			var args = string.Join(", ", Items.Skip(1).Select(i => i.Transpile()));
			var fn = Items[0].Transpile();

			if (Items[0] is SymbolExpression && fn.StartsWith("RT") || fn.StartsWith("System"))
			{
				if (fn.StartsWith("."))
				{
					var interopArgs = string.Join(", ", Items.Skip(2).Select(i => i.Transpile()));
					return $"{Items[1].Transpile()}{fn}({interopArgs})";
				}
				else
				{
					return $"{fn}({args})";
				}
			}
			else {
				if (args.Length == 0) 
					// return $"RT.Eval({fn})";
					return $"{fn}()";
				else
					// return $"RT.Eval({fn}, {args})";
					return $"{fn}({args})";
			}
		}
	}
}