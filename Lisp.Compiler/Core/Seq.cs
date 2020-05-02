using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public static class Seq
	{
		public static IEnumerable<object> AsSeqOLD(this object o)
		{
			if (o == null) return new object[] { };
			if (o is IDictionary<object, object> dict) return dict.Select(kvp => (object)ImmutableArray.Create(kvp.Key, kvp.Value));
			if (o is string s) return s.Select(item => (object)item.ToString());
			return o.As<IEnumerable<object>>().Select(i => i);
		}
		
		// public static IAsyncEnumerable<object> AsSeq(this object o)
		// {
		// 	if (o == null) return new object[] { }.ToAsyncEnumerable();
		// 	if (o is IDictionary<object, object> dict) return dict.Select(kvp => (object)ImmutableArray.Create(kvp.Key, kvp.Value));
		// 	if (o is string s) return s.Select(item => (object)item.ToString());
		// 	return o.As<IAsyncEnumerable<object>>().Select(i => i);
		// }

		public static object Seq_(object coll) => coll.AsSeqOLD();
		public static object Count(object o) => o.AsSeqOLD().Count();
		public static object First(object o) => o.AsSeqOLD().FirstOrDefault();
		public static object Next(object o) => o.AsSeqOLD().Skip(1).ReturnNilIfCompleted();
		public static object Drop(object count, object coll) => coll.AsSeqOLD().Skip(count.As<int>());
		public static object Take(object count, object coll) => coll.AsSeqOLD().Take(count.As<int>());
		public static object Last(object coll) => coll.AsSeqOLD().LastOrDefault();
		public static object Distinct(object coll) => coll.AsSeqOLD().Distinct();
		public static object Concat(object coll, object second) => coll.AsSeqOLD().Concat(second.AsSeqOLD());
		public static object Repeat(object value) => Enumerable.Repeat(value, int.MaxValue);
		public static object Repeat(object count, object value) => Enumerable.Repeat(value, count.As<int>());
		public static object Range() => Enumerable.Range(0, int.MaxValue).Cast<object>();
		public static object Range(object end) => Enumerable.Range(0, end.As<int>()).Cast<object>();
		public static object Range(object start, object end)
			=> Enumerable.Range(start.As<int>(), end.As<int>() - start.As<int>()).Cast<object>();
		public static object Range(object start, object end, object nth)
			=> Enumerable.Range(start.As<int>(), end.As<int>() - start.As<int>()).TakeEvery(nth.As<int>()).Cast<object>();

		public static IEnumerable<object> ReturnNilIfCompleted(this IEnumerable<object> seq)
		{
			if (!seq.Any()) return null;
			return seq;
		}

		public static IEnumerable<T> TakeEvery<T>(this IEnumerable<T> sequence, int every)
		{
			return sequence.TakeEvery(every, 0);
		}

		public static IEnumerable<T> TakeEvery<T>(
			this IEnumerable<T> sequence, int every, int skipInitial)
		{
			if (sequence == null) throw new ArgumentNullException("sequence");
			if (every < 1) throw new ArgumentException("'every' must be 1 or greater");
			if (skipInitial < 0)
				throw new ArgumentException("'skipInitial' must be 0 or greater");

			return TakeEveryImpl(sequence, every, skipInitial);
		}

		private static IEnumerable<T> TakeEveryImpl<T>(
			IEnumerable<T> sequence, int every, int toSkip)
		{
			var enumerator = sequence.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (toSkip == 0)
				{
					yield return enumerator.Current;
					toSkip = every;
				}
				toSkip--;
			}
		}
	}
}