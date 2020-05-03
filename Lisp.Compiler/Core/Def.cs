using System;

namespace Lisp.Compiler
{
	public class Def : IFn
	{
		public object Invoke(object[] args)
		{
			var symbol = ((Symbol)args[0]).Name;
			var value = args[1].Eval();
			Environment.Root[symbol] = value;
			return null;
		}
	}
}