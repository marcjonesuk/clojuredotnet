// using System.Collections.Generic;
// using System.Linq;

// namespace Lisp.Compiler
// {
// 	public class Apply : IFn
// 	{
// 		public object Invoke(object[] args)
// 		{
// 			var fn = args[0].As<IFn>();
// 			var last = (IEnumerable<object>)args?.Last();
// 			var newArgs = args[1..^1].Union(last).ToArray();
// 			return fn.Invoke(newArgs);
// 		}
// 	}
// }