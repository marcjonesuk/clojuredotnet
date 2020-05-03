namespace Lisp.Compiler
{
    public interface IStringify
    {
        string Stringify(bool quoteString = false);
    }
}