using System.Collections.Generic;

namespace Lisp.Compiler
{
	public class Reduce : IFn
	{
		public object Invoke(object[] args)
		{
			object value = null;
			var function = args[0];
			IEnumerable<object> target = null;

			if (args.Length < 2 || args.Length > 3)
				throw new ArityException(args.Length);

			if (args.Length == 2)
			{
				target = (IEnumerable<object>)args[1];
			}
			else if (args.Length == 3)
			{
				value = args[1];
				target = (IEnumerable<object>)args[2];
			}

			bool hasItems = false;
			foreach (var item in target)
			{
				hasItems = true;
				if (value == null)
				{
					value = item;
					continue;
				}
				value = function.Eval(new object[] { value, item });
			}
			if (!hasItems)
			{
				if (args.Length == 3)
					return function.Eval(new object[] { value });
				else
					return function.Eval();
			}
			return value;
		}
	}
}