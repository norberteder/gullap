using System;
using NUnit.Framework;

using DevTyr.Gullap;

namespace DevTyr.Gullap.Tests.With_Guard.For_NotNullOrEmpty
{
	[TestFixture]
	public class When_argument_is_not_null_or_empty
	{
		[Test]
		public void Should_pass ()
		{
			Guard.NotNullOrEmpty ("Test", null);
			Assert.Pass ();
		}
	}
}
