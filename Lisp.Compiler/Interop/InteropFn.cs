namespace Lisp.Compiler
{
	public class InteropFn : IFn
	{
		private IFn _cached = null;
		public object Invoke(object[] args)
		{
			if (_cached == null) {
				_cached = InteropCompiler.Create(args[0].As<string>());
			}
			return _cached;
		}
	}
}