namespace Lisp.Compiler
{
	public interface IExpression
	{
		IExpression Parent { get; set; }
	}
}