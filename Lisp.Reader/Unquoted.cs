namespace Lisp.Compiler
{
	public class Unquoted 
	{
		public object Value { get; }

		public Unquoted(object value)
		{
			Value = value;
		}
		
		public override string ToString() => "~" + Value.ToString();
	}
}
