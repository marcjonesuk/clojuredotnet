namespace Lisp.Reader
{
	public class ReaderSymbol : ReaderItem
	{
		public ReaderSymbol(string name, Token token) : base(token)
		{
			Name = name;
		}

		public string Name { get; }
	}
}