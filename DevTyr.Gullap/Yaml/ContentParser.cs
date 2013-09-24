using System;
using System.Linq;
using System.Text.RegularExpressions;
using DevTyr.Gullap.Model;
using YamlDotNet.Dynamic;

namespace DevTyr.Gullap.Yaml
{
    public class ContentParser
    {
        public Page ParsePage(string content)
        {
            var splitted = ParseInternal(content);
            var parsed = new DynamicYaml(splitted[0]);
            return new Page(parsed, splitted[1].Trim());
        }

        public Post ParsePost(string content)
        {
            var splitted = ParseInternal(content);
            var parsed = new DynamicYaml(splitted[0]);
            return new Post(parsed, splitted[1].Trim());
        }

        private string[] ParseInternal(string content)
        {
            var splitted = Regex.Split(content, "^---" + Environment.NewLine, RegexOptions.Multiline);

            var first = splitted.FirstOrDefault();
            if (first != null && string.IsNullOrWhiteSpace(first))
            {
                splitted = splitted.Skip(1).ToArray();
            }

            if (splitted.Length != 2)
                throw new InvalidPageException();

            return splitted;
        }
    }
}
