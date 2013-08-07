using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevTyr.Gullap.Yaml;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_YamlFrontMatterSimpleParser.For_ParsingData
{
    [TestFixture]
    public class When_content_is_invalid
    {
        [Test]
        public void Should_throw_yamlfrontmatterparserexception()
        {
            var data = "invalid yaml front matter";
            var parser = new YamlFrontMatterSimpleParser();
            
            Assert.Throws<YamlFrontMatterParserException>(() => parser.ParseFrontMatterInto<object>(data));
        }

        [Test]
        public void Should_throw_yamlfrontmatterparserexception_when_having_empty_lines()
        {
            var sampleData = "---" + Environment.NewLine +
                             " " + Environment.NewLine +
                             "title: Test" + Environment.NewLine +
                             "---";
            var parser = new YamlFrontMatterSimpleParser();
            Assert.Throws<YamlFrontMatterParserException>(() => parser.ParseFrontMatterInto<TestPage>(sampleData));
        }
    }
}
