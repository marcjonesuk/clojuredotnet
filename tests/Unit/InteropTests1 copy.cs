using System;
using System.Collections.Generic;
using System.Linq;
using marcclojure;
using marcclojure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace tests
{
	[TestClass]
	public class IfTests : BaseTest
	{
		IFn target = new IfFn();
		IFn _true = new Function(_ => true);
		IFn _false = new Function(_ => false);
		IFn _zero = new Function(_ => 0);
		IFn _one = new Function(_ => 1);

		private void Test(object expected, object[] args = null)
		{
			if (args == null) args = new object[0];
			var result = target.Invoke(args);
			Assert.AreEqual(expected, result);
		}

		private void TestThrows<T>(object[] args = null)
		{
			if (args == null) args = new object[0];
			try
			{
				var result = target.Invoke(args);
			}
			catch (Exception e)
			{
				Assert.AreEqual(typeof(T), e.GetType());
			}
		}

		[TestMethod] public void If_1() { Test(1, new object[] { _true, _one, _zero }); }
		[TestMethod] public void If_2() { Test(0, new object[] { _false, _one, _zero }); }
		[TestMethod] public void If_3() { Test(1, new object[] { _true, _one }); }
		[TestMethod] public void If_4() { Test(null, new object[] { _false, _one }); }
		[TestMethod] public void If_5() { TestThrows<ArityException>(new object[] { }); }
		[TestMethod] public void If_6() { TestThrows<ArityException>(new object[] { _true }); }
		[TestMethod] public void If_7() { TestThrows<ArityException>(new object[] { _true, _true, _true, _true }); }
		
		[TestMethod]
		public void If_8()
		{
			var mock1 = new Mock<IFn>();
			var mock2 = new Mock<IFn>();
			Test(null, new object[] { _true, mock1.Object, mock2.Object });
			mock1.Verify(m => m.Invoke(), Times.Once);
			mock2.Verify(m => m.Invoke(), Times.Never);
		}
		
		[TestMethod]
		public void If_9()
		{
			var mock1 = new Mock<IFn>();
			var mock2 = new Mock<IFn>();
			Test(null, new object[] { _false, mock1.Object, mock2.Object });
			mock1.Verify(m => m.Invoke(), Times.Never);
			mock2.Verify(m => m.Invoke(), Times.Once);
		}
	}
}