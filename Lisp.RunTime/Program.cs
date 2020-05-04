using System;
using System.Diagnostics;
using System.IO;
using Lisp.Compiler;
using System.Linq;
using System.Collections.Generic;

namespace Lisp.RunTime
{
	class Program
	{
		public static object Add(dynamic x, dynamic y)
		{
			// if (x is int xi && y is int yi)
			// 	return xi + yi;
			// return (dynamic)x + (dynamic)y;
			return x + y;
		}

		static void Main(string[] args)
		{
			var test = new List<string>().Select(i => i);
			if (args.Length > 0)
			{
				try
				{
					var code = File.ReadAllText(args[0]).Replace("\"", "'");
					var result = new Lisp.Compiler.Compiler().Compile(code).Invoke();
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
				return;
			}

			Stopwatch sw = new Stopwatch();
			// new Lisp.Compiler.Compiler().Compile("(defn x [a] (+ a 5))").Invoke();
			// var fn = new Lisp.Compiler.Compiler().Compile("(x 10)");
			var compiler = new Lisp.Compiler.Compiler();
			compiler.Compile("(defn myfunc [a b] (+ a b))").Invoke();
			var fn = compiler.Compile("(+ 10 20)");

			while (true)
			{
				sw.Reset();
				fn.Invoke();
				sw.Start();
				var x = 0;
				Func<object[], object> f = args => Add(args[0], args[1]);
				for (var i = 0; i < 1000000; i++)
				{
					fn.Invoke();
					// x += (int)f(new object[] { 10,20 });
				}
				sw.Stop();
				Console.WriteLine(sw.ElapsedMilliseconds);
			}

		}
	}
}
