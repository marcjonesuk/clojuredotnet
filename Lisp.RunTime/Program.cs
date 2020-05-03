using System;
using System.Diagnostics;
using System.IO;
using Lisp.Compiler;

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

        static void Benchmark()
        {

        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            // new Lisp.Compiler.Compiler().Compile("(defn x [a] (+ a 5))").Invoke();
            // var fn = new Lisp.Compiler.Compiler().Compile("(x 10)");
            var fn = new Lisp.Compiler.Compiler().Compile("(+ 10 20)");

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

            // Console.WriteLine("±±±± MARC LANG ±±±±");
            // try
            // {
            // 	var code = File.ReadAllText(args[0]);
            // 	var result = new Lisp.Compiler.Compiler().Compile(code).Invoke();
            // }
            // catch (Exception e)
            // {
            // 	Console.WriteLine(e);
            // }
        }
    }
}
