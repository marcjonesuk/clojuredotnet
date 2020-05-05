using System;
using System.Linq.Expressions;

namespace DLRTest
{
    class Program
    {
		public class RunTime {
			public static object Add(int x, int y) {
				return x+y;
			}
		}

		public static BlockExpression CreateAddFunction() {
			ParameterExpression value = Expression.Parameter(typeof(int), "value");
			ParameterExpression value2 = Expression.Parameter(typeof(int), "value");

			var addMethodInfo = typeof(RunTime).GetMethods()[0];
			var addBody = Expression.Block(
				Expression.Call(addMethodInfo, new Expression[] { Expression.Constant(1), value })
			);

			return addBody;

			// dynamic del = Expression.Lambda(addBody, false, value).Compile();
			// return del;
		}

        static void Main(string[] args)
        {
			// ParameterExpression value = Expression.Parameter(typeof(int), "value");
			// ParameterExpression value2 = Expression.Parameter(typeof(int), "value");

			// var addMethodInfo = typeof(RunTime).GetMethods()[0];
			// var addBody = Expression.Block(
			// 	Expression.Call(addMethodInfo, new Expression[] { Expression.Constant(1), value })
			// );

			// dynamic del = Expression.Lambda(addBody, false, value).Compile();

			BlockExpression del = CreateAddFunction();


	
			Console.WriteLine(del(5));

		


			// var x = new Lisp.Compiler.Reader().Read("(+ 1 2)");
			// Console.ReadKey();
		}
    }
}
