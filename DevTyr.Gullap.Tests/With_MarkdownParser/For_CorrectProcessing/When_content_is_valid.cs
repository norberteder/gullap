using DevTyr.Gullap.Parser.Markdown;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_MarkdownParser.For_CorrectProcessing
{
    [TestFixture]
    public class When_content_is_valid
    {
        [Test]
        public void Should_return_parsed_Content()
        {
            const string content = "Test **Test**";
            const string expected = "<p>Test <strong>Test</strong></p>\n";

            var parser = new MarkdownParser();
            var parsedContent = parser.Parse(content);

            parsedContent.Should().NotBeNull();
            parsedContent.Should().BeEquivalentTo(expected);
        }
    }
}
