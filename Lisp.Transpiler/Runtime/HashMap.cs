using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Lisp.Transpiler
{
	// * Immutable * hashmap implementation
	public class HashMap : IEnumerable<object>, IDictionary<object, object>
	{
		public ImmutableDictionary<object, object> Items { get; }
		public ICollection<object> Keys => ((IDictionary<object, object>)Items).Keys;
		public ICollection<object> Values => ((IDictionary<object, object>)Items).Values;
		public int Count => ((ICollection<KeyValuePair<object, object>>)Items).Count;
		public bool IsReadOnly => ((ICollection<KeyValuePair<object, object>>)Items).IsReadOnly;

		public object this[object key]
		{
			get
			{
				if (Items.TryGetValue(key, out var value))
					return value;
				return null;
			}
			set
			{
				throw new NotImplementedException("HashMap.set");
			}
		}

		public HashMap(ImmutableDictionary<object, object> items)
		{
			Items = items;
		}

		public HashMap(object[] items)
		{
			var temp = ImmutableDictionary<object, object>.Empty;
			for (var i = 0; i < items.Length; i += 2)
				temp = temp.SetItem(items[i], items[i + 1]);
			Items = temp;
		}

		public HashMap SetItem(object key, object value)
		{
			return new HashMap(Items.SetItem(key, value));
		}

		public override string ToString()
		{
			return "{" + string.Join(" ", Items.Select(i => RT.Str(i.Key, true) + " " + RT.Str(i.Value, true))) + "}";
		}

		public IEnumerator<object> GetEnumerator()
		{
			return Items.Select(kvp => new Vector(new object[] { kvp.Key, kvp.Value })).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Items.Select(kvp => new Vector(new object[] { kvp.Key, kvp.Value })).GetEnumerator();
		}

		public void Add(object key, object value)
		{
			((IDictionary<object, object>)Items).Add(key, value);
		}

		public bool ContainsKey(object key)
		{
			return ((IDictionary<object, object>)Items).ContainsKey(key);
		}

		public bool Remove(object key)
		{
			return ((IDictionary<object, object>)Items).Remove(key);
		}

		public bool TryGetValue(object key, [MaybeNullWhen(false)] out object value)
		{
			return ((IDictionary<object, object>)Items).TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<object, object> item)
		{
			((ICollection<KeyValuePair<object, object>>)Items).Add(item);
		}

		public void Clear()
		{
			((ICollection<KeyValuePair<object, object>>)Items).Clear();
		}

		public bool Contains(KeyValuePair<object, object> item)
		{
			return ((ICollection<KeyValuePair<object, object>>)Items).Contains(item);
		}

		public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<object, object>>)Items).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<object, object> item)
		{
			return ((ICollection<KeyValuePair<object, object>>)Items).Remove(item);
		}

		IEnumerator<KeyValuePair<object, object>> IEnumerable<KeyValuePair<object, object>>.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, object>>)Items).GetEnumerator();
		}
	}
}