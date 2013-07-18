# gullap

gullap is a [Mono](http://www.mono-project.com "Mono") based static website generator.

## Features

* Based on [Mono](http://www.mono-project.org "Mono") it works on Windows and Linux
* Supports Markdown markup
* Supports Mustache templating engine
* Main menu is fully built based on the source files
  * Source file with Markdown markup are rendered as local HTML files
	* Source files can define external Links (no markup)
* Support for sidebar menus
* Include HTML to your markup files
* Site structure generation

### Extensibility

There are two interfaces `IParser` (per default one for Markdown is implemented) and `ITemplater` (per default one for Nustache is implemented). They can be used to replace the defaults by other markup and templating engines.

## How To

First you have to compile the solution. For that, can either use [MonoDevelop](http://monodevelop.com/ "MonoDevelop") or Microsoft Visual Studio.

There are the following options to call gullap:

	GullapConsole -i my/Path/To/My/Site
	GullapConsole -g my/Path/To/My/Site

The argument `-i` generates the necessary structure within `my/Path/To/My/Site`. Therefore the target path has to exist. It is not created by gullap. The file structure looks like the following

	Site
	|- assets
	|- output
	|- pages
	|- templates

Copy all your necessary assets (CSS, JavaScript, Images) into `assets`. They will be copied to the `output` directory as they are. So, if you have sub directories they will be available also within the output directory. The `pages` directory contains your markup. Per default they should contain all your Markdown files (the extension of your files is unimportant, as all files are taken for the transformation). Put all your Nustache templates to the `templates` directory. 

After you filled up the structure with all necessary files, call `GullapConsole` with argument `-g` and the path to your site directory. Everything will be generated to the `output` directory.

## Default Markdown Meta Tags

Each Markdown file has to contain one or more meta tags. They are important for the transformation. Here's a list of supported meta tags:

	Title 			Title of the page
	Description		Description of the page
	Author			Author of the page
	Menu 			Definition of a new Menu this page belongs to
	MenuTitle 		Definition of a new menu
	MenuCategory	Category within your menu
	Sidebar 		Define a sidebar this page belongs to
	SidebarTitle 	If set this one is taken for sidebar generation, otherwise `MenuTitle`
	Template 		A specific template to use
	Link 			A link to a resource
	Date			Creation/update date of the page
	Directory		Output directory (relativ) for the page 

Not all meta tags have to be used. `Title`, `Description` and `Author` are only important if the page results in a real HTML transformation (this is not the case if `Link` is in use). The `Sidebar*` tags are only useful if you have a sidebar defined in your templates and access the exported variables.

There is one template that MUST exist: `page.template`. This is the default template. Use `Template` to override that.

> All of them have to be in a separate line, directly followed by `:` and the value.

## Variables available for templating

	content			The parsed content from your pages
	menu 			The complete menu tree
	title 			Title of the page
	description 	Description of the page
	author 			Author of the page
	sidebarheader	Header of the sidebar (defined by `Sidebar` meta tag)
	sidebaritems 	Items for your sidebar
	date		Date of the page

Most of them are single values, except `menu` and `sidebaritems`.

Here's an example for `menu`:

	<ul class="nav">
	  {{#menu.Items}}
	    {{#HasSubMenu}}
	      <li class="dropdown">
	        <a href="#" class="dropdown-toggle" data-toggle="dropdown">{{Name}} <b class="caret"></b></a>
	        <ul class="dropdown-menu">
	          {{#HasCategories}}
	            {{#Categories}}
	              <li class="nav-header">{{Name}}</li>
	              {{#SubItems}}
	                <li><a href="{{Link}}">{{Name}}</a></li>
	              {{/SubItems}}
	              <li class="divider"></li>
	            {{/Categories}}
	          {{/HasCategories}}
	          {{#SubItems}}
	            <li><a href="{{Link}}">{{Name}}</a></li>
	          {{/SubItems}}
	        </ul>
	      </li>
	    {{/HasSubMenu}}
	    {{^HasSubMenu}}
	      <li><a href="{{Link}}" title="{{Name}}">{{Name}}</a></li>
	    {{/HasSubMenu}}
	  {{/menu.Items}}
	</ul>

Here's an example for `sidebaritems`:

	<ul class="nav nav-list">
		<li class="nav-header">{{sidebarheader}}</li>
		{{#sidebaritems}}
			<li><a href="{{Link}}">{{SidebarTitle}}</a></li>
		{{/sidebaritems}}
	</ul>

## Examples

**Generate a menu entry for the root menu without a link**

	MenuTitle: Projects
	-----

**Generate a menu entry providing a link for the root menu**

	MenuTitle: News
	Link: http://news.devtyr.com
	-----

**Generating a sub menu for an existing root menu**

	Title: FastSearch - DevTyr
	Description: Ajax Search for BlogEngine.NET 
	Author: DevTyr / Norbert Eder
	Menu: Projects
	MenuTitle: FastSearch
	MenuCategory: BlogEngine.NET
	Sidebar: FastSearch
	SidebarTitle: Features
	Template: page_sidebar
	-----

This creates a sub menu called `FastSearch` right under root menu `Projects`. 

You can have a look at [DevTyr](http://devtyr.com). This is one of my sites and fully generated by gullap.

## Future

There are a lot of ideas to improve gallup. One of the next step is to provide a complete public sample integrating [Bootstrap from Twitter](http://getbootstrap.com "Bootstrap from Twitter") as a real world example.
