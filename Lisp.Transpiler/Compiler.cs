using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lisp.Reader;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Lisp.Transpiler
{
	public static class Compiler
	{
		public static string Compile(this ReaderItem item)
		{
			var tree = item.BuildExpressionTree();
			return tree.Transpile();
		}

		public static object Compile(this IEnumerable<ReaderItem> items)
		{
			string output = "object returnValue = null;";
			var i = items.ToArray();
			
			for(var c = 0; c < i.Length - 1; c++) {
				output += i[c].BuildExpressionTree().Transpile() + ";\n";
			}
			output += "return " + i[i.Length - 1].BuildExpressionTree().Transpile() + ";";

			var scriptOptions = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
				.WithReferences(Assembly.GetCallingAssembly())
				.WithImports("Lisp.Transpiler");
			var script = CSharpScript.Create<object>(output, scriptOptions, globalsType: typeof(Globals) );
			
			script.Compile();

			Console.WriteLine(output);
			Console.WriteLine("=====================================================================================");
			var result = script.RunAsync(new Globals()).Result.ReturnValue;
			return result; 
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
}