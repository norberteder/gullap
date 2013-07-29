using System.Collections.Generic;

namespace DevTyr.Gullap.Parser
{
	public static class ParserKeys
	{
		public static string InfoEnd = "-----";
		
		public static List<KeyValuePair<string, string>> Mappings = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("Title", "Title:"),
			new KeyValuePair<string, string>("Description", "Description:"),
			new KeyValuePair<string, string>("Author", "Author:"),
			new KeyValuePair<string, string>("Template", "Template:"),
			new KeyValuePair<string, string>("Link", "Link:"),
			new KeyValuePair<string, string>("Menu", "Menu:"),
			new KeyValuePair<string, string>("MenuCategory", "MenuCategory:"),
			new KeyValuePair<string, string>("MenuTitle", "MenuTitle:"),
			new KeyValuePair<string, string>("Sidebar", "Sidebar:"),
			new KeyValuePair<string, string>("SidebarTitle", "SidebarTitle:"),
			new KeyValuePair<string, string>("Keywords", "Keywords:"),
			new KeyValuePair<string, string>("Date", "Date:"),
			new KeyValuePair<string, string>("Directory", "Directory:")
		};
	}
}

