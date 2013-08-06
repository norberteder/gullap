using System;
using DevTyr.Gullap.Parser.Markdown;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_MarkdownParser.For_NullOrEmpty
{
    [TestFixture]
    public class When_content_is_null
    {
        [Test]
        public void Should_throw_argumentnullexception()
        {
            var parser = new MarkdownParser();
            Assert.Throws<ArgumentNullException>(() => parser.Parse(null)); 
        }
    }
}
