using System;

namespace Lisp.Reader
{
	public class ReaderLiteral : ReaderItem
	{
		public ReaderLiteral(object i, Token token) : base(token)
		{
			Value = i;
		}

		public object Value { get; }

		public override string ToString() => Value.ToString();
	}
}