using System;

namespace Lisp.Compiler
{
	public class InteropException : Exception
	{
		public InteropException(string message): base(message)
		{
		}
	}
}