using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public static class TypeHelper
	{
		public static T As<T>(this object o)
		{
			if (o is T t) return t;
			return (T)o;
		}

		public static T As<T>(this object o, object caller, object defaultValue = null)
		{
			if (o is T t) return t;
			try 
			{
				return (T)o;
			}
			catch (Exception ex)
			{
				throw ex.Wrap(caller);
			}
		}

		// todo : tests, add duplicate handling etc.
		public static ImmutableDictionary<object, object> ToHashMap(this IEnumerable<object> items)
		{
			var hashmap = ImmutableDictionary.Create<object, object>(new CustomComparer());
			var list = items.ToList();
			for (var i = 0; i < list.Count; i += 2)
			{
				hashmap = hashmap.Add(list[i], list[i + 1]);
			}
			return hashmap;
		}
	}

	public static class StringifyExtensions
	{
		public static string Stringify(this object o, bool quoteStrings = false, bool nilIsEmpty = false)
		{
			if (o == null) return nilIsEmpty ? "" : "nil";
			if (o.Equals(true)) return "true";
			if (o.Equals(false)) return "false";

			var s = o switch
			{
				string strng => quoteStrings ? "'" + strng + "'" : strng,
				IStringify str => str.Stringify(quoteStrings),
				ImmutableDictionary<object, object> arr => "{" + string.Join(' ', arr.Select(item => item.Key.Stringify(quoteStrings) + " " + item.Value.Stringify(quoteStrings))) + "}",
				ImmutableHashSet<object> arr => "#{" + string.Join(' ', arr.Select(item => item.Stringify(quoteStrings))) + "}",
				object[] array => "[" + string.Join(' ', array.Select(item => item.Stringify(quoteStrings))) + "]",
				ImmutableArray<object> arr => "[" + string.Join(' ', arr.Select(item => item.Stringify(quoteStrings))) + "]",
				IEnumerable<object> list => "(" + string.Join(' ', list.Select(item => item.Stringify(quoteStrings))) + ")",
				_ => o.ToString()
			};
			return s;
		}
		// public static string Stringify(this object o)
		// {
		// 	if (o == null) return "nil";
		// 	if (o.Equals(true)) return "true";
		// 	if (o.Equals(false)) return "false";

		// 	var s = o switch
		// 	{
		// 		string strng => strng, 
		// 		IStringify str => str.Stringify(),
		// 		ImmutableDictionary<object, object> arr => "{" + string.Join(' ', arr.Select(item => item.Key.Stringify() + " " + item.Value.Stringify())) + "}",
		// 		ImmutableHashSet<object> arr => "#{" + string.Join(' ', arr.Select(item => item.Stringify())) + "}",
		// 		object[] array => "[" + string.Join(' ', array.Select(item => item.Stringify())) + "]",
		// 		ImmutableArray<object> arr => "[" + string.Join(' ', arr.Select(item => item.Stringify())) + "]",
		// 		IEnumerable<object> list => "(" + string.Join(' ', list.Select(item => item.Stringify())) + ")",
		// 		_ => o.ToString()
		// 	};
		// 	return s;
		// }
	}
}