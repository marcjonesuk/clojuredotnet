using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lisp.Compiler.Tests
{
	public class Test
	{
		public static object Add(dynamic x) => x;
		public static object Add(dynamic x, dynamic y) => x + y;
	}

	[TestClass]
	public class InteropTests
	{
		[TestMethod]
		public void None()
		{
			Stopwatch sw = new Stopwatch();
			var fn = new Function(args => Test.Add(args[0], args[1]));
			var args = new object[] { 1, 2 };

			for (int i = 0; i < 1000; i++)
			{
				fn.Invoke(args);
			}

			sw.Start();

			for (int i = 0; i < 10000000; i++)
			{
				fn.Invoke(args);
				// Test.Add(args[0], args[1]);
			}

			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		[TestMethod]
		public void Simple()
		{
			Stopwatch sw = new Stopwatch();
			var name = typeof(Test).FullName;
			var fn = InteropCompiler.Create("Lisp.Compiler.Tests.Test/Add");
			var args = new object[] { 1, 2 };

			for (int i = 0; i < 1000; i++)
			{
				fn.Invoke(args); // Eval(args);
			}

			sw.Start();
			for (int i = 0; i < 10000000; i++)
			{
				fn.Invoke(args); // Eval(args);
			}

			Console.WriteLine(sw.ElapsedMilliseconds);

		}
	}
}