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
		public delegate object Fn(object[] args);


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

			Func<Fn, object, object> eval1 = (m, a) => m(new object[1] { a });
			Func<Fn, object, object, object> eval2 = (m, a, b) => m(new object[2] { a, b });
			Func<Fn, object, object, object, object> eval3 = (m, a, b, c) => m(new object[2] { a, b });
			Func<object, Fn> wrap = (fn) => (Fn)(_ => fn);
			var add = (Fn)(args => (dynamic)args[0] + (dynamic)args[1]);
			var log = (Fn)(args => { Console.WriteLine((dynamic)args[0]); return null; });
			var if_ = (Fn)(args =>
			{
				if ((bool)args[0])
					return ((Fn)args[1])(null);
				else
					return ((Fn)args[2])(null);
			});

			while (true)
			{
				sw.Reset();
				fn.Invoke();
				sw.Start();
				for (var i = 0; i < 1000000; i++)
				{
					// fn.Invoke();
					// runner(null);
					// x += (int)f(new object[] { 10,20 });
					var add2 = ((Fn)((args) => { var x = (dynamic)args[0]; var y = (dynamic)args[1]; return eval2(add, x, y); }));
					eval2(add2, 10, 20);
				}
				sw.Stop();
				Console.WriteLine(sw.ElapsedMilliseconds);
			}

		}
	}
}
