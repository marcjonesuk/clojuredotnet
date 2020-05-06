namespace Lisp.Compiler
{
	public class LiteralExpression : IExpression, IFn
	{
		public object Value { get; }
		public IExpression Parent { get; set; }

		public LiteralExpression(object value)
		{
			Value = value;
		}

		public object Invoke(object[] args)
		{
			return Value;
		}
	}
}