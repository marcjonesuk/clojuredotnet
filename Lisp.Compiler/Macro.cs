// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace Lisp.Compiler
// {
// 	public sealed class Macro : IFn
// 	{
// 		public Func<object[], object> fn;
// 		private readonly string[] argumentNames;

// 		private void BindArgumentValues(object[] args)
// 		{
// 			object[] values = new object[argumentNames.Length];
// 			for (var i = 0; i < argumentNames.Length; i++)
// 			{
// 				if (i == argumentNames.Length - 1 && argumentNames[i][0..1] == "&")
// 				{
// 					State.Current[argumentNames[i][1..]] = args[i..];
// 				}
// 				else
// 				{
// 					State.Current[argumentNames[i]] = args[i];
// 				}
// 			}
// 		}

// 		public Macro(IEnumerable<string> argumentNames, Func<object[], object> fn, string name = null)
// 		{
// 			this.argumentNames = argumentNames.ToArray();
// 			this.fn = fn;
// 			Name = name;
// 		}

// 		public string Name { get; }

// 		public object Invoke()
// 		{
// 			return fn(null);
// 		}

// 		public object Invoke(object[] args)
// 		{
// 			BindArgumentValues(args);
// 			return fn(args);
// 		}
// 	}
// }