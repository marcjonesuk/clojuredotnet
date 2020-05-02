using System;
using System.Threading.Tasks;

namespace Lisp.Compiler
{
	public sealed class Function : IFn
	{
		public Func<object[], Task<object>> fn;

		public Function()
		{

		}

		public Function(Func<object[], Task<object>> fn, string name = null)
		{
			this.fn = fn;
			Name = name;
		}

		public string Name { get; }

		public object Invoke()
		{
			return fn(null);
		}

		public async Task<object> Invoke(object[] args)
		{
			try
			{
				return await fn(args);
			}
			catch (Exception e)
			{
				throw e.Wrap(this);
			}
		}

		public override string ToString() => $"{Name}";
	}
}