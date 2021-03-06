// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Collections.Immutable;
// using System.Collections;

// namespace Lisp.Compiler
// {
// 	public interface ISyntaxItem
// 	{

// 	}

// 	public class StringConstant : ISyntaxItem 
// 	{
// 		public string Value;
// 	}

// 	public class IntConstant : ISyntaxItem 
// 	{
// 		public int Value;
// 	}
	
// 	public class DoubleConstant : ISyntaxItem 
// 	{
// 		public double Value;
// 	}
	
// 	public class SyntaxList : IEnumerable<ISyntaxItem>, ISyntaxItem
// 	{
// 		public char? Modifier { get; }
// 		public string Delimiters { get; }
// 		public object Value { get; }
// 		public string Source { get; }
// 		public List<ISyntaxItem> Items;

// 		public SyntaxList(char? modifier, string delimiters, List<ISyntaxItem> items)
// 		{
// 			Modifier = modifier;
// 			Delimiters = Delimiters;
// 			Items = items;
// 		}

// 		public IEnumerator<ISyntaxItem> GetEnumerator()
// 		{
// 			return ((IEnumerable<ISyntaxItem>)Items).GetEnumerator();
// 		}

// 		IEnumerator IEnumerable.GetEnumerator()
// 		{
// 			return ((IEnumerable)Items).GetEnumerator();
// 		}
// 	}

// 	public class SynItem
// 	{
// 	}

// 	public class Reader2
// 	{
// 		public string Load(string file) => File.ReadAllText(file).Replace("'", "`").Replace("\"", "'");

// 		public IEnumerable<object> Read(string code) => Read(new Tokeniser().Tokenise(code));

// 		private IEnumerable<object> Read(TokenEnumerator en)
// 		{
// 			var statements = ImmutableList<object>.Empty;
// 			while (!en.IsEof)
// 			{
// 				statements = statements.Add(ReadNext(en));
// 			}
// 			return statements;
// 		}

// 		private ISyntaxItem ReadNext(TokenEnumerator en)
// 		{
// 			var t = en.Current;
// 			switch (t)
// 			{
// 				// case "`":
// 				// 	en.MoveNext();
// 				// 	return new Quoted(ReadNext(en));
// 				// case "~":
// 				// 	en.MoveNext();
// 				// 	return new Unquoted(ReadNext(en));
// 				case " ":
// 					en.MoveNext();
// 					return ReadNext(en);
// 				case "(":
// 					return ReadCollection(en, ")", (items) => new SyntaxList(null, "()", items));
// 				case "[":
// 					return ReadCollection(en, ")", (items) => new SyntaxList(null, "[]", items));
// 				case "{":
// 					return ReadCollection(en, "}", (items) => new SyntaxList(null, "{}", items));
// 				case "#":
// 					en.MoveNext();
// 					switch (en.Current)
// 					{
// 						case "(": return ReadAnonymousFunction(en);
// 						case "{": return ReadCollection(en, "}", (items) => items.ToImmutableHashSet(new CustomComparer()));
// 						default: throw new Exception();
// 					}
// 				case "&":
// 					return CompileVariadic(en);
// 				case "'":
// 					return ReadString(en);
// 				case ":":
// 					return ReadKeyword(en);
// 				case "true":
// 					en.MoveNext();
// 					return true;
// 				case "false":
// 					en.MoveNext();
// 					return false;
// 				case "nil":
// 					en.MoveNext();
// 					return null;
// 				default:
// 					if (Helper.IsNumeric(t))
// 					{
// 						return ReadNumber(en);
// 					}
// 					else
// 					{
// 						return ReadSymbol(en);
// 					}
// 			}
// 			throw new Exception();
// 		}

// 		private object CompileVariadic(TokenEnumerator en)
// 		{
// 			Assert("&", en);
// 			en.MoveNext();
// 			Assert(" ", en);
// 			en.MoveNext();
// 			var symbol = en.Current;
// 			en.MoveNext();
// 			return new Symbol(symbol, true);
// 		}

// 		// todo: doesnt support lists like 1,2,3,4
// 		private List<ISyntaxItem> ReadItems(TokenEnumerator en, string terminator)
// 		{
// 			var items = new List<ISyntaxItem>();
// 			if (en.Current != terminator)
// 			{
// 				items.Add(ReadNext(en));
// 				while (en.Current == " " || en.Current == ",")
// 				{
// 					en.MoveNext();
// 					if (en.Current != " " && en.Current != ",")
// 						items.Add(ReadNext(en));
// 				}
// 			}
// 			Assert(terminator, en);
// 			en.MoveNext();
// 			return items;
// 		}

// 		private T ReadCollection<T>(TokenEnumerator en, string terminator, Func<List<ISyntaxItem>, T> create)
// 		{
// 			en.MoveNext();
// 			var items = ReadItems(en, terminator);
// 			return create(items);
// 		}

// 		private object ReadKeyword(TokenEnumerator en)
// 		{
// 			Assert(":", en);
// 			en.MoveNext();
// 			var name = en.Current;
// 			en.MoveNext();
// 			return new Keyword(name);
// 		}

// 		private object ReadAnonymousFunction(TokenEnumerator en)
// 		{
// 			var list = ImmutableList.Create<object>(new Symbol("fn"));
// 			var fnBody = ReadCollection(en, ")", items => items.ToImmutableList());

// 			// Deep search for any anonymous argument usage, e.g. %, %1 
// 			var args = new List<Symbol>();
// 			void GetArgs(List<Symbol> args, object o)
// 			{
// 				if (o is Symbol sym && sym.Name.StartsWith("%"))
// 					args.Add(sym);

// 				if (o is IEnumerable<object> e)
// 					foreach (var item in e)
// 						GetArgs(args, item);
// 			}
// 			GetArgs(args, fnBody);

// 			list = list.Add(ImmutableArray<object>.Empty.AddRange(args));
// 			list = list.Add(fnBody);
// 			return list;
// 		}

// 		private Symbol ReadSymbol(TokenEnumerator en)
// 		{
// 			var symbol = en.Current;
// 			en.MoveNext();
// 			return new Symbol(symbol, false);
// 		}

// 		private object ReadNumber(TokenEnumerator en)
// 		{
// 			var n = en.Current;
// 			en.MoveNext();
// 			if (n.Contains("."))
// 				return new DoubleConstant() { Value = double.Parse(n) };
// 			else
// 				return new IntConstant() { Value = int.Parse(n) };
// 		}

// 		private string ReadString(TokenEnumerator en)
// 		{
// 			Assert("'", en);
// 			en.MoveNext();
// 			var str = en.Current;
// 			en.MoveNext();
// 			Assert("'", en);
// 			en.MoveNext();
// 			return str;
// 		}

// 		private void Assert(string expected, TokenEnumerator en)
// 		{
// 			if (expected != en.Current)
// 				throw new System.Exception($"Expected '{expected}' but got '{en.Current}'");
// 		}

// 		public override bool Equals(object obj)
// 		{
// 			return base.Equals(obj);
// 		}

// 		public override int GetHashCode()
// 		{
// 			return base.GetHashCode();
// 		}

// 		public override string ToString()
// 		{
// 			return base.ToString();
// 		}
// 	}
// }
