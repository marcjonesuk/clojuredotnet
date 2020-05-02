using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisp.Compiler
{
	public class MultiArityFunction : IFn
	{
		private readonly Dictionary<int, object> implementations;
		private readonly object variadicImplementation;

		public MultiArityFunction(Dictionary<int, object> implementations, object variadicImplementation = null)
		{
			if (implementations == null) throw new ArgumentNullException("implementations");
			if (implementations.Count == 0) throw new ArgumentOutOfRangeException("implementations");
			this.implementations = implementations;
			this.variadicImplementation = variadicImplementation;
		}

		// Invokes the multi arity function. 
		// If we find an exact arity match, use it. If not, use the variadic implementation if it exists,
		// allowing it to validate any argument constraints
		public async Task<object> Invoke(object[] args)
		{
			if (implementations.TryGetValue(args.Length, out var impl))
			{
				return await impl.Eval(args);
			}
			else
			{
				if (variadicImplementation != null)
					return await variadicImplementation.Eval(args);
				throw new ArityException(args.Length);
			}
		}
	}
}