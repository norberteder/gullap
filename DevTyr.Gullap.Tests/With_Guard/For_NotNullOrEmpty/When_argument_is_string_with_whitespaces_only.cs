using System;
using NUnit.Framework;

using DevTyr.Gullap;

namespace DevTyr.Gullap.Tests.With_Guard.For_NotNullOrEmpty
{
	
	[TestFixture]
	public class When_argument_is_string_with_whitespaces_only
	{
		[Test]
		public void Should_throw_argumentexception ()
		{
			Assert.Throws<ArgumentException> (() => Guard.NotNullOrEmpty (" ", " "));
		}
	}
}
