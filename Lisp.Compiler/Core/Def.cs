using System;
using System.Threading.Tasks;

namespace Lisp.Compiler
{
	public class Def : IFn
	{
		public async Task<object> Invoke(object[] args)
		{
			var symbol = ((Symbol)args[0]).Name;
			var value = await args[1].Eval();
			State.Root[symbol] = value;
			return Task.FromResult<object>(null);
		}
	}
}