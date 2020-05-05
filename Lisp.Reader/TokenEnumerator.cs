using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.Reader
{
	public class EndOfFileException : Exception
	{
		public EndOfFileException(string expected) : base($"Found end of file, expected {expected}")
		{
		}
	}

	public class TokenEnumerator : IEnumerator<Token>
	{
		private readonly Token[] tokens;
		private int position = 0;

		public TokenEnumerator(Token[] tokens)
		{
			this.tokens = tokens;
		}

		internal string[] ToArray()
		{
			throw new NotImplementedException();
		}

		public Token Current
		{
			get
			{
				return tokens[position];
			}
		}

		public Token Previous
		{
			get
			{
				return tokens[position - 1];
			}
		}

		public List<Token> Tokens => tokens.ToList();

		object IEnumerator.Current => tokens[position];

		public void Dispose()
		{
		}

		public bool MoveNext(string expected = "")
		{
			if (IsEof) throw new EndOfFileException(expected);
			position++;
			return IsEof;
		}

		public (Token, bool) Next()
		{
			position++;
			return (tokens[position], IsEof);
		}

		public bool IsEof => position >= tokens.Length;

		public int Count => tokens.Length;

		public void Reset()
		{
			position = 0;
		}

		public bool MoveNext() => MoveNext(null);

		public Token Peek()
		{
			return tokens[position + 1];
		}
	}
}
