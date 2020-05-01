using System;
using Mono.Terminal;
using Lisp.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.REPL
{
	class Program
	{
		static void Main(string[] args)
		{
			// Creates a line editor, and sets the name of the editing session to
			// "foo".  This is used to save the history of input entered
			LineEditor le = new LineEditor("foo")
			{
				HeuristicsMode = "csharp"
			};

			// Configures auto-completion, in this example, the result
			// is always to offer the numbers as completions
			le.AutoCompleteEvent += delegate (string text, int pos)
			{
				string prefix = "";
				var completions = new string[] {
				"first", "reduce", "map" };
				return new Mono.Terminal.LineEditor.Completion(prefix, completions);
			};

			string s;

			// Prompts the user for input
			while ((s = le.Edit("> ", "")) != null)
			{
				if (s== "exit") Environment.Exit(0);
				var compiler = new Lisp.Compiler.Compiler();
				try {
					if (s != string.Empty)
					Console.WriteLine(compiler.Compile(s).Invoke().Stringify(true));
				}
				catch(Exception e) {
					Console.WriteLine(e);
				}
			}
		}
	}
}
