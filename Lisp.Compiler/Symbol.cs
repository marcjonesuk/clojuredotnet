using System;
using System.Collections.Generic;

namespace Lisp.Compiler
{
	public class CallStack
	{
		public static object[] Stack = new object[512];
		private static int position = 0;
		public static int Frame = 0;
		private static int lastFrame = 0;

		public static void PushFrame(object[] values)
		{
			lastFrame = Frame;
			Frame = position;
			for(var i = 0; i < values.Length; i++) {
				Stack[position] = values[i];
				position++;
			}
		}

		public static void PopFrame(int size) {
			position -= size;
			Frame = lastFrame;
		}
	}

	public class Symbol : IFn, IStringify
	{
		private Token Token { get; }
		private object _interop;
		public bool IsInterop { get; private set; }
		public string Name { get; }

		public int LocalArgumentIndex {get;set;}

		public Symbol(string name, bool isVariadic, Token token)
		{
			IsVariadic = isVariadic;
			Token = token;
			Name = name;
			LocalArgumentIndex = -1;

			if (name.Contains("/")) IsInterop = true;
		}

		public object Invoke(object[] args)
		{
			// if (LocalArgumentIndex != -1)
			// {
			// 	Console.WriteLine("using local");
			// 	return CallStack.Stack[CallStack.Frame + LocalArgumentIndex];
			// }

			if (!IsInterop)
			{
				try
				{
					return Environment.Current[Name];
				}
				catch
				{
					throw new System.Exception($"Unable to resolve symbol: {Name} in this context ({Token.Position})");
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