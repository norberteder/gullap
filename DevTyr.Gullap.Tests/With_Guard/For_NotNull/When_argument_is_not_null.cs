using System;
using NUnit.Framework;

using DevTyr.Gullap;

namespace DevTyr.Gullap.Tests.With_Guard.For_NotNull
{
	[TestFixture]
	public class When_argument_is_not_null
	{
		[Test]
		public void Should_pass ()
		{
			Guard.NotNull (new object (), null);
			Assert.Pass ();
		}
	}
}
