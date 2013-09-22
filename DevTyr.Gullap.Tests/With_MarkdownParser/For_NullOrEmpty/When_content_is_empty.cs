using DevTyr.Gullap.Parser.Markdown;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_MarkdownParser.For_NullOrEmpty
{
    [TestFixture]
    public class When_content_is_empty
    {
        [Test]
        public void Should_not_throw_an_exception()
        {
            var content = string.Empty;
            var parser = new MarkdownParser();

            var result = parser.Parse(content);
            result.Should().BeEmpty();
        }
    }
}
