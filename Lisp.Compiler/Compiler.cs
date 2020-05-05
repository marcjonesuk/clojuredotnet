using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Compiler
{
    public class Compiler
    {
        public Compiler()
        {
            Environment.Root.Bootstrap();
        }

        public IFn Compile(string code)
        {
            var expressions = new Rdr().Read(code);
            var compiled = expressions.Select(e => Compile(e, false)).ToList();
            foreach (var c in compiled)
            {
                if (c is SymbolicExpression sym) sym.Parent = null;
            }
            return new Program_(compiled);
        }

        public IFn Compile(IEnumerable<object> expressions)
        {
            return new Program_(expressions.Select(e => Compile(e, false)).ToList());
        }

        private object CompileList(IEnumerable<ReaderItem> expression, bool quoted)
        {
            var items = expression.Select(item => Compile(item, quoted)).ToList();
            if (quoted)
                return new Function(_ => items.Select(item => item.Eval()).ToImmutableList(), items.Stringify());
            else
                return new SymbolicExpression(items);
        }

		private object CompileArray(IEnumerable<ReaderItem> expression, bool quoted)
        {
            var items = expression.Select(item => Compile(item, quoted)).ToList();
            // if (quoted)
            //     return new Function(_ => items.Select(item => item.Eval()).ToImmutableList(), items.Stringify());
            // else
            return items.ToImmutableArray();
        }

        // todo: add more quoting for other types?
        // todo: need to add hashmap!!
        private object Compile(object o, bool quoted)
        {
            if (quoted)
            {
                return o switch
                {
					ReaderLiteral literal => literal.Value, 
					ReaderVector vector => CompileArray(vector, quoted),
                    ReaderList list => CompileList(list, quoted),
                    Symbol sym => new Function(_ => sym, $"symbol({sym.Name})"),
                    Quoted q => Compile(q.Value, true),
                    Unquoted q => Compile(q.Value, false),
                    _ => o
                };
            }
            else
            {
                return o switch
                {
					ReaderLiteral literal => literal.Value, 
					ReaderVector vector => CompileArray(vector, quoted),
                    ReaderList list => CompileList(list, quoted),
                    ReaderSymbol sym => new Symbol(sym.Name, false, sym.Token),
                    Quoted q => Compile(q.Value, true),
                    Unquoted q => Compile(q.Value, false),
                    _ => o
                };
            }
        }
    }
}
