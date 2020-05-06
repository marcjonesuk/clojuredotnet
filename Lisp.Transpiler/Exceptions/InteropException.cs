using System;

namespace Lisp.Transpiler
{
    public class InteropException : Exception
    {
        public InteropException(string message) : base(message)
        {
        }
    }
}