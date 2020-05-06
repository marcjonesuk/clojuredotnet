using System;
using System.Collections.Generic;

namespace Lisp.Compiler
{
    public class MultiArityFn : IFn
    {
        private readonly Dictionary<int, object> implementations;
        private readonly object variadicImplementation;

        public MultiArityFn(Dictionary<int, object> implementations, object variadicImplementation = null)
        {
            if (implementations == null) throw new ArgumentNullException("implementations");
            if (implementations.Count == 0) throw new ArgumentOutOfRangeException("implementations");
            this.implementations = implementations;
            this.variadicImplementation = variadicImplementation;
        }

        // Invoke the multi arity function. 
        // If we find an exact arity match, use it. If not, use the variadic implementation if it exists
        public object Invoke(object[] args)
        {
            if (implementations.TryGetValue(args.Length, out var impl))
            {
                return impl.Eval(args);
            }
            else
            {
                if (variadicImplementation != null)
                    return variadicImplementation.Eval(args);
                throw new ArityException(args.Length);
            }
        }
    }
}