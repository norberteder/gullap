namespace DevTyr.Gullap.Templating
{
	public interface ITemplater
	{
		string Transform (string templatePath, string template, object content);
	}
}

