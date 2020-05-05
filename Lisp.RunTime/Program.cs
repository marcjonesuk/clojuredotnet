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
			Func<object[], object> arg1 = (object[] args) => 10;
			Func<object[], object> arg2 = (object[] args) => 20;

			Func<object[], object> add = (object[] args) => (dynamic)args[0] + (dynamic)args[1];


			Func<object[], object> addGetter = (object[] args) => add;

			Func<object[], object> sex = (object[] args) => addGetter(new object[] { arg1(null), arg2(null) });

			List<Func<object[], object>> program = new List<Func<object[], object>>();
			program.Add(sex);

			Func<object[], object> runner = (object[] args) => {
				foreach(var item in program)
					item(null);
				return null;
			};

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
					// fn.Invoke();
					runner(null);
					// x += (int)f(new object[] { 10,20 });
				}
				sw.Stop();
				Console.WriteLine(sw.ElapsedMilliseconds);
			}

		}
	}
}
