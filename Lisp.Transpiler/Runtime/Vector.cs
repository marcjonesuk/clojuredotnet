using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.Transpiler
{
	public class Vector : IEnumerable<object>, IList<object>
	{
		public List<object> Items { get; set; }

		public Vector(object[] items)
		{
			Items = new List<object>(items);
		}

		public override string ToString()
		{
			return "[" + string.Join(" ", Items.Select(i => RT.Str(i, true))) + "]";
		}

		public IEnumerator<object> GetEnumerator()
		{
			return ((IEnumerable<object>)Items).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Items).GetEnumerator();
		}

		public int IndexOf(object item)
		{
			return ((IList<object>)Items).IndexOf(item);
		}

		public void Insert(int index, object item)
		{
			((IList<object>)Items).Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			((IList<object>)Items).RemoveAt(index);
		}

		public void Add(object item)
		{
			((ICollection<object>)Items).Add(item);
		}

		public void Clear()
		{
			((ICollection<object>)Items).Clear();
		}

		public bool Contains(object item)
		{
			return ((ICollection<object>)Items).Contains(item);
		}

		public void CopyTo(object[] array, int arrayIndex)
		{
			((ICollection<object>)Items).CopyTo(array, arrayIndex);
		}

		public bool Remove(object item)
		{
			return ((ICollection<object>)Items).Remove(item);
		}
		
		public int Count => ((ICollection<object>)Items).Count;

		public bool IsReadOnly => ((ICollection<object>)Items).IsReadOnly;

		public object this[int index] { get => ((IList<object>)Items)[index]; set => ((IList<object>)Items)[index] = value; }
	}
}