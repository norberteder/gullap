using System.Collections.Generic;
using YamlDotNet.Dynamic;

namespace DevTyr.Gullap.Model
{
    public class Post : ContentBase
    {
        private List<Post> categoryPosts = new List<Post>();

        public Post(DynamicYaml dynamicYaml, string unparsedContent) 
            : base(dynamicYaml, unparsedContent)
        {
        }

        public List<Post> CategoryPosts
        {
            get { return categoryPosts; }
            set { categoryPosts = value; }
        }
    }
}
