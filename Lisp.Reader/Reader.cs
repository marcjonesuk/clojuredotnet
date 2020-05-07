using System;
using System.Collections.Generic;
using System.IO;

namespace Lisp.Reader
{
	public class Rdr
	{
		public string Load(string file) => File.ReadAllText(file).Replace("'", "`").Replace("\"", "'");

		public IEnumerable<ReaderItem> Read(string code) => Read(new Tokeniser().Tokenise(code));

		private IEnumerable<ReaderItem> Read(TokenEnumerator en)
		{
			var statements = new List<ReaderItem>();
			while (!en.IsEof)
			{
				statements.Add(ReadNext(en));
			}
			return statements;
		}

		private ReaderItem ReadNext(TokenEnumerator en)
		{
			var t = en.Current.Value;
			switch (t)
			{
				// case "`":
				// 	en.MoveNext();
				// 	return new Quoted(ReadNext(en));
				// case "~":
				// 	en.MoveNext();
				// 	return new Unquoted(ReadNext(en));
				case " ":
				case "\r":
					en.MoveNext();
					// if (en.IsEof)
					// 	return null;
					return ReadNext(en);
				case "(":
					return ReadCollection(en, ")", (items) => new ReaderList(items));
				case "[":
					return ReadCollection(en, "]", (items) => new ReaderVector(items));
				// case "{":
				// 	return ReadCollection(en, "}", (items) => items.ToHashMap());
				// case "#":
				// 	en.MoveNext();
				// 	switch (en.Current.Value)
				// 	{
				// 		case "(": return ReadAnonymousFunction(en);
				// 		case "{": return ReadCollection(en, "}", (items) => items.ToImmutableHashSet(new CustomComparer()));
				// 		default: throw new Exception();
				// 	}
				// case "&":
				// 	return CompileVariadic(en);
				case "'":
					return ReadString(en);
				// case ":":
				// 	return ReadKeyword(en);
				case "true":
				 	var token = en.Current;
					en.MoveNext();
					return new ReaderLiteral(true, token);
				case "false":
				 	var tokenFalse = en.Current;
					en.MoveNext();
					return new ReaderLiteral(false, tokenFalse);
				case "nil":
					var tokenNil = en.Current;
					en.MoveNext();
					return new ReaderLiteral(null, tokenNil);
				default:
					if (Helper.IsNumeric(t))
					{
						return ReadNumber(en);
					}
					else
					{
						return ReadSymbol(en);
					}
			}
			throw new Exception();
		}

		private object CompileVariadic(TokenEnumerator en)
		{
			Assert("&", en);
			en.MoveNext();
			Assert(" ", en);
			en.MoveNext();
			var symbol = en.Current;
			en.MoveNext();
			return new ReaderSymbol(symbol.Value, symbol);
		}

		// todo: doesnt support lists like 1,2,3,4
		private IList<ReaderItem> ReadItems(TokenEnumerator en, string terminator)
		{
			var items = new List<ReaderItem>();
			try
			{
				if (en.Current.Value != terminator)
				{
					items.Add(ReadNext(en));
					while (en.Current.Value == " " || en.Current.Value == ",")
					{
						en.MoveNext();
						if (en.Current.Value == terminator)
							break;
						
						if (en.Current.Value != " " && en.Current.Value != ",")
							items.Add(ReadNext(en));
					}
				}
				Assert(terminator, en);
				en.MoveNext();
				return items;
			}
			catch (Exception e)
			{
				throw new Exception($"Expected more items {items} @ " + en.Previous, e);
			}
		}

		private T ReadCollection<T>(TokenEnumerator en, string terminator, Func<IEnumerable<ReaderItem>, T> create)
		{
			en.MoveNext();
			var items = ReadItems(en, terminator);
			return create(items);
		}

		// private object ReadKeyword(TokenEnumerator en)
		// {
		// 	Assert(":", en);
		// 	en.MoveNext();
		// 	var name = en.Current.Value;
		// 	en.MoveNext();
		// 	return new Keyword(name);
		// }

		// private object ReadAnonymousFunction(TokenEnumerator en)
		// {
		// 	var list = ImmutableList.Create<object>(new Symbol("fn", false, en.Current));
		// 	var fnBody = ReadCollection(en, ")", items => items.ToImmutableList());

		// 	// Deep search for any anonymous argument usage, e.g. %, %1 
		// 	var args = new List<Symbol>();
		// 	void GetArgs(List<Symbol> args, object o)
		// 	{
		// 		if (o is Symbol sym && sym.Name.StartsWith("%"))
		// 			args.Add(sym);

		// 		if (o is IEnumerable<object> e)
		// 			foreach (var item in e)
		// 				GetArgs(args, item);
		// 	}
		// 	GetArgs(args, fnBody);

		// 	list = list.Add(ImmutableArray<object>.Empty.AddRange(args));
		// 	list = list.Add(fnBody);
		// 	return list;
		// }

		private ReaderSymbol ReadSymbol(TokenEnumerator en)
		{
			var symbol = en.Current;
			en.MoveNext();
			return new ReaderSymbol(symbol.Value, symbol);
		}

		private ReaderItem ReadNumber(TokenEnumerator en)
		{
			var n = en.Current;
			en.MoveNext();
			if (n.Value.Contains("."))
				return new ReaderLiteral(double.Parse(n.Value), n);
			else
				return new ReaderLiteral(int.Parse(n.Value), n);
		}

		private ReaderItem ReadString(TokenEnumerator en)
		{
			Assert("'", en);
			en.MoveNext();
			var str = en.Current;
			en.MoveNext();
			Assert("'", en);
			en.MoveNext();
			return new ReaderLiteral(str.Value, str);
		}

		private void Assert(string expected, TokenEnumerator en)
		{
			if (expected != en.Current.Value)
				throw new System.Exception($"Expected '{expected}' but got '{en.Current}'");
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
