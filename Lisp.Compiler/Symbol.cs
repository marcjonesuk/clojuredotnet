using System;
using Lisp.Reader;

namespace Lisp.Compiler
{
	public class Symbol : IExpression, IFn, IStringify
	{
		private Token Token { get; }
		private object _interop;
		public bool IsInterop { get; private set; }
		public string Name { get; }
		public IExpression Parent { get; set; }

		public Symbol(string name, bool isVariadic, Token token)
		{
			IsVariadic = isVariadic;
			Token = token ?? new Token(null, 0, 0);
			Name = name;

			if (name.Contains("/")) IsInterop = true;
		}

		public object Invoke(object[] args)
		{
			if (!IsInterop)
			{
				try
				{
					return Environment.Current[Name];
				}
				catch
				{
					throw new System.Exception($"Unable to resolve symbol: {Name} in this context ({Token})");
				}
			}

			if (_interop == null)
				_interop = InteropCompiler.Create(Name);

			return _interop;
		}

		public override string ToString() => (IsVariadic ? "& " : "") + Name;
		public bool IsVariadic { get; }

		public override bool Equals(object obj)
		{
			return obj is Symbol symbol &&
				   Name == symbol.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name);
		}

		public string Stringify(bool quoteStrings) => ToString();
	}
}