using System;
using System.Collections.Generic;
using System.Linq;
using Lisp.Reader;

namespace Lisp.Transpiler
{
	public class LoopExpression : IExpression
	{
		private VectorExpression arguments;
		private IExpression body;
		private List<KeyValuePair<string, object>> values = new List<KeyValuePair<string, object>>();

		public LoopExpression()
		{
		}

		public LoopExpression(ReaderList list)
		{
			var children = list.Children.ToList();
			arguments = (VectorExpression)children[1].BuildExpressionTree();
			body = children[2].BuildExpressionTree();
		}

		public ReaderItem Source { get; }

		public IExpression Create(ReaderItem item)
		{
			if (item is ReaderList list && list.First() is ReaderSymbol sym && sym.Name == "loop")
			{
				return new LoopExpression(list);
			}
			return null;
		}

		public static string Iife(string body) => $"((Iife)(() => {body}))()";

		public string Transpile()
		{
			var vars = "";
			var rebinds = "";
			for (var i = 0; i < arguments.Items.Count; i += 2)
			{
				var name = arguments.Items[i];
				var value = arguments.Items[i + 1];
				vars += $"object {name.Transpile()} = {value.Transpile()}; ";
				rebinds += $"{name.Transpile()} = recur_signal.Args[{i / 2}]; ";
			}
			vars += " /* loop init */";
			rebinds += " /* loop rebind */";

			var loop = $@"
object loop_result = null;
{ vars }
while (true)
{{ 
	loop_result = {body.Transpile()};
	var recur_signal = loop_result as {nameof(Lisp.Transpiler.RecurSignal)};
	if (recur_signal == null) return loop_result;
	{ rebinds }
}}
";
			return Iife("{ " + loop + " }");
		}
	}
}