using System;
using marcclojure;

namespace tests
{
	public class BaseTest
	{
		public void AssertThat(string code, object expected)
		{
			var tokeniser = new Tokeniser();
			var compiler = new Reader();
			var result = compiler.Compile(code).Invoke();
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
		}

		public void AssertThrows(string code, Type expectedExceptionType)
		{
			var tokeniser = new Tokeniser();
			var compiler = new Reader();
			try
			{
				var result = compiler.Compile(code).Invoke();
			}
			catch (Exception e)
			{
				Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedExceptionType, e.GetType());
				return;
			}
			throw new Exception("Did not throw an exception");
		}
	}
}