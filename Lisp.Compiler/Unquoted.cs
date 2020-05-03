namespace Lisp.Compiler
{
    public class Unquoted : IStringify
    {
        public object Value { get; }

        public Unquoted(object value)
        {
            Value = value;
        }

        public string Stringify(bool quoteStrings) => "~" + Value.Stringify();
    }
}
