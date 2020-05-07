using System.Collections;
using System.Collections.Generic;

namespace Lisp.Reader
{
	public class ReaderVector : ReaderCollection
	{
		public ReaderVector(IEnumerable<ReaderItem> children):base(children)
		{
		}
	}
			public class ReaderSet : ReaderCollection
	{
		public ReaderSet(IEnumerable<ReaderItem> children):base(children)
		{
		}
	}
	public class ReaderHashMap : ReaderCollection
	{
		public ReaderHashMap(IEnumerable<ReaderItem> children):base(children)
		{
		}
	}
	
	public class ReaderCollection : ReaderItem, IEnumerable<ReaderItem>
	{
		public IEnumerable<ReaderItem> Children {get;}

		public ReaderCollection(IEnumerable<ReaderItem> children):base(null)
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
	
	public class ReaderList : ReaderCollection
	{
		public ReaderList(IEnumerable<ReaderItem> children):base(children)
		{
		}
	}
}