using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lisp.Compiler
{
	public class MacroProcessor
	{
		public static IList<object> Compile(IEnumerable<object> expression)
		{
			return expression.Select(i =>
			{
				if (i is IList<object> list && list[0] is Symbol sym && State.Current.Macros.ContainsKey(sym.Name))
				{
					var m = State.Current.Macros[sym.Name].Invoke(list.Skip(1).ToArray());
					return m;
				}
				return i;
			})
			.ToImmutableList();
		}
	}
}
