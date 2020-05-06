using System;
using System.Collections.Generic;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class SymbolExpression : IExpression
	{
		public ReaderItem Source => throw new NotImplementedException();

		public string Name { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderSymbol symbol)
				return new SymbolExpression(symbol.Name);
			return null;
		}

		public SymbolExpression()
		{
		}

		public SymbolExpression(string symbol)
		{
			Name = symbol;
		}

		public string Transpile()
		{
			var symbolMap = new Dictionary<string, string>();
			symbolMap["if"] = "if_";
			symbolMap["="] = "eq";
			symbolMap["+"] = "add";
			symbolMap["*"] = "mult";
			symbolMap["-"] = "sub";
			symbolMap[">"] = "gt";
			symbolMap["<"] = "lt";

			var name = Name;
			foreach(var kvp in symbolMap)
				name = name.Replace(kvp.Key, kvp.Value);
			return name;
		}
	}
}