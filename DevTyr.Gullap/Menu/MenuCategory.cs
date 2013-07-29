using System.Collections.Generic;

namespace DevTyr.Gullap.Menu
{
	internal class MenuCategory
	{
		private readonly List<MenuItem> subItems = new List<MenuItem>();

		public string Name { get; set; }

		public List<MenuItem> SubItems {
			get { return subItems; }
		}
	}
}

