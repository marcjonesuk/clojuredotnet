using System;

namespace Lisp.Compiler
{
	public sealed class Function : IFn
	{
		public Func<object[], object> fn;

		public Function()
		{

		}

		public Function(Func<object[], object> fn, string name = null)
		{
			this.fn = fn;
			Name = name;
		}

		public string Name { get; }

		public object Invoke()
		{
			return fn(null);
		}

		public object Invoke(object[] args)
		{
			try
			{
				return fn(args);
			}
			catch (Exception e)
			{
				throw e.Wrap(this);
			}
		}

		public override string ToString() => $"{Name}";
	}
}