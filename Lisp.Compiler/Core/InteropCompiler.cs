using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisp.Compiler
{
	public class InteropCompiler
	{
		public static IFn Create(MethodInfo mi)
		{
			var parameters = mi.GetParameters();

			object d = null;

			if (parameters.Length == 1 && parameters[0].ParameterType == typeof(object[]))
				d = (InteropDelegate)Delegate.CreateDelegate(typeof(InteropDelegate), null, mi);
			else
				d = parameters.Length switch
				{
					0 => (InteropDelegate0Arg)Delegate.CreateDelegate(typeof(InteropDelegate0Arg), null, mi),
					1 => (InteropDelegate1Arg)Delegate.CreateDelegate(typeof(InteropDelegate1Arg), null, mi),
					2 => (InteropDelegate2Arg)Delegate.CreateDelegate(typeof(InteropDelegate2Arg), null, mi),
					3 => (InteropDelegate3Arg)Delegate.CreateDelegate(typeof(InteropDelegate3Arg), null, mi),
					_ => (InteropDelegate)Delegate.CreateDelegate(typeof(InteropDelegate), null, mi),
				};

			return new Function(async args => await d.Eval(args), "Interop_" + mi.Name);
		}

		public static IFn Create(string name)
		{
			var typeName = name.Split("/")[0].Replace("RT", "Lisp.Compiler.RT").Replace("Seq", "Lisp.Compiler.Seq");
			var type = Type.GetType(typeName);
			if (type == null) throw new InteropException($"Type '{typeName}' cannot be found ({name})");

			var methodName = name.Split("/")[1];
			var methods = type.GetMethods().Where(m => m.Name.ToLower() == methodName.ToLower()).ToList();
			if (methods.Count == 0) throw new InteropException($"Method '{methodName}' does not exist on type '{type.FullName}' ({name})");

			// Single arity function
			if (methods.Count == 1)
				return Create(methods[0]);

			// Multi arity function 
			var arities = new Dictionary<int, object>();
			foreach (var mi in methods)
			{
				var parameterCount = mi.GetParameters().Count();
				if (arities.ContainsKey(parameterCount)) throw new CompileTimeException($"Interop does not support parameter overloading: {name}");
				arities[parameterCount] = Create(mi);
			}
			return new MultiArityFunction(arities);
		}
	}
}