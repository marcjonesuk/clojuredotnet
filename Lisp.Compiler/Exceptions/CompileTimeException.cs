using System;
using System.Runtime.Serialization;

namespace Lisp.Compiler
{
	[Serializable]
	internal class CompileTimeException : Exception
	{
		public CompileTimeException()
		{
		}

		public CompileTimeException(string message) : base(message)
		{
		}

		public CompileTimeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected CompileTimeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
	
	[Serializable]
	internal class RuntimeArgumentException: Exception
	{
		public RuntimeArgumentException()
		{
		}

		public RuntimeArgumentException(int position, Type expectedType)
		{
		}

		public RuntimeArgumentException(string message) : base(message)
		{
		}

		public RuntimeArgumentException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected RuntimeArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}