using System;
using System.IO;
using System.Collections.Generic;
using DevTyr.Gullap.Parser;

namespace DevTyr.Gullap.Menu
{
	internal class MenuBuilder
	{
		public MenuBuilder ()
		{
		}

		public MainMenu Build (List<ParsedFileInfo> infos)
		{
			MainMenu menu = new MainMenu ();

			var rootElements = GetRootElements (infos);

			foreach (var rooty in rootElements) {
				MenuItem item = new MenuItem ();
				item.Name = string.IsNullOrWhiteSpace(rooty.MenuTitle) ? rooty.Title : rooty.MenuTitle;
				item.Link = rooty.Link ?? Path.GetFileName (rooty.TargetFileName);
				item.Sidebar = rooty.Sidebar;
				item.SidebarTitle = rooty.SidebarTitle;
				menu.Items.Add (item);
			}

			foreach (var menuItem in menu.Items) {
				BuildRecursive(menuItem, infos);
			}

			return menu;
		}

		public void BuildRecursive(MenuItem menuItem, List<ParsedFileInfo> infos) {
			var children = GetElementsWithParent(menuItem.Name, infos);
			
			foreach(var child in children) {
				MenuItem item = new MenuItem();
				item.Name = string.IsNullOrWhiteSpace(child.MenuTitle) ? child.Title : child.MenuTitle;
				item.Link = child.Link ?? Path.GetFileName(child.TargetFileName);
				item.Sidebar = child.Sidebar;
				item.SidebarTitle = child.SidebarTitle;
				
				if (!string.IsNullOrWhiteSpace(child.MenuCategory)) {
					MenuCategory category = menuItem.GetCategory(child.MenuCategory);
					if (category == null)
					{
						category = new MenuCategory() { Name = child.MenuCategory };
						category.SubItems.Add(item);
						menuItem.Categories.Add(category);
					} else {
						category.SubItems.Add(item);
					}
					
				} else {
					menuItem.SubItems.Add(item);
				}
				BuildRecursive(item, infos);
			}
		}

		private List<ParsedFileInfo> GetRootElements (List<ParsedFileInfo> infos)
		{
			List<ParsedFileInfo> rootElements = new List<ParsedFileInfo> ();

			foreach (var info in infos) {
				if (string.IsNullOrWhiteSpace(info.Menu)) {
					rootElements.Add(info);
				}
			}
			return rootElements;
		}

		private List<ParsedFileInfo> GetElementsWithParent (string parentMenu, List<ParsedFileInfo> infos)
		{
			List<ParsedFileInfo> children = new List<ParsedFileInfo> ();

			foreach (var info in infos) {
				if (!string.IsNullOrWhiteSpace(info.Menu) && info.Menu.ToUpperInvariant().Equals(parentMenu.ToUpperInvariant())) 
				{
					children.Add(info);
				}
			}
			return children;
		}
	}
}

