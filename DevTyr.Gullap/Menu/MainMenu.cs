using System;
using System.Collections.Generic;

namespace DevTyr.Gullap.Menu
{
	internal class MainMenu 
	{
		private List<MenuItem> items = new List<MenuItem>();
		
		public List<MenuItem> Items {
			get { return items; }
		}

		public List<MenuItem> GetSidebarItems (string name)
		{
			List<MenuItem> sidebarItems = new List<MenuItem> ();

			if (!string.IsNullOrWhiteSpace (name)) {
				foreach (var item in Items) {
					GetSidebarItemsRecursive (item, name, sidebarItems);
				}
			}

			return sidebarItems;
		}

		private void GetSidebarItemsRecursive (MenuItem item, string name, List<MenuItem> sidebarItems)
		{
			if (!string.IsNullOrWhiteSpace(item.Sidebar) && item.Sidebar.ToUpperInvariant().Equals(name.ToUpperInvariant())) {
				sidebarItems.Add(item);
			}
			foreach (var subItem in item.SubItems) {
				GetSidebarItemsRecursive (subItem, name, sidebarItems);
			}

			foreach (var category in item.Categories) {
				foreach(var subItem in category.SubItems) {
					GetSidebarItemsRecursive(subItem, name, sidebarItems);
				}
			}
		}
	}
}

