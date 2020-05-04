using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public class DefTest : IFn
	{
		public object Invoke(object[] args)
		{
			var description = args[0].As<string>();
			var body = args[1];
			object result = body.Eval();
			if (result is bool b && b)
				Console.WriteLine($"Success: {description}");
			else 
				Console.WriteLine($"Failed: {description}");
			return true;
		}
	}
}