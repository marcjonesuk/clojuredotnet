using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisp.Compiler 
{
	public class Symbol : IFn, IStringify
	{
		private object _interop;
		public bool IsInterop {get; private set;}
		
		public Symbol(string name) : this(name, false) {}
		
		public Symbol(string name, bool isVariadic)
		{
			IsVariadic = isVariadic;
			Name = name;

			if (name.Contains("/")) IsInterop = true;
		}

		public string Name { get; }

		public Task<object> Invoke(object[] args)
		{
			if (!IsInterop)
				return Task.FromResult<object>(State.Current[Name]);

			if (_interop == null) 
				_interop = InteropCompiler.Create(Name);

			return Task.FromResult<object>(_interop);
		}
		
		public override string ToString() => (IsVariadic ? "& " : "") + Name;
		public bool IsVariadic { get; }

		public static Symbol Read(TokenEnumerator en)
		{
			var symbol = en.Current;
			en.MoveNext();
			return new Symbol(symbol, false);
		}

		public override bool Equals(object obj)
		{
			return obj is Symbol symbol &&
				   Name == symbol.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name);
		}

		public static Symbol Create(string v)
		{
			return new Symbol(v);
		}

		public string Stringify(bool quoteStrings) => ToString();
	}
}