using System.Threading.Tasks;

namespace Lisp.Compiler 
{
	public interface IFn
	{
		Task<object> Invoke(object[] args);
		Task<object> Invoke() => Invoke(null);
	}
} 