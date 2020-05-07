using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class HashMapExpression : IExpression
	{
		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderHashMap hm)
			{
				return new HashMapExpression(hm);
			}
			return null;
		}

		public List<IExpression> Items { get; }

		public HashMapExpression()
		{
		}

		private HashMapExpression(ReaderHashMap hashmap)
		{			
			Items = hashmap.Select(item => item.BuildExpressionTree()).ToList();
		}

		public string Transpile()
		{
			var items = string.Join(", ", Items.Select(item => item.Transpile()));
			return $"new HashMap(new object[] {{{items}}})";
		}
	}
}