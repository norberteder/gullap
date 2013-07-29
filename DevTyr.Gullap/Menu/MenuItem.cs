using System.Collections.Generic;
using System.Linq;

namespace DevTyr.Gullap.Menu
{
	internal class MenuItem 
	{
		private readonly List<MenuItem> subItems = new List<MenuItem>();
		private readonly List<MenuCategory> categories = new List<MenuCategory>();
		private string sidebarTitle;

		public string Name { get; set; }
		public string Link { get; set; }
		public string Sidebar { get; set; }

		public string SidebarTitle { 
			get
			{
			    return string.IsNullOrWhiteSpace (sidebarTitle) ? Name : sidebarTitle;
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
		    return Categories.Count > 0 ? Categories.FirstOrDefault(category => category.Name == name) : null;
		}
	}
}

