using System;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.Compiler
{
	public class Tokeniser
	{
		public TokenEnumerator Tokenise(string code)
		{
			var lineNumber = 1;
			code = code.Replace("\t", " ");

			var output = new List<Token>();
			var done = false;
			var pos = 0;
			var priorLinePos = 0;
			var buffer = string.Empty;

			while (!done)
			{
				var current = code[pos];

				switch (current)
				{
					case ';':
						while (code[pos] != '\n') pos++;
						priorLinePos = pos + 1;
						lineNumber++;
						break;
					case '\n':
					case '[':
					case ']':
					case '(':
					case '{':
					case '}':
					case ')':
					case '#':
					case '~':
					case ',':
					case ':':
					case '`':
					case ' ':
						if (buffer != string.Empty)
						{
							output.Add(new Token(buffer, lineNumber, pos - priorLinePos));
							buffer = string.Empty;
						}

						if (current == '\n')
						{
							output.Add(new Token(" ", lineNumber, pos - priorLinePos));
							priorLinePos = pos + 1;
							lineNumber++;
						}
						else if (current != ',')
						{
							output.Add(new Token(current.ToString(), lineNumber, pos - priorLinePos));
						}

						break;
					case '\'':
						var s = string.Empty;
						pos++;
						while (code[pos] != '\'')
						{

							s += code[pos];
							pos++;
						}
						output.Add(new Token("'", lineNumber, pos - priorLinePos));
						output.Add(new Token(s, lineNumber, pos - priorLinePos));
						output.Add(new Token("'", lineNumber, pos - priorLinePos));
						break;
					// case '.':
					// 	if (output.Last() == "(")
					// 		output.Add(".");
					// 	else 
					// 		buffer += '.';
					// 	break;
					default:
						buffer += current;
						break;
				}
				pos++;
				if (pos == code.Length)
				{
					if (buffer != string.Empty)
					{
						output.Add(new Token(buffer, lineNumber, pos - priorLinePos - buffer.Length + 1));
						buffer = string.Empty;
					}
					done = true;
				}
			}

			// trim leading and ending whitespace
			var result = new List<Token>();

			int b = 0;
			for (b = 0; b < output.Count; b++)
			{
				if (output[b].Value != " ") break;
			}

			int e = output.Count - 1;
			for (e = output.Count - 1; e > 0; e--)
			{
				if (output[e].Value != " ") break;
			}
			e++;

			return new TokenEnumerator(output.ToArray()[b..e]);
		}
	}
}
