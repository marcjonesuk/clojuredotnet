using System.Collections.Generic;
using System.Linq;

namespace Lisp.Compiler
{
	public struct RecurSignal
	{
		public RecurSignal(object[] args)
		{
			Args = args;
		}

		public object[] Args { get; }
	}

	public class Recur : IFn
	{
		public object Invoke()
		{
			throw new System.Exception();
		}

		public object Invoke(object[] args)
		{
			return new RecurSignal(args);
		}
	}

	public class Loop : IFn
	{
		public object Invoke(object[] args)
		{
			var initialArguments = args[0].As<IList<object>>();
			var body = args[1];

			// Initial bindings
			for (var i = 0; i < initialArguments.Count; i += 2)
			{
				var key = (initialArguments[i] as Symbol).Name;
				var value = initialArguments[i + 1].Eval();
				Environment.Current[key] = value;
			}

			object result = null;
			bool done = false;
			while (!done)
			{
				result = body.Eval();
				if (result is RecurSignal recur)
				{
					var newArgs = recur.Args;
					for (var i = 0; i < initialArguments.Count; i += 2)
					{
						// Update bindings
						var key = (initialArguments[i] as Symbol).Name;
						Environment.Current[key] = newArgs[i / 2];
					}
				}
				else
				{
					done = true;
				}
			}
			return result;
		}
	}
}