namespace Lisp.Compiler
{
	public class Quoted
	{
		public object Value { get; }

		public Quoted(object value)
		{
			Value = value;
		}
	}
}
