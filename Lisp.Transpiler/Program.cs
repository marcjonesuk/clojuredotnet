using System;
using System.IO;
using System.Linq;
using System.Text;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class Globals
	{
	}

	public delegate object Fn(params object[] args);
	public delegate object Iife();

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Starting");
			var reader = new Rdr();

			var code = string.Empty;
			if (args.Length > 0)
			{
				code = File.ReadAllText(args[0]);
				code = Transpiler.Core + code;
			}

			Console.WriteLine("Parsing...");
			var read = reader.Read(code).ToList();
			Console.WriteLine("Building expression tree...");
			Console.WriteLine(read.Execute());
		}
	}
}