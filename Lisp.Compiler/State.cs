using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Lisp.Compiler
{
	public class State
	{
		public State ParentState { get; set; }
		public HashSet<string> Keywords { get; set; }
		public Dictionary<string, object> Bindings { get; set; }
		public Dictionary<string, IFn> Macros { get; set; }
		public static State Current = null;
		public static State Root = new State();

		// Root state constructor
		public State()
		{
			ParentState = null;
			Bindings = new Dictionary<string, object>();
			Macros = new Dictionary<string, IFn>();
			Keywords = new HashSet<string>();

			Keywords.Add("def");
			Keywords.Add("defn");
			Keywords.Add("defmacro");
			Keywords.Add("fn");
			Keywords.Add("let");
			Keywords.Add("if");
			Keywords.Add("loop");

			this["nil"] = null;
			this["true"] = true;
			this["false"] = false;
			// this["defmacro"] = new DefMacro();

			// this["list"] = new Function(args => new List<object>((IEnumerable<object>)args).ToImmutableList());

			this["if"] = new If();
			this["defn"] = new Defn();
			this["fn"] = InteropCompiler.Create("RT/Fn");
			this["let"] = new Let();
			this["def"] = InteropCompiler.Create("RT/Def");
			this["str"] = InteropCompiler.Create("RT/Str");
			this["apply"] = InteropCompiler.Create("RT/Apply");
			this["map"] = InteropCompiler.Create("RT/Map");
			this["reduce"] = new Reduce();
		}

		public State(State parent)
		{
			if (parent == null) parent = Root;
			ParentState = parent;
			Bindings = new Dictionary<string, object>();
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

			// new Compiler().Compile("(def + (fn [& args] (reduce RT/Add args)))").Invoke();
			this["+"] = new Function(async args => RT.Add(args[0], args[1]));
			// new Compiler().Compile("(def * (fn [& args] (reduce RT/Multiply args)))").Invoke();
			// new Compiler().Compile("(defn concat ([coll] (Seq/Seq_ coll)) ([& args] (reduce Seq/Concat args)))").Invoke();

			// this["print"] = new Function(args => { Console.WriteLine(args[0].Stringify()); return null; });
			// // this["read-line"] = new Function(args => Console.ReadLine());
			// this["loop"] = new Loop();
			// this["recur"] = new Recur();
			
			// this["async"] = new Function(args => RT.TestRange(args[0], args[1]));
			// this["interval"] = new Function(args => RT.Interval(args[0]));
		}

		public object this[string symbol]
		{
			get
			{
				var s = this;
				if (Bindings.ContainsKey(symbol)) return Bindings[symbol];
				while (s.ParentState != null)
				{
					s = s.ParentState;
					if (s.Bindings.ContainsKey(symbol)) return s.Bindings[symbol];
				}
				throw new System.Exception($"Unable to resolve symbol: {symbol} in this context");
			}
			set
			{
				Bindings[symbol] = value;
			}
		}

		public bool Contains(string symbol)
		{
			var s = this;
			if (Bindings.ContainsKey(symbol)) return true;
			while (s.ParentState != null)
			{
				s = s.ParentState;
				if (s.Bindings.ContainsKey(symbol)) return true;
			}
			return false;
		}

		public void Clear()
		{
			Bindings.Clear();
		}
	}
}