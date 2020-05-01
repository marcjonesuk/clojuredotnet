using System;
using System.IO;
using Lisp.Compiler;

namespace Lisp.RunTime
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("±±±± MARC LANG ±±±±");
			try {
				var code = File.ReadAllText(args[0]);
				var result = new Lisp.Compiler.Compiler().Compile(code).Invoke();
			}
			catch(Exception e){
				Console.WriteLine(e);
			}
        }
    }
}
