using System.IO;
using System.Collections.Generic;
using System.Linq;
using DevTyr.Gullap.Parser;

namespace DevTyr.Gullap.Menu
{
	internal class MenuBuilder
	{
	    public MainMenu Build (List<ParsedFileInfo> infos)
		{
			var menu = new MainMenu ();

			var rootElements = GetRootElements (infos);

			foreach (var rooty in rootElements) {
				var item = new MenuItem
				{
				    Name = string.IsNullOrWhiteSpace(rooty.MenuTitle) ? rooty.Title : rooty.MenuTitle,
				    Link = rooty.Link ?? "/" + Path.GetFileName(rooty.TargetFileName),
				    Sidebar = rooty.Sidebar,
				    SidebarTitle = rooty.SidebarTitle
				};
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
				var item = new MenuItem
				{
				    Name = string.IsNullOrWhiteSpace(child.MenuTitle) ? child.Title : child.MenuTitle,
				    Link = child.Link ?? "/" + child.TargetFileName.Replace("\\", "/"),
				    Sidebar = child.Sidebar,
				    SidebarTitle = child.SidebarTitle
				};

			    if (!string.IsNullOrWhiteSpace(child.MenuCategory)) {
					var category = menuItem.GetCategory(child.MenuCategory);
					if (category == null)
					{
						category = new MenuCategory { Name = child.MenuCategory };
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

		private IEnumerable<ParsedFileInfo> GetRootElements (IEnumerable<ParsedFileInfo> infos)
		{
		    return infos.Where(info => string.IsNullOrWhiteSpace(info.Menu)).ToList();
		}

	    private IEnumerable<ParsedFileInfo> GetElementsWithParent (string parentMenu, IEnumerable<ParsedFileInfo> infos)
	    {
	        return infos.Where(info => !string.IsNullOrWhiteSpace(info.Menu) && info.Menu.ToUpperInvariant().Equals(parentMenu.ToUpperInvariant())).ToList();
	    }
	}
}

