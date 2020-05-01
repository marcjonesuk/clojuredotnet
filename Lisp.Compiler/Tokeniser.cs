using System;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.Compiler 
{

	// TODO
	// Support strings with tabs etc.

	public class Tokeniser
	{
		public TokenEnumerator Tokenise(string code)
		{
			code = code.Replace("\t", "").Replace("\n", "").Replace("\r", "").Trim();
				
			var output = new List<string>();
			var done = false;
			var pos = 0;
			var buffer = string.Empty;

			while (!done)
			{
				var current = code[pos];

				switch (current)
				{
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
							output.Add(buffer);
							buffer = string.Empty;
						}

						// Filter out adjacent white space
						if (current == ' ' && output.Last() == " ")
							break;

						if (current != ',')
							output.Add(current.ToString());

						break;
					case '\'':
						var s = string.Empty;
						pos++;
						while(code[pos] != '\'') {

							s += code[pos];
							pos++;							
						}
						output.Add("'");
						output.Add(s);
						output.Add("'");
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
						output.Add(buffer);
						buffer = string.Empty;
					}
					done = true;
				}
			}
			return new TokenEnumerator(output.ToArray());
		}
	}
}
