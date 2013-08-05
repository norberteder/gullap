using System;
using NUnit.Framework;

using DevTyr.Gullap;

namespace DevTyr.Gullap.Tests.With_Guard.For_NotNullOrEmpty
{
	[TestFixture]
	public class When_argument_is_null
	{
		[Test]
		public void Should_throw_argumentnullexception ()
		{
			Assert.Throws<ArgumentNullException> (() => Guard.NotNullOrEmpty (null, null));
		}

		[Test]
		public void Should_include_argument_name_in_exception_message ()
		{
			string what = null;
			Assert.Throws<ArgumentNullException> (() => Guard.NotNullOrEmpty (what, "what"), "what");
		}
    }
}
