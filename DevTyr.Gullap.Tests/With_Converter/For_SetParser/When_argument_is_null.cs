using System;
using NUnit.Framework;

using DevTyr.Gullap;

namespace DevTyr.Gullap.Tests.With_Converter.For_SetParser
{
	[TestFixture]
	public class When_argument_is_null
	{
		[Test]
		public void Should_throw_argument_null_exception ()
		{
			var converter = new Converter (new ConverterOptions {
				SitePath = "any"
			});

			Assert.Throws<ArgumentNullException> (() => converter.SetParser (null));
		}

		[Test]
		public void Should_throw_exception_with_proper_message ()
		{
			var converter = new Converter (new ConverterOptions {
				SitePath = "any"
			});

			Assert.IsTrue (
				Assert.Throws<ArgumentNullException> (() => converter.SetParser (null))
					.Message.Contains ("No valid parser given")
			);
		}
    }
}
