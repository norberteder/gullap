namespace DevTyr.Gullap.Model
{
    public class SiteConfiguration
    {
		private string templater = "nustache";
		
        public string Title { get; set; }
		public string Templater 
		{
			get { return templater; }
			set { templater = value; }
		}
    }
}
