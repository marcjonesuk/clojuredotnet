using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Lisp.Compiler
{
	public class InteropDispatcher : IFn
	{
		private readonly InteropInfo info;

		Dictionary<string, IFn> cached = new Dictionary<string, IFn>();

		public InteropDispatcher(InteropInfo info)
		{
			this.info = info;
		}

		public string GetCachingKey(object[] args)
		{
			if (args == null) return null;
			string key = "";
			for (var a = 0; a < args.Length; a++)
				key += args[a].GetType();
			return key;
		}

		public MethodInfo GetMethodInfo(object[] args)
		{
			Type type = null;
			type = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => t.FullName == info.TypeName)
				.SingleOrDefault();

			if (type == null)
			{
				type = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => t.Name == info.TypeName)
				.SingleOrDefault();
			}

			if (type == null)
				throw new InteropException($"Type '{info.TypeName}' cannot be found");

			var methodInfos = type.GetMethods().Where(m => m.Name == info.MethodName && m.GetParameters().Length == args.Length).ToArray();

			if (methodInfos.Length == 1)
			{
				return methodInfos[0];
			}
			else
			{
				// Exact match
				for (var m = 0; m < methodInfos.Length; m++)
				{
					var mi = methodInfos[m];
					var parameterTypes = mi.GetParameters().Select(p => p.ParameterType).ToArray();
					var match = true;
					for (var p = 0; p < mi.GetParameters().Length; p++)
					{
						if (args[0].GetType() != parameterTypes[p])
						{
							match = false;
							break;
						}
					}
					if (match)
					{
						return mi;
					}
				}
				var objArray = type.GetMethods().Where(mi => mi.Name == info.MethodName && mi.GetParameters().Length == 1 && mi.GetParameters()[0].ParameterType == typeof(object[])).ToArray();
				if (objArray.Length > 0)
					return objArray[0];	

				throw new InteropException($"Could not find suitable method to bind to {info}");
			}
		}

		// Could use number of args first then type to speed this up
		public object Invoke(object[] args)
		{
			var key = GetCachingKey(args);
			if (!cached.ContainsKey(key))
			{
				var mi = GetMethodInfo(args);
				var fn = InteropCompiler.Create(mi, info.GenericTypeParameters);
				cached[key] = fn;
			}
			return cached[key].Invoke(args);
		}
	}

	public class InteropCompiler
	{
		public static IFn Dynamicize<T1, T2, T3, TResult>(MethodInfo mi)
		{
			var d = (Func<T1, T2, T3, TResult>)Delegate.CreateDelegate(typeof(Func<T1, T2, T3, TResult>), null, mi);
			return new Function(args => d((dynamic)args[0], (dynamic)args[1], (dynamic)args[2]));
		}

		public static IFn Dynamicize<T1, T2, TResult>(MethodInfo mi)
		{
			var d = (Func<T1, T2, TResult>)Delegate.CreateDelegate(typeof(Func<T1, T2, TResult>), null, mi);
			return new Function(args => d((dynamic)args[0], (dynamic)args[1]));
		}

		public static IFn Dynamicize<T1, TResult>(MethodInfo mi)
		{
			var d = (Func<T1, TResult>)Delegate.CreateDelegate(typeof(Func<T1, TResult>), null, mi);
			return new Function(args => d((dynamic)args[0]));
		}

		public static IFn DynamicizePassThrough(MethodInfo mi)
		{
			var d = (Func<object[], object>)Delegate.CreateDelegate(typeof(Func<object[], object>), null, mi);
			return new Function(args => d(args));
		}

		public static IFn Dynamicize<TResult>(MethodInfo mi)
		{
			var d = (Func<TResult>)Delegate.CreateDelegate(typeof(Func<TResult>), null, mi);
			return new Function(args => d());
		}

		public static IFn DynamicizeVoid(MethodInfo mi)
		{
			var d = (Action)Delegate.CreateDelegate(typeof(Action), null, mi);
			return new Function(args => { d(); return null; });
		}

		public static IFn DynamicizeVoid<T1>(MethodInfo mi)
		{
			var d = (Action<T1>)Delegate.CreateDelegate(typeof(Action<T1>), null, mi);
			return new Function(args => { d((dynamic)args[0]); return null; });
		}

		public static IFn DynamicizeVoid<T1, T2>(MethodInfo mi)
		{
			var d = (Action<T1, T2>)Delegate.CreateDelegate(typeof(Action<T1, T2>), null, mi);
			return new Function(args => { d((dynamic)args[0], (dynamic)args[1]); return null; });
		}

		public static IFn Create(MethodInfo mi, string[] generic = null)
		{
			var parameters = mi.GetParameters();

			if (generic != null)
				mi = mi.MakeGenericMethod(Type.GetType(generic[0]));

			var parametersLength = mi.GetParameters().Length;

			if (mi.ReturnType == typeof(object) && parametersLength == 1 && mi.GetParameters()[0].ParameterType == typeof(object[])) {
				return DynamicizePassThrough(mi);
			}

			// Returns something
			if (mi.ReturnType != typeof(void) && parametersLength <= 3)
			{
				var dynamicize = typeof(InteropCompiler).GetMethods()
					.Where(mi => mi.Name == nameof(Dynamicize) && mi.GetGenericArguments().Length == parametersLength + 1)
					.Single();
				var genericTypeArguments = mi.GetParameters().Select(p => p.ParameterType).Concat(new Type[] { mi.ReturnType });
				dynamicize = dynamicize.MakeGenericMethod(genericTypeArguments.ToArray());
				return (IFn)dynamicize.Invoke(null, new object[] { mi });
			}
			// Returns void
			else if (parametersLength <= 2)
			{
				var dynamicize = typeof(InteropCompiler).GetMethods()
					.Where(mi => mi.Name == nameof(DynamicizeVoid) && mi.GetGenericArguments().Length == parametersLength)
					.Single();
				var genericTypeArguments = mi.GetParameters().Select(p => p.ParameterType);
				dynamicize = dynamicize.MakeGenericMethod(genericTypeArguments.ToArray());
				return (IFn)dynamicize.Invoke(null, new object[] { mi });
			}

			return new Function(args =>
			{
				try
				{
					Console.WriteLine("Using reflection!");
					return mi.Invoke(null, args);
				}
				catch (Exception e)
				{
					throw e.Wrap(null);
				}
			});
		}

		public static IFn Create(string name)
		{
			var info = InteropReader.Read(name);
			return new InteropDispatcher(info);

			// Type type = null;
			// type = AppDomain.CurrentDomain.GetAssemblies()
			// 	.SelectMany(a => a.GetTypes())
			// 	.Where(t => t.FullName == info.TypeName)
			// 	.SingleOrDefault();

			// if (type == null)
			// {
			// 	type = AppDomain.CurrentDomain.GetAssemblies()
			// 	.SelectMany(a => a.GetTypes())
			// 	.Where(t => t.Name == info.TypeName)
			// 	.SingleOrDefault();
			// }

			// if (type == null)
			// 	throw new InteropException($"Type '{info.TypeName}' cannot be found ({name})");

			// var methods = type.GetMethods().Where(m => m.Name.ToLower() == info.MethodName.ToLower()).ToList();
			// if (methods.Count == 0) throw new InteropException($"Method '{info.MethodName}' does not exist on type '{type.FullName}' ({name})");

			// // Single arity function
			// if (methods.Count == 1)
			// 	return Create(methods[0], info.GenericTypeParameters);

			// if (info.ParameterTypes != null)
			// {
			// 	var te = typeof(IEnumerable<object>);
			// 	// Find methodinfo with matching supplied signature
			// 	var ps = info.ParameterTypes.Select(p => Type.GetType(p)).ToArray();
			// 	var mi = type.GetMethod(info.MethodName, ps);
			// 	return Create(mi, info.GenericTypeParameters);
			// }

			// // Multi arity function 
			// var arities = new Dictionary<int, object>();
			// foreach (var mi in methods)
			// {
			// 	var parameters = mi.GetParameters();
			// 	var parameterCount = mi.GetParameters().Count();
			// 	if (arities.ContainsKey(parameterCount))
			// 		throw new CompileTimeException($"Interop does not support parameter overloading: {name}, {parameterCount}");
			// 	arities[parameterCount] = Create(mi, info.GenericTypeParameters);
			// }
			// return new MultiArityFunction(arities);
		}
	}
}