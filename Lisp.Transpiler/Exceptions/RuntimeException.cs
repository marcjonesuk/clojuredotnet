using System;
using System.Collections.Generic;
using System.Linq;

namespace Lisp.Compiler
{
    public static class ExceptionExtensions
    {
        public static RuntimeException Wrap(this Exception ex, object location)
        {
            return RuntimeException.Create(ex, location);
        }
    }

    public class RuntimeException : Exception
    {
        public Stack<object> Stack { get; }

        private RuntimeException(Stack<object> trace, Exception inner)
            : base(inner.Message + GetTrace(trace), inner)
        {
            Stack = trace;
        }

        public override string ToString()
        {
            return Message;
        }
	
	
		private static string GetTrace(Stack<object> stack) => "todo";
        // private static string GetTrace(Stack<object> stack) => "\n    Stack Trace: \n      " + string.Join("\n      => ", stack.Reverse().Select(s => RT. s.Stringify())) + "\n\n";

        public static RuntimeException Create(Exception inner, object location)
        {
            if (inner is RuntimeException rt)
            {
                var stack = rt.Stack;
                stack.Push(location);
                return new RuntimeException(stack, rt.InnerException);
            }
            else
            {
                var stack = new Stack<object>();
                stack.Push(location);
                return new RuntimeException(stack, inner);
            }
        }
    }
}