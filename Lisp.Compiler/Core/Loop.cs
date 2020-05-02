using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lisp.Compiler
{
	public class RecurSignal
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

		public async Task<object> Invoke(object[] args)
		{
			return new RecurSignal(args);
		}
	}

	public class Loop : IFn
	{
		public async Task<object> Invoke(object[] args)
		{
			var initialArguments = args[0].As<IList<object>>();
			var body = args[1];

			// Initial bindings
			for (var i = 0; i < initialArguments.Count; i += 2)
			{
				var key = (initialArguments[i] as Symbol).Name;
				var value = await initialArguments[i + 1].Eval();
				State.Current[key] = value;
			}

			object result = null;
			bool done = false;
			while (!done)
			{
				result = await body.Eval();
				if (result is RecurSignal recur)
				{
					var newArgs = recur.Args;
					for (var i = 0; i < initialArguments.Count; i += 2)
					{
						// Update bindings
						var key = (initialArguments[i] as Symbol).Name;
						State.Current[key] = newArgs[i / 2];
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