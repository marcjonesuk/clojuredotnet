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
		public State ExpressionState { get; set; }

		private object[] _args;

		public SymbolicExpression(IList<object> items)
		{
			Items = items;

			if (items != null && items.Count > 1)
				_args = items.Skip(1).ToArray();
		}

		public override string ToString() => "(" + string.Join(' ', Items.Select(item => item.Stringify())) + ")";

		private SymbolicExpression _parent;
		public SymbolicExpression Parent
		{
			set
			{
				_parent = value;
				if (_parent == null)
					ExpressionState = new State(null);
				else
					ExpressionState = new State(_parent.ExpressionState);

				foreach (var i in Items.Where(i => i is SymbolicExpression sym).Cast<SymbolicExpression>())
				{
					i.Parent = this;
				}
			}
		}

		public object Invoke(object[] args)
		{
			// throw new Exception("Null state");

			try
			{
				if (ExpressionState != null)
					State.Current = ExpressionState;

				if (Items == null || Items.Count == 0) return ImmutableArray<object>.Empty;
				var fn = Items[0].Eval(null);
				if (Items[0] is Symbol && State.Root.Keywords.Contains(((Symbol)Items[0]).Name))
				{
					// Pass arguments un-evaled
					return fn.Eval(_args);
				}
				else
				{
					// Evaluate arguments and invoke
					if (_args == null)
						return fn.Eval();

					var evaled = _args.Select(i => i.Eval()).ToArray();
					return fn.Eval(evaled);
				}
			}
			catch (Exception e)
			{
				throw e.Wrap(this);
			}
			finally
			{
				if (ExpressionState != null)
				{
					State.Current = ExpressionState.ParentState;
					ExpressionState.Clear();
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