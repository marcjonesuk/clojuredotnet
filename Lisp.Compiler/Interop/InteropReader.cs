using System.Collections.Generic;
using System.Linq;

namespace Lisp.Compiler
{
	public class InteropReader
	{
		// Reads a list of comma-seperated types, e.g. 'System.Object,Func<object,bool>' should return 'Systen.Object' & 'Func<System.Object,bool>'
		public static IEnumerable<string> ReadTypeList(string types)
		{
			var pos = 0;
			var buffer = "";
			int level = 0;
			while (true)
			{
				var c = types[pos];
				if (c == '<')
					level++;
				if (c == '>')
					level--;

				if (c == ',' && level == 0)
				{
					yield return buffer;
					buffer = "";
				}
				else
				{
					buffer += c;
				}
				pos++;
				if (pos == types.Length)
				{
					if (!string.IsNullOrEmpty(buffer)) yield return buffer;
					break;
				}
			}
		}

		public static InteropInfo Read(string info)
		{
			var parts = info.Split("/");
			var typeName = parts[0];
			var methodName = parts[1];

			string[] generics = null;
			if (methodName.Contains("<"))
			{
				generics = methodName.Split('<')[1].Split('>')[0].Split(",").Select(p => p.Trim()).ToArray();
				methodName = methodName.Split('<')[0];
			}

			string[] parameterTypes = null;
			if (parts[1].Contains("("))
			{
				var p = parts[1].Split('(')[1].Split(')')[0];
				parameterTypes = ReadTypeList(p).ToArray();
				methodName = methodName.Split('(')[0];
			}
			
			typeName = typeName.Replace("RT", "Lisp.Compiler.RT").Replace("Seq", "Lisp.Compiler.Seq");
			// if (typeName == "System.Linq.Enumerable")
			// 	typeName = typeName + ", System.Linq";

			return new InteropInfo()
			{
				TypeName = typeName,
				MethodName = methodName,
				GenericTypeParameters = generics,
				ParameterTypes = parameterTypes
			};
		}
	}
}