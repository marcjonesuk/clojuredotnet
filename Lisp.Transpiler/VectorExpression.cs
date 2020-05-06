using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class VectorExpression : IExpression
	{
		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderVector v)
			{
				return new VectorExpression(v);
			}
			return null;
		}

		public List<IExpression> Items { get; }

		public VectorExpression()
		{
		}

		private VectorExpression(ReaderVector vector)
		{
			Items = vector.Select(item => item.BuildExpressionTree()).ToList();
		}

		public string Transpile()
		{
			var items = string.Join(", ", Items.Select(item => item.Transpile()));
			return $"new Vector(new object[] {{{items}}})";
		}
	}
}