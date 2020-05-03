using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
			var expressions = new Reader().Read(code);
			var compiled = expressions.Select(e => Compile(e, false)).ToList();
			foreach(var c in compiled) {
				if (c is SymbolicExpression sym) sym.Parent = null;
			}
			return new Program_(compiled);
		}

		public IFn Compile(IEnumerable<object> expressions)
		{
			return new Program_(expressions.Select(e => Compile(e, false)).ToList());
		}

		private object CompileList(IList<object> expression, bool quoted)
		{
			var items = expression.Select(item => Compile(item, quoted)).ToList();
			if (quoted)
				return new Function(_ => items.Select(item => item.Eval()).ToImmutableList(), items.Stringify());
			else
				return new SymbolicExpression(items);
		}

		private object CompileArray(ImmutableArray<object> array, bool quoted)
		{
			// var items = array.Select(item => Compile(item, quoted)).ToList();
			// return new Function(_ => items.Select(item => item.Eval()).ToImmutableArray(), items.Stringify());

			var items = array.Select(item => Compile(item, quoted)).ToImmutableArray();
            return items;
		}

		private object CompileEnumerable(IEnumerable<object> enumerable, bool quoted)
		{
			return enumerable.Select(i => Compile(i, quoted));
		}

		// todo: add interop quoting?
		// todo: need to add hashmap etc here now?
		private object Compile(object o, bool quoted)
		{
			if (quoted)
			{
				return o switch
				{
					ImmutableArray<object> l => CompileArray(l, quoted),
					IList<object> l => CompileList(l, quoted),
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
					ImmutableArray<object> l => CompileArray(l, quoted),
					IList<object> l => CompileList(l, quoted),
					// IEnumerable<object> e => CompileEnumerable(e, quoted),
					Symbol sym => sym,
					Quoted q => Compile(q.Value, true),
					Unquoted q => Compile(q.Value, false),
					int i => new Function(_ => i, i.ToString()),
					double d => new Function(_ => d, d.ToString()),
					string str => new Function(_ => str, str),
					bool b => new Function(_ => b, b.ToString()),
					null => new Function(_ => null, "null"),
					_ => o
				};
			}
		}
	}
}
