using System;
using System.Collections.Generic;

namespace DevTyr.Gullap.Menu
{
	internal class MenuItem 
	{
		private List<MenuItem> subItems = new List<MenuItem>();
		private List<MenuCategory> categories = new List<MenuCategory>();
		private string sidebarTitle = null;

		public string Name { get; set; }
		public string Link { get; set; }
		public string Sidebar { get; set; }

		public string SidebarTitle { 
			get {
				if (string.IsNullOrWhiteSpace (sidebarTitle))
					return Name;
				else
					return sidebarTitle;
			}
			set {
				sidebarTitle = value;
			}
		}

		public bool HasSubMenu { get { return SubItems.Count > 0 || Categories.Count > 0; } }
		public bool HasCategories { get { return Categories.Count > 0; } }
		public string OriginalFilePath { get; set; }
		
		public List<MenuItem> SubItems {
			get { return subItems; }
		}

		public List<MenuCategory> Categories {
			get { return categories; }
		}

		public MenuCategory GetCategory (string name)
		{
			if (Categories.Count > 0) {
				foreach(var category in Categories) {
					if (category.Name == name)
						return category;
				}
			}
			return null;
		}
	}
}

