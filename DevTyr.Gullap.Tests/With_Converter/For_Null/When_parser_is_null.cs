using System;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_Converter.For_Null
{
    [TestFixture]
    public class When_parser_is_null
    {
        [Test]
        public void Should_throw_argumentnullexception()
        {
            var options = new ConverterOptions {SitePath = "Test"};
            var converter = new Converter(options);
            Assert.Throws<ArgumentNullException>(() => converter.SetParser(null), "converter");
        }
    }
}
