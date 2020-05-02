using System.Linq;
using System.Threading.Tasks;

namespace Lisp.Compiler
{
	public class If : IFn
	{
		public async Task<object> Invoke(object[] args)
		{
			return args.Length switch
			{
				2 => RT.IsTruthy(await args[0].Eval()) switch
				{
					true => await args[1].Eval(),
					false => null,
				},
				3 => RT.IsTruthy(await args[0].Eval()) switch
				{
					true => await args[1].Eval(),
					false => await args[2].Eval(),
				},
				_ => throw new ArityException(args.Length)
			};
		}
	}
}