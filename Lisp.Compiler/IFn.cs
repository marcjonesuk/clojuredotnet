namespace Lisp.Compiler
{
	public interface IFn
	{
		object Invoke(object[] args);
		object Invoke() => Invoke(null);
	}
}