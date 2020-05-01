using System.Collections.Generic;
using System.Linq;
using marcclojure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class HashMapTests : BaseTest
	{
		[TestMethod] public void HashMap_1() { AssertThat("(str (hash-map :key1 1 :key2 2))", "{ :key1 1, :key2 2 }"); }
		[TestMethod] public void HashMap_2() { AssertThat("(str {:key1 1 :key2 2})", "{ :key1 1, :key2 2 }"); }
		[TestMethod] public void HashMap_3() { AssertThat("(def person {:name 'Steve' :age 24 :salary 7886 :company 'Acme'})(:name person)", "Steve"); }
		[TestMethod] public void HashMap_4() { AssertThat("(:b (apply hash-map [:a 1 :b 2]))", 2.0); }
		[TestMethod] public void HashMap_5() { AssertThat("({:a 1, :b 2} :a)", 1.0); }
		[TestMethod] public void HashMap_6() { AssertThat("({:a 1, :b 2} :qwerty)", null); }
		[TestMethod] public void HashMap_7() { AssertThat("(str ({:a 1, :b 2} :qwerty :uiop))", ":uiop"); }
		[TestMethod] public void HashMap_8() { AssertThat("(str (:key1 (hash-map :key1 1 :key1 2)))", "2"); }
		[TestMethod] public void HashMap_9() { AssertThat("(def a :key1)({a 5} :key1)", 5.0); }
	}
}