using System.Collections;
using System.Collections.Generic;

namespace Lisp.Reader
{
	public class ReaderVector : ReaderItem, IEnumerable<ReaderItem>
	{
		public IEnumerable<ReaderItem> Children {get;}

		public ReaderVector(IEnumerable<ReaderItem> children):base(null)
		{
			Children = children;
		}

		public IEnumerator<ReaderItem> GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Children).GetEnumerator();
		}
	}
}