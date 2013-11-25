namespace DevTyr.Gullap.Templating
{
	public interface ITemplater
	{
		string Transform (string template, object content);
	}
}

