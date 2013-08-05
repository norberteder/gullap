using System;
using NUnit.Framework;

using DevTyr.Gullap;

namespace DevTyr.Gullap.Tests.With_Converter.For_SetParser
{
	[TestFixture]
	public class When_argument_is_null
	{
		[Test]
		public void Should_throw_argument_exception ()
		{
			var converter = new Converter (new ConverterOptions {
				SitePath = "any"
			});

			Assert.Throws<ArgumentException> (() => converter.SetParser (null));
		}

		[Test]
		public void Should_throw_exception_with_proper_message ()
		{
			var converter = new Converter (new ConverterOptions {
				SitePath = "any"
			});

			Assert.Throws<ArgumentException> (() => converter.SetParser (null), "No valid parser given");
		}
    }
}
