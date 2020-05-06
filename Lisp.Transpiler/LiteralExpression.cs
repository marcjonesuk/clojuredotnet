using System;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class LiteralExpression : IExpression
	{
		public ReaderItem Source => throw new NotImplementedException();

		public object Value { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderLiteral literal)
				return new LiteralExpression(literal.Value);
			return null;
		}

		public LiteralExpression()
		{
		}

		public LiteralExpression(object value)
		{
			Value = value;
		}

		public string Transpile()
		{
			return Value switch {
				bool b => b.ToString(),
				string s => $"\"{s}\"",
				object o => o.ToString(),
				_ => throw new Exception()
			};
		}
	}
}