using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public class VectorExpression : IExpression, IFn, IEnumerable<object>
	{
		public VectorExpression(IEnumerable<IExpression> items)
		{
			Items = items;
		}

		public IEnumerable<IExpression> Items { get; }
		public IExpression Parent { get; set; }
		public IEnumerator<object> GetEnumerator() => Items.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
		public object Invoke(object[] args) => Items.Select(i => i.Eval()).ToImmutableArray();
	}
}
