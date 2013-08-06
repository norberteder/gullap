using System;
using DevTyr.Gullap.Parser.Markdown;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_MarkdownParser.For_CorrectProcessing
{
    [TestFixture]
    public class When_content_is_valid
    {
        [Test]
        public void Should_return_correct_parsedfileinfo()
        {
            const string expectedTitle = "The Home of DevTyr";
            const string expectedDescription = "The Home of DevTyr";
            const string expectedAuthor = "Norbert Eder";

            var content = "Title: The Home of DevTyr" + Environment.NewLine
                          + "Description: The Home of DevTyr" + Environment.NewLine
                          + "Author: Norbert Eder" + Environment.NewLine
                          + "-----";

            var parser = new MarkdownParser();
            var info = parser.Parse(content);

            info.Should().NotBeNull();
            info.Title.Should().Be(expectedTitle);
            info.Description.Should().Be(expectedDescription);
            info.Author.Should().Be(expectedAuthor);
        }
    }
}
