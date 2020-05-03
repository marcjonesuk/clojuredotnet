using System;

namespace Lisp.Compiler
{
    public class Function : IFn
    {
        public string Name { get; }
        public Func<object[], object> fn;

        public Function(Func<object[], object> fn, string name = null)
        {
            this.fn = fn;
            Name = name;
        }

        public object Invoke(object[] args)
        {
            try
            {
                return fn(args);
            }
            catch (Exception e)
            {
                throw e.Wrap(this);
            }
        }

        public override string ToString() => $"{Name}";
    }
}