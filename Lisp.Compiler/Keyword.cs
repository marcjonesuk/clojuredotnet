namespace Lisp.Compiler
{
	public class Keyword
	{
		public Keyword(string name)
		{
			Name = name;
		}

		public string Name { get; }
		public override string ToString() => ":" + Name;
		public override bool Equals(object obj) => obj is Keyword keyword && Name == keyword.Name;
		public override int GetHashCode() => Name.GetHashCode();
	}
}