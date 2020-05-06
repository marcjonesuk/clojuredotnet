using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
    public class Defn : IFn
    {
        private (int, int, int) GetIndexesFromArgs(bool isMultiArityFunction, object[] args)
        {
            var symbolIndex = 0;
            var argumentNamesIndex = 1;

            if (!isMultiArityFunction)
            {
                while (!(args[argumentNamesIndex] is ImmutableArray<object>))
                    argumentNamesIndex++;
            }
            else
            {
                while (args[argumentNamesIndex] is string)
                    argumentNamesIndex++;
            }

            var bodyIndex = argumentNamesIndex + 1;
            return (symbolIndex, argumentNamesIndex, bodyIndex);
        }

        private bool IsMultiArityFunction(object[] args)
        {
            return !args.Any(a => (a.GetType() == typeof(ImmutableArray<object>)));
        }

        public object Invoke(object[] args)
        {
            var symbol = (args[0] as Symbol).Name;
            var isMultiArityFunction = IsMultiArityFunction(args);
            var (symbolIndex, argumentNamesIndex, bodyIndex) = GetIndexesFromArgs(isMultiArityFunction, args);

            if (args[argumentNamesIndex] is ImmutableArray<object> va)
            {
                var body = (IFn)args[bodyIndex];
                var fn = new Fn(va.Cast<Symbol>(), body);
                Environment.Root[symbol] = fn;
                return symbol;
            }
            // Multi arity function
            else
            {
                var implementations = new Dictionary<int, object>();
                object variadic = null;
                foreach (var arity in args.Skip(symbolIndex + 1))
                {
                    var argBodyPair = arity.As<IEnumerable<object>>().ToList();
                    if (argBodyPair.Count != 2) throw new ArityException($"Invalid function definition {argBodyPair.Stringify()}", argBodyPair.Count);
                    var fn = new Fn(((IEnumerable<object>)argBodyPair[0]).Cast<Symbol>(), argBodyPair[1]);
                    if (fn.IsVariadic)
                        variadic = fn;
                    else
                        implementations[argBodyPair[0].As<IList<object>>().Count] = fn;
                }
                Environment.Root[symbol] = new MultiArityFn(implementations, variadic);
                return symbol;
            }
        }
    }
}