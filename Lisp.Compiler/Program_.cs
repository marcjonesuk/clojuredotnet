using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lisp.Compiler
{
	public class Program_ : List<object>, IFn, IStringify
	{
		public Program_(List<object> items) : base(items)
		{
		}

		public object Value { get; }

		public override string ToString() => string.Join(' ', this.Select(i => i.ToString()));

		public async Task<object> Invoke(object[] args)
		{
			object current = null;
			try
			{
				object result = null;
				foreach (object item in this) {
					current = item;
					result = await item.Eval();
				}
				return result;
			}
			catch (Exception e)
			{
				// Only wrap .NET exceptions here
				if (e is RuntimeException) throw;
				throw e.Wrap(current);
			}
		}

		public string Stringify(bool quoteStrings) => ToString();
	}
}