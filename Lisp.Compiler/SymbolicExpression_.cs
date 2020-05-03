using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public class SymbolicExpression : IEnumerable<object>, IFn
	{
		public IList<object> Items { get; }
		public string Source { get; }
		private IList<object> Args { get; }
		public Environment Env { get; set; }

		private IFn fn = null;

		private object[] _args;

		public SymbolicExpression(IList<object> items)
		{
			Items = items;

			if (items != null && items.Count > 1)
				_args = items.Skip(1).ToArray();

			fn = (IFn)Items[0];
		}

		public override string ToString() => "(" + string.Join(' ', Items.Select(item => item.Stringify())) + ")";

		private SymbolicExpression _parent;
		public SymbolicExpression Parent
		{
			set
			{
				_parent = value;
				if (_parent == null)
					Env = new Environment(null);
				else
					Env = new Environment(_parent.Env);

				foreach (var i in Items.Where(i => i is SymbolicExpression sym).Cast<SymbolicExpression>())
				{
					i.Parent = this;
				}
			}
		}

		public object Invoke(object[] args)
		{
			try
			{
				if (Env != null)
					Environment.Current = Env;

				if (Items == null || Items.Count == 0) return ImmutableArray<object>.Empty;
				var fn = Items[0].Eval(null);
				if (Items[0] is Symbol && Environment.Root.Keywords.Contains(((Symbol)Items[0]).Name))
				{
					// Pass arguments un-evaled
					return fn.Eval(_args);
				}
				else
				{
					if (_args == null)
						return fn.Eval();

					// Evaluate arguments and invoke
					var evaled = new object[_args.Length];
					for (var i = 0; i < _args.Length; i++)
						evaled[i] = _args[i].Eval();

					return fn.Eval(evaled);
				}
			}
			catch (Exception e)
			{
				throw e.Wrap(this);
			}
			finally
			{
				if (Env != null)
				{
					Environment.Current = Env.Parent;
					Env.Clear();
				}
			}
		}

		public IEnumerator<object> GetEnumerator()
		{
			return ((IEnumerable<object>)Items).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Items.GetEnumerator();
		}
	}
}