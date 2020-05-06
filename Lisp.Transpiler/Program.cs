using System;
using System.Collections.Generic;
using System.Text;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	class Program
	{
		static string Transpile(ReaderItem input)
		{
			StringBuilder output = new StringBuilder();
			if (input is IEnumerable<ReaderItem> enumerable)
			{
				foreach (var i in enumerable)
					output.Append(Transpile(i));
			}
			else {
				output.Append(input);
			}

			return output.ToString();
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");


		}
	}
}
