using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lisp.Reader;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Lisp.Transpiler
{
	public static class Transpiler
	{
		public static string Core = @"
(defn print [v] (RT.Print (RT.Str v)))
(defn str [x] (RT.Str x))
(defn + [x y] (RT.Add x y))
(defn * [x y] (RT.Mult x y))
(defn = [x y] (RT.Eq x y))
(defn > [x y] (RT.Gt x y))
(defn < [x y] (RT.Lt x y))
(defn inc [x] (RT.Inc x))
(defn dec [x] (RT.Dec x))
(defn pred [fn] (RT.Pred fn))
(defn count [x] (RT.Count x))
(defn take [coll count] (System.Linq.Enumerable.Take (RT.Seq coll) count))
(defn filter [coll fn] (System.Linq.Enumerable.Where (RT.Seq coll) (RT.Pred fn)))
(defn first [coll] (System.Linq.Enumerable.FirstOrDefault (RT.Seq coll)))
(defn conj [coll item] (RT.Conj coll item))
(defn get [coll index] (RT.Get coll index))
(defn reduce [fn values] (RT.Reduce fn values))
;(defn apply [fn values] (RT.Apply fn values))

(defn type [obj] (RT.Type obj))

";


		public static string Compile(this ReaderItem item)
		{
			var tree = item.BuildExpressionTree();
			return tree.Transpile();
		}

		public static object Execute(this IEnumerable<ReaderItem> items)
		{
			string output = "object returnValue = null;\n";
			var i = items.ToArray();

			for (var c = 0; c < i.Length - 1; c++)
			{
				output += i[c].BuildExpressionTree().Transpile() + ";\n";
			}
			output += "return " + i[i.Length - 1].BuildExpressionTree().Transpile() + ";\n";

			var scriptOptions = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
				.WithReferences(Assembly.GetCallingAssembly())
				.WithImports("Lisp.Transpiler");
			var script = CSharpScript.Create<object>(output, scriptOptions, globalsType: typeof(Globals));

			Console.WriteLine("Compiling...");
			script.Compile();

			Console.WriteLine(output);
			Console.WriteLine("=====================================================================================");
			var result = script.RunAsync(new Globals()).Result.ReturnValue;
			return result;
		}

		public static IExpression BuildExpressionTree(this ReaderItem item)
		{
			var expressions = new List<IExpression>() {
				new OperatorExpression(),
				new IfExpression(true),
				new TimeExpression(),
				new LoopExpression(),
				new RecurExpression(),
				new DefnMultiArityExpression(),
				new DefnExpression(),
				new FnExpression(),
				new LiteralExpression(),
				new SymbolExpression(),
				new VectorExpression(),
				new HashMapExpression(),
				new LetExpression(),
				new DefExpression(),
				new ListExpression() };

			foreach (var expr in expressions)
			{
				var result = expr.Create(item);
				if (result != null) return result;
			}
			throw new NotImplementedException(item.GetType().ToString());
		}

		public static string AsIife(this string body) => $"((Fn)(() => {body}))()";
		public static string Wrap(this string body) => $"(Fn)((_) => {body})";

		public static string CommaJoin(this IEnumerable<string> values)
		{
			return string.Join(", ", values);
		}

		public static IEnumerable<string> Transpile(this IEnumerable<IExpression> values)
		{
			return values.Select(x => x.Transpile());
		}
	}
}