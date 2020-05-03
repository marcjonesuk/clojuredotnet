using System;
using System.Collections;
using System.Collections.Generic;

namespace Lisp.Compiler
{
    public class EndOfFileException : Exception
    {
        public EndOfFileException(string expected) : base($"Found end of file, expected {expected}")
        {
        }
    }

    public class TokenEnumerator : IEnumerator<string>
    {
        private readonly string[] tokens;
        private int position = 0;

        public TokenEnumerator(string[] tokens)
        {
            this.tokens = tokens;
        }

        internal string[] ToArray()
        {
            throw new NotImplementedException();
        }

        public string Current => tokens[position];
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

        public (string, bool) Next()
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

        public string Peek()
        {
            return tokens[position + 1];
        }
    }
}
