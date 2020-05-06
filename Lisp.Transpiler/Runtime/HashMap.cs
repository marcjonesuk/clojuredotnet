using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.Transpiler
{
	public class HashMap 
	{
		public Dictionary<string, object> Items { get; set; }

		public override string ToString()
		{
			return "[" + string.Join(" ", Items.Select(i => RT.Str(i, true))) + "]";
		}
	}
}