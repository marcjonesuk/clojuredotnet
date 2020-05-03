using System.Linq;

namespace Lisp.Compiler
{
    public class If : IFn
    {
        public object Invoke(object[] args)
        {
            return args.Length switch
            {
                2 => RT.IsTruthy(args[0].Eval()) switch
                {
                    true => args[1].Eval(),
                    false => null,
                },
                3 => RT.IsTruthy(args[0].Eval()) switch
                {
                    true => args[1].Eval(),
                    false => args[2].Eval(),
                },
                _ => throw new ArityException(args.Length)
            };
        }
    }
}