using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
    public interface IBindingFunction
    {
        ISet<Symbol> Bindings { get; }
        object Body { get; }
    }

    public class Let : IFn, IBindingFunction
    {
        public ISet<Symbol> Bindings => throw new NotImplementedException();

        public object Body => throw new NotImplementedException();

        public IEnumerable<object> FlatZip(IEnumerable<object> one, IEnumerable<object> two)
        {
            var t = two.GetEnumerator();
            foreach (var i in one)
            {
                yield return i;
                if (t.MoveNext())
                {
                    yield return t.Current;
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void Destructure(IList<object> bindings, IEnumerable<object> x, IEnumerable<object> y)
        {
            bool done = false;
            IEnumerator<object> yen = null;
            if (y != null)
            {
                yen = y.GetEnumerator();
                done = !yen.MoveNext();
            }
            else { done = true; }

            foreach (var item in x)
            {
                if (item is Symbol sym)
                {
                    if (done) { bindings.Add(sym); bindings.Add(null); }
                    else // if (!(yen.Current is ImmutableArray<object>))
                    {
                        bindings.Add(sym);
                        bindings.Add(yen.Current);
                        if (!done) done = !yen.MoveNext();
                    }
                }
                else if (item is ImmutableArray<object> xInner)
                {
                    if (done) Destructure(bindings, xInner, null);
                    if (yen.Current is ImmutableArray<object> yInner)
                    {
                        Destructure(bindings, xInner, yInner);
                        if (!done) done = !yen.MoveNext();
                    }
                }
                else throw new Exception("boom");
            }
        }

        public object Invoke(object[] args)
        {
            if (args[0] is IList<object> bindings)
            {
                if (bindings[0] is IList<object> l)
                {
                    var b = new List<object>();
                    Destructure(b, l, (IEnumerable<object>)bindings[1].Eval());
                    bindings = b;
                }
                for (var i = 0; i < bindings.Count; i += 2)
                {
                    var symbol = ((Symbol)bindings[i]).Name;

					// Nasty hack for where a let expression contains a symbolic expression
					if (bindings[i + 1] is SymbolicExpression symbolicExpression)
						symbolicExpression.Parent = Environment.Current.SymbolicExpression;

                    var value = bindings[i + 1].Eval();
                    Environment.Current[symbol] = value;
                }
            }
            object result = null;
            if (args.Length == 1)
                return null;

            for (var i = 1; i < args.Length; i++)
                result = args[i].Eval();
            return result;
        }
    }
}