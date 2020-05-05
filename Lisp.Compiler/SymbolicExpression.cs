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
		private IList<object> Args { get; }
		public Environment Env { get; set; }
		private bool _isSpecialForm;
		private object[] _args;

		public SymbolicExpression(IList<object> items)
		{
			Items = items;

			if (items != null && items.Count > 1)
				_args = items.Skip(1).ToArray();

			_isSpecialForm = (Items != null && Items.Count > 0 && Items[0] is Symbol && Environment.SpecialForms.Contains(((Symbol)Items[0]).Name));
			arg0 = Items[0] as IFn;
		}

		public override string ToString() => "(" + string.Join(' ', Items.Select(item => item.Stringify())) + ")";

		private SymbolicExpression _parent;
		public SymbolicExpression Parent
		{
			set
			{
				_parent = value;
				if (_parent == null)
					Env = new Environment(this, null);
				else
					Env = new Environment(this, _parent.Env);

				foreach (var i in Items.Where(i => i is SymbolicExpression sym).Cast<SymbolicExpression>())
				{
					i.Parent = this;
				}
			}
		}

		private IFn arg0 = null;
		public object Invoke(object[] args)
		{
			try
			{
				if (Env != null)
					Environment.Current = Env;

				if (Items == null || Items.Count == 0) return ImmutableArray<object>.Empty;

				var fn = Items[0].Eval();

				if (_isSpecialForm)
				{
					// Special forms get arguments without evaluation
					return fn.Eval(_args);
				}
				else
				{
					if (_args == null)
						return fn.Eval();

					// Evaluate arguments and invoke
					for (var i = 0; i < _args.Length; i++)
						_args[i] = _args[i].Eval();

					return fn.Eval(_args);
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