namespace Lisp.Compiler
{
	// TODO
	// Support strings with tabs etc.

	public class Token
	{
		public string Value { get; }
		public int Line { get; }
		public int Column { get; }

		public Token(string value, int line, int column)
		{
			Value = value;
			Line = line;
			Column = column;
		}

		public string Position => $"line {Line} column {Column}";

		public override string ToString()
		{
			return $"'{Value}' at line {Line} column {Column}";
		}
	}
}
