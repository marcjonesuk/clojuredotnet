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

			var core = @"
(defn print [v] (RT.Print (RT.Str v)))
(defn str [x] (RT.Str x))
(defn + [x y] (RT.Add x y))
(defn * [x y] (RT.Mult x y))
(defn = [x y] (RT.Eq x y))
(defn > [x y] (RT.Gt x y))
(defn < [x y] (RT.Lt x y))
(defn inc [x] (RT.Inc x))
(defn dec [x] (RT.Dec x))
(defn take [coll count] (System.Linq.Enumerable.Take (RT.Seq coll) count))
(defn first [coll] (System.Linq.Enumerable.FirstOrDefault (RT.Seq coll)))
(defn conj [coll item] (RT.Conj coll item))
(defn get [coll index] (RT.Get coll index))
(defn reduce [fn values] (RT.Reduce fn values))
; (defn apply [fn values] (RT.Apply fn values))
";
			if (args.Length > 0) {
				var code = File.ReadAllText(args[0]);
				core = core + code;
			}

			Console.WriteLine("Parsing...");
			var read = reader.Read(core).ToList();
			Console.WriteLine("Building expression tree...");
			Console.WriteLine(read.Compile());
		}
	}
}