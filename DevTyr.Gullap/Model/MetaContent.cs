
namespace DevTyr.Gullap.Model
{
    public class MetaContent
    {
        public MetaContent(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
        public Page Page { get; set; }
        public Post Post { get; set; }

        public string GetTemplate()
        {
            if (Page != null && !string.IsNullOrWhiteSpace(Page.Template))
                return Page.Template;
            if (Post != null && !string.IsNullOrWhiteSpace(Post.Template))
                return Post.Template;
            return null;
        }
		
		public string GetOverriddenFileName() 
		{
			if (Page != null && !string.IsNullOrWhiteSpace(Page.FileName))
				return Page.FileName;
			if (Post != null && !string.IsNullOrWhiteSpace(Post.FileName))
				return Post.FileName;
			return null;
		}
		
        public bool HasValidTemplate()
        {
            return !string.IsNullOrWhiteSpace(GetTemplate());
        }
    }
}
