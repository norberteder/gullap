using System;
using System.Collections;
using System.Collections.Generic;
using DevTyr.Gullap.Yaml;
using FluentAssertions;
using NUnit.Framework;

namespace DevTyr.Gullap.Tests.With_PageParser.Acceptance_Criteria
{
	[TestFixture]
	public class Case_of_complex_front_matter
	{
		readonly PageParser parser = new PageParser ();
		readonly SampleYamlFrontMatter sample = new SampleYamlFrontMatter {
			Text = @"---
title: 'NOW: Colons!'
tags:
  - simple
  - effective
  - 'good looking'
date: !!timestamp 2011-05-23 16:00:30
dictionary:
  key: value
  list:
    - 1
    - 2
    - 3
---
Yes!
",
			Title = "NOW: Colons!",
			Content = "Yes!",
			Date = new DateTime (2011, 5, 23, 16, 0, 30),
			Tags = new List<string> {
				"simple",
				"effective",
				"good looking"
			},
			Dictionary = new Dictionary<string, object> {
				{ "key", "value" },
				{ "list", new List<int> { 1, 2, 3 } }
			}
		};

		private class SampleYamlFrontMatter 
		{
			public string Text { get; set; }
			public string Title { get; set; }
			public string Content { get; set; }
			public DateTime Date { get; set; }
			public IList Tags { get; set; }
			public IDictionary Dictionary { get; set; }
		}

		[Test]
		public void Can_parse_content()
		{
		    var page = parser.Parse(sample.Text);

		    page.Title
		        .Should()
		        .BeEquivalentTo(sample.Title);

            page.Date
                .Should()
                .Be(sample.Date);

            page.Tags
                .ShouldAllBeEquivalentTo(sample.Tags);
			
            page.Content
				.Should ()
				.BeEquivalentTo (sample.Content);
		}
	}    
}
