using System;

namespace Lisp.Compiler
{
    public class ArityException : Exception
    {
        public ArityException(int argsLength) : base($"Invalid arity: {argsLength}")
        {
        }

        public ArityException(string message, int argsLength) : base($"Invalid arity: {argsLength}; {message}")
        {
        }
    }

    public class ArgumentException : Exception
    {
        public ArgumentException(string message) : base(message)
        {
        }
    }
}