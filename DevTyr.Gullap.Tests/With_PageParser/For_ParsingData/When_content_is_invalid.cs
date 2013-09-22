using System;
using DevTyr.Gullap.Yaml;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_PageParser.For_ParsingData
{
    [TestFixture]
    public class When_content_is_invalid
    {
        [Test]
        public void Should_throw_invalidpageexception()
        {
            const string data = "invalid yaml front matter";
            var parser = new PageParser();
            
            Assert.Throws<InvalidPageException>(() => parser.Parse(data));
        }

        [Test]
        public void Should_throw_invalidpageexception_when_having_empty_lines()
        {
            var sampleData = "---" + Environment.NewLine +
                             " " + Environment.NewLine +
                             "title: Test" + Environment.NewLine +
                             "---";
            var parser = new PageParser();
            Assert.Throws<InvalidPageException>(() => parser.Parse(sampleData));
        }
    }
}
