// using System.Collections.Generic;
// using System.Linq;
// using marcclojure;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Moq;

// namespace tests
// {
// 	public class MockObject
// 	{
// 		public static string Static() => "Static";
// 		public static string Static(int x) => x.ToString();
// 		public string Property => "Property";
// 		public string Instance() => "Instance";
// 		public string Instance(int x) => $"Instance_{x}";
// 		public string Instance(int x, string y) => $"Instance_{x}_{y}";
// 	}

// 	[TestClass]
// 	public class Interop1Tests : BaseTest
// 	{
// 		public void Interop_Test(string target, string expected, object[] args = null)
// 		{
// 			if (args == null) args = new object[0];
// 			var result = (new InteropFn(target)).Invoke((new object[] { new MockObject() }).Union(args).ToArray());
// 			Assert.AreEqual(expected, result);
// 		}

// 		[TestMethod]
// 		public void Interop_Instance_Method()
// 		{
// 			var method = "Instance";
// 			var args = new object[] {};
// 			var expected = "Instance";
// 			Interop_Test(method, expected, args);
// 		}

// 		[TestMethod]
// 		public void Interop_Instance_Method_2()
// 		{
// 			var method = "Instance";
// 			var args = new object[] { 5 };
// 			var expected = "Instance_5";
// 			Interop_Test(method, expected, args);
// 		}

// 		[TestMethod]
// 		public void Interop_Instance_Method_3()
// 		{
// 			var method = "Instance";
// 			var args = new object[] { 5, "hello" };
// 			var expected = "Instance_5_hello";
// 			Interop_Test(method, expected, args);
// 		}

// 		[TestMethod]
// 		public void Interop_Property()
// 		{
// 			var target = "-Property";
// 			var expected = "Property";
// 			Interop_Test(target, expected);
// 		}

// 		[TestMethod]
// 		public void Interop_Static_Property()
// 		{
// 			var target = "MockObject/Static";
// 			var expected = "Static";
// 			Interop_Test(target, expected);
// 		}
// 	}
// }