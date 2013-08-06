using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevTyr.Gullap.Parser.Markdown;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_MarkdownParser.For_NullOrEmpty
{
    [TestFixture]
    public class When_content_is_empty
    {
        [Test]
        public void Should_throw_argumentexception()
        {
            var content = string.Empty;
            var parser = new MarkdownParser();
            Assert.Throws<ArgumentException>(() => parser.Parse(content));  
        }
    }
}
