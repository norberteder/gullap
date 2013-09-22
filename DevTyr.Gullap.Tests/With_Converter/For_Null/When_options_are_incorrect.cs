using System;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_Converter.For_Null
{
    [TestFixture]
    public class When_options_are_incorrect
    {
        [Test]
        public void Should_throw_argumentnullexception_for_null_parameter()
        {
            Assert.Throws<ArgumentNullException>(() => new Converter(null));
        }

        [Test]
        public void Should_throw_argumentnullexception_for_options_with_null_infos()
        {
            Assert.Throws<ArgumentNullException>(() => new Converter(new ConverterOptions()));
        }

        [Test]
        public void Should_throw_argumentexecption_for_empty_sitepath_in_options()
        {
            Assert.Throws<ArgumentException>(() => new Converter(new ConverterOptions {SitePath = ""}));
        }
    }
}
