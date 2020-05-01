using System.Collections.Generic;

namespace Lisp.Compiler
{
	public class CustomComparer : IEqualityComparer<object>
	{
		public new bool Equals(object x, object y)
		{
			return RT.Equiv_(x, y);
		}

		public int GetHashCode(object obj)
		{
			return RT.Hash_(obj);
		}
	}
}