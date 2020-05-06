using Lisp.Reader;

namespace Lisp.Transpiler
{
	public interface IExpression
	{
		ReaderItem Source { get; }
		IExpression Create(ReaderItem item);
		string Transpile();
		string[] Locals => new string[0];
	}
}