namespace Lisp.Compiler
{
    public class Quoted : IStringify
    {
        public object Value { get; }

        public Quoted(object value)
        {
            Value = value;
        }

        public string Stringify(bool quoteStrings) => "`" + Value.Stringify(quoteStrings);
    }
}
