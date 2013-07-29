using System.Collections.Generic;
using System.Linq;

namespace DevTyr.Gullap.Menu
{
	internal class MainMenu 
	{
		private readonly List<MenuItem> items = new List<MenuItem>();
		
		public List<MenuItem> Items {
			get { return items; }
		}

		public List<MenuItem> GetSidebarItems (string name)
		{
			var sidebarItems = new List<MenuItem> ();

		    if (string.IsNullOrWhiteSpace(name)) return sidebarItems;

		    foreach (var item in Items) {
		        GetSidebarItemsRecursive (item, name, sidebarItems);
		    }

		    return sidebarItems;
		}

		private void GetSidebarItemsRecursive (MenuItem item, string name, ICollection<MenuItem> sidebarItems)
		{
			if (!string.IsNullOrWhiteSpace(item.Sidebar) && item.Sidebar.ToUpperInvariant().Equals(name.ToUpperInvariant())) {
				sidebarItems.Add(item);
			}
			foreach (var subItem in item.SubItems) {
				GetSidebarItemsRecursive (subItem, name, sidebarItems);
			}

			foreach (var subItem in item.Categories.SelectMany(category => category.SubItems))
			{
			    GetSidebarItemsRecursive(subItem, name, sidebarItems);
			}
		}
	}
}

