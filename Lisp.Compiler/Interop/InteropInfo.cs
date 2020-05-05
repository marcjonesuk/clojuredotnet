namespace Lisp.Compiler
{
	public class InteropInfo
	{
		public string TypeName { get; set; }
		public string MethodName { get; set; }
		public string[] GenericTypeParameters { get; set; }
		public string[] ParameterTypes { get; set; }

		public override string ToString() => $"{TypeName}/{MethodName}";
	}
}