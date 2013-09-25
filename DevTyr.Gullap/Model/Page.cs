using System.Collections.Generic;
using YamlDotNet.Dynamic;

namespace DevTyr.Gullap.Model
{
    public class Page : ContentBase
    {
        private List<Page> categoryPages = new List<Page>();

        public Page(DynamicYaml dynamicYaml, string unparsedContent) 
            : base(dynamicYaml, unparsedContent)
        {
        }

        public string Description
        {
            get { return Meta.description; }
        }

        public List<Page> CategoryPages
        {
            get { return categoryPages; }
            set { categoryPages = value; }
        }
    }
}
