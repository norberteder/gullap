using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp;
using YamlDotNet.Dynamic;

namespace DevTyr.Gullap.Model
{
    public class Page
    {
        private readonly dynamic yaml;
        private List<Page> categoryPages = new List<Page>();

        public Page(DynamicYaml dynamicYaml, string unparsedContent)
        {
            yaml = dynamicYaml;
            Content = unparsedContent;
        }

        public string Title
        {
            get { return yaml.title; }
        }

        public string Description
        {
            get { return yaml.description; }
        }

        public string Category
        {
            get { return yaml.category; }
        }

        public string Author
        {
            get { return yaml.author; }
        }

        public DateTime Date
        {
            get { return DateTime.Parse((string)yaml.date); }
        }

        public List<string> Tags
        {
            get { return yaml.tags; }
        }

        public string Template
        {
            get { return yaml.template; }
        }

        public bool Draft
        {
            get
            {
                var draft = (string)yaml.draft;
                if (string.IsNullOrWhiteSpace(draft))
                    return false;
                return Boolean.Parse(draft);
            }
        }

        public string Content { get; set; }

        public string Url { get; set; }

        public List<Page> CategoryPages
        {
            get { return categoryPages; }
            set { categoryPages = value; }
        } 

        public object Meta
        {
            get
            {
                return ((DynamicYaml)yaml);
            }
        }
    }
}
