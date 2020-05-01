namespace Lisp.Compiler 
{
	public interface IFn
	{
		object Invoke(object[] args);
		object Invoke() => Invoke(null);
		object ApplyTo(object[] args) {
			return null;
		}
	}
} 