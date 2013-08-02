using System;
using DevTyr.Gullap.Parser.Markdown;
using NUnit.Framework;
using FluentAssertions;

namespace DevTyr.Gullap.Tests.Parser
{
    [TestFixture]
    public class MarkdownParserTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_Empty_Content()
        {
            var content = string.Empty;
            var parser = new MarkdownParser();
            parser.Parse(content);            
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Parse_Null_Content()
        {
            var parser = new MarkdownParser();
            parser.Parse(null);            
        }

        [Test]
        public void Get_FileParseInfo_From_Valid_Content()
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
