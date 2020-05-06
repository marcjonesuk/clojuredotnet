using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Lisp.Compiler
{
	public class Environment
	{
		public SymbolicExpression SymbolicExpression { get; set; }
		public Environment Parent { get; set; }
		public static HashSet<string> SpecialForms { get; set; }
		public Dictionary<string, object> SymbolTable { get; set; }
		public Dictionary<string, IFn> Macros { get; set; }
		public static Environment Current = null;
		public static Environment Root = new Environment();

		static Environment()
		{
			SpecialForms = new HashSet<string>();
			SpecialForms.Add("def");
			SpecialForms.Add("defn");
			SpecialForms.Add("defmacro");
			SpecialForms.Add("fn");
			SpecialForms.Add("let");
			SpecialForms.Add("if");
			SpecialForms.Add("loop");
		}

		public Environment()
		{
			SymbolTable = new Dictionary<string, object>();
			this["if"] = new If();
			this["defn"] = new Defn();
			this["fn"] = InteropCompiler.Create("RT/Fn");
			this["let"] = new Let();
			this["def"] = new Function(args => RT.Def(args[0], args[1]));
			this["str"] = new Function(args => RT.Str(args)); 
			this["apply"] = new Function(args => RT.Apply(args));
			this["map"] = InteropCompiler.Create("RT/Map");
			this["reduce"] = new Reduce();
			this["interop"] = new InteropFn();
		}

		public Environment(SymbolicExpression symbolicExpression, Environment parent)
		{
			if (parent == null) parent = Root;
			SymbolicExpression = symbolicExpression;
			Parent = parent;
			SymbolTable = new Dictionary<string, object>();
		}

		private bool _bootstrapped = false;

		public void Bootstrap()
		{
			if (_bootstrapped) return;
			_bootstrapped = true;

			this["="] = InteropCompiler.Create("RT/Equiv");
			this["hash"] = InteropCompiler.Create("RT/Hash");
			this["-"] = InteropCompiler.Create("RT/Subtract");
			this["/"] = InteropCompiler.Create("RT/Divide");

			this["inc"] = InteropCompiler.Create("RT/Inc");
			this["dec"] = InteropCompiler.Create("RT/Dec");

			this[">"] = InteropCompiler.Create("RT/Gt");
			this["<"] = InteropCompiler.Create("RT/Lt");

			this["quot"] = InteropCompiler.Create("RT/Quot");
			this["mod"] = InteropCompiler.Create("RT/Mod");

			this["odd?"] = InteropCompiler.Create("RT/Odd");
			this["even?"] = InteropCompiler.Create("RT/Even");

			this["string?"] = InteropCompiler.Create("RT/IsString");
			this["bool?"] = InteropCompiler.Create("RT/IsBool");
			this["int?"] = InteropCompiler.Create("RT/IsInt");
			this["double?"] = InteropCompiler.Create("RT/IsDouble");
			this["list?"] = InteropCompiler.Create("RT/IsList");
			this["nil?"] = InteropCompiler.Create("RT/IsNil");

			this["seq"] = InteropCompiler.Create("Seq/Seq_");
			this["first"] = InteropCompiler.Create("Seq/First");
			this["count"] = InteropCompiler.Create("Seq/Count");
			this["take"] = InteropCompiler.Create("Seq/Take");
			this["drop"] = InteropCompiler.Create("Seq/Drop");
			this["next"] = InteropCompiler.Create("Seq/Next");
			this["last"] = InteropCompiler.Create("Seq/Last");
			this["distinct"] = InteropCompiler.Create("Seq/Distinct");
			this["concat"] = InteropCompiler.Create("Seq/Concat");
			this["repeat"] = InteropCompiler.Create("Seq/Repeat");
			this["range"] = InteropCompiler.Create("Seq/Range");

			this["list"] = new Function(args => new List<object>((IEnumerable<object>)args).ToImmutableList());
			// new Compiler().Compile("(def + (fn [& args] (reduce RT/Add args)))").Invoke();
			new Compiler().Compile("(defn + [x y] (RT/Add x y))").Invoke();
			// this["+"] = InteropCompiler.Create("RT/Add");

			// this["+"] = new Function(args => RT.Add(args[0], args[1]));
			new Compiler().Compile("(def * (fn [& args] (reduce _* args)))").Invoke();
			// new Compiler().Compile("(defn concat ([coll] (Seq/Seq_ coll)) ([& args] (reduce Seq/Concat args)))").Invoke();

			this["print"] = new Function(args => { Console.WriteLine(args[0].Stringify()); return null; });
			this["read-line"] = new Function(args => Console.ReadLine());
			this["loop"] = new Loop();
			this["recur"] = new Recur();
			this["deftest"] = new DefTest();
		}

		public object this[string symbol]
		{
			get
			{
				var s = this;

				if (SymbolTable.TryGetValue(symbol, out var value))
					return value;

				while (s.Parent != null)
				{
					s = s.Parent;
					if (s.SymbolTable.TryGetValue(symbol, out var v))
						return v;
				}
				throw new System.Exception($"Unable to resolve symbol: {symbol} in this context");
			}
			set
			{
				SymbolTable[symbol] = value;
			}
		}

		public bool Contains(string symbol)
		{
			var s = this;
			if (SymbolTable.ContainsKey(symbol)) return true;
			while (s.Parent != null)
			{
				s = s.Parent;
				if (s.SymbolTable.ContainsKey(symbol)) return true;
			}
			return false;
		}

		public void Clear() => SymbolTable.Clear();
	}
}