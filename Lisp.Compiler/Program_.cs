using System;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.Compiler
{
    public class Program_ : List<object>, IFn, IStringify
    {
        public Program_(List<IExpression> items) : base(items)
        {
        }

        public object Value { get; }

        public override string ToString() => string.Join(' ', this.Select(i => i.ToString()));

        public object Invoke(object[] args)
        {
            object current = null;
            try
            {
                object result = null;
                foreach (object item in this)
                {
                    current = item;
                    result = item.Eval();
                }
                return result;
            }
            catch (Exception e)
            {
                // Only wrap .NET exceptions here
                if (e is RuntimeException) throw;
                throw e.Wrap(current);
            }
        }

        public string Stringify(bool quoteStrings) => ToString();
    }
}