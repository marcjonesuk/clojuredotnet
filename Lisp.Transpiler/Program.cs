using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Lisp.Reader;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Lisp.Transpiler
{
	public class Globals
	{
	}

	public static class Compiler
	{
		// public DefExpression 

		public static string Compile(this ReaderItem item)
		{
			var tree = item.BuildExpressionTree();
			return tree.Transpile();
		}

		public static string Compile(this List<ReaderItem> items)
		{
			string output = "";
			foreach (var item in items)
			{
				output += item.BuildExpressionTree().Transpile() + ";\n";
			}

			var scriptOptions = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
				.WithReferences(Assembly.GetCallingAssembly())
				.WithImports("Lisp.Transpiler");
			var script = CSharpScript.Create<object>(output, scriptOptions, globalsType: typeof(Globals) );
			
			script.Compile();

			// var result2 = CSharpScript.EvaluateAsync("Directory.GetCurrentDirectory()", 
			// 	Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default.WithImports("Lisp.Transpiler"));
			// var result2 = CSharpScript.EvaluateAsync<object>().Result;

			Console.WriteLine(output);
			Console.WriteLine("=====================================================================================");
			var result = script.RunAsync(new Globals()).Result;
			// Console.WriteLine(result);
			return "";
		}

		public static IExpression BuildExpressionTree(this ReaderItem item)
		{
			var expressions = new List<IExpression>() {
				new IfExpression(),
				new DefnExpression(),
				new FnExpression(),
				new LiteralExpression(),
				new SymbolExpression(),
				new VectorExpression(),
				new LetExpression(),
				new DefExpression(),
				new LambdaExpression() };

			foreach (var expr in expressions)
			{
				var result = expr.Create(item);
				if (result != null) return result;
			}
			throw new NotImplementedException(item.GetType().ToString());
		}
	}

	public delegate object Fn(object[] args);

	class Program
	{
		static void Main(string[] args)
		{
			var reader = new Rdr();
			Func<object, Fn> wrap = (fn) => (Fn)(_ => fn);
			
			var if_ = (Fn)(args =>
			{
				if ((bool)args[0])
					return ((Fn)args[1])(null);
				else
					return ((Fn)args[2])(null);
			});

			var core = @"
(defn print [v] (RT.Print (RT.Str v)))
(defn str [x] (RT.Str x))
(defn + [x y] (RT.Add x y))
(defn = [x y] (RT.Eq x y))
(defn > [x y] (RT.Gt x y))
(defn < [x y] (RT.Lt x y))
(defn take [coll count] (System.Linq.Enumerable.Take (RT.Seq coll) count))
(defn first [coll] (System.Linq.Enumerable.FirstOrDefault (RT.Seq coll)))
(defn conj [coll item] (RT.Conj coll item))
(defn get [coll index] (RT.Get coll index))

";
			if (args.Length > 0) {
				var code = File.ReadAllText(args[0]);
				core = core + code;
			}

			var read = reader.Read(core).ToList();
			Console.WriteLine(read.Compile());
		}
	}
}