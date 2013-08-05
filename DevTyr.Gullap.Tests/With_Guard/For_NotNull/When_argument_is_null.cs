using System;
using NUnit.Framework;

using DevTyr.Gullap;

namespace DevTyr.Gullap.Tests.With_Guard.For_NotNull
{
	[TestFixture]
	public class When_argument_is_null
	{
		[Test]
		public void Should_throw_argumentnullexception ()
		{
			Assert.Throws<ArgumentNullException> (() => Guard.NotNull (null, null));
		}

		[Test]
		public void Should_include_argument_name_in_exception_message ()
		{
			object what = null;
			Assert.Throws<ArgumentNullException> (() => Guard.NotNull (what, "what"), "what");
		}
    }
}
