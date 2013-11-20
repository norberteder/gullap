# gullap

gullap is a Microsoft .NET/[Mono](http://www.mono-project.com "Mono") based static website generator.

## Features

* Based on [Mono](http://www.mono-project.org "Mono") it works on Windows and Linux
* Supports Markdown markup
* Supports Mustache templating engine
* Supports [YAML](http://yaml.org/ "YAML") Front Matter
* Include HTML to your markup files
* Site structure generation
* Generate single files/posts
* Support of pages and posts
* Support of static HTML files

### Extensibility

There are two interfaces `IParser` (per default one for Markdown is implemented) and `ITemplater` (per default one for Nustache is implemented). They can be used to replace the defaults by other markup and templating engines.

## How To

[Download](http://devtyr.com/projects/gullap/gullap-download.html "Gullap Download") the binaries or compile the solution. For that, can either use [MonoDevelop](http://monodevelop.com/ "MonoDevelop") or Microsoft Visual Studio.

There are the following options to call gullap:

	GullapConsole -i my/Path/To/My/Site
	GullapConsole -g my/Path/To/My/Site
	GullapConsole -g
	GullapConsole -f my/Path/To/A/File

The argument `-i` generates the necessary structure within `my/Path/To/My/Site`. Therefore the target path has to exist. It is not created by gullap. The file structure looks like the following

	Site
	|- assets
	|- output
	|- pages
	|- posts
	|- templates

Copy all your necessary assets (CSS, JavaScript, Images) into `assets`. They will be copied to the `output` directory as they are. So, if you have sub directories they will be available also within the output directory. The `pages` directory contains your markup. Per default they should contain all your Markdown files (the extension of your files is unimportant, as all files are taken for the transformation). Put all your Nustache templates to the `templates` directory. 

After you filled up the structure with all necessary files, call `GullapConsole` with argument `-g` and the path to your site directory. Everything will be generated to the `output` directory. In case you use `-g` without giving a site path, the current directory will be used as the site path.

Using the parameter `-f` you can define a single file to be builded. The parameter needs a file path. This can either be an absolute path, a relative one or just the filename. In that case the first file matching the given name will be built, so take care if there are more having this name.

Use `GullapConsole -h` you can show help information. Add `-v` to your commands to get the full output.

## YAML Front Matter

Since version 1.1 Gullap uses a *YAML Front Matter* (as for example [Jekyll](http://jekyllrb.com/ "Jekyll") does. Each file that has a valid front matter will be processed. The following front matter attributes are currently available for pages:

	title 			Title of the page
	description		Description of the page
	author			Author of the page
	category 		The category of the page
	template 		A specific template to use
	date			Creation/update date of the page
	tags 			Keywords for your page
	draft			Defines whether this page is in draft mode or not
	filename		Target file name (e.g. rss.xml)

The same attributes are available for posts, except `description`.

There is one template that MUST exist: `page.template`. This is the default template. Use `template` to override that.

Here is an example of a valid front matter:

	---
	title: DEVTYR
	description: The Home of DevTyr
	author: DevTyr / Norbert Eder
	tags: 
	    - DevTyr
	    - Tools
	    - Projects
	    - Norbert Eder
	template: page
	---

Everything after the front matter is processed as page content.

> `.html` and `.htm` files having no valid front matter won't be parsed, just copied.

## Variables available for templating

Within your templates there are information on site level as well for the current page available. 

### site

The site section contains the following possibilities:

* config
* time
* pages
* pageCategories
* posts
* postCategories

`config` contains the site's configuration that can be defined in the file `config.yml`. This is an optional configuration. Currently only the site title can be set (attribute name `title`). `time` represents the date/time of the generation. `pages` contains all pages (the available attributes are the same as defined by the YAML front matter). `categories` is a dictionary containing the pages mapped to their category.

### current

This is the page that is currently rendered. It contains all attributes that are specified in the YAML front matter. In addition you have access to `url` and `categorypages`. `categorypages` is a list of all pages placed within the same category as the current one.

> Please note: It is currently not possible to define custom attributes. It won't break anything if you do it, but you can't access them within your template at the moment.

As an example see the template that is used to build the detail page including a sidebar of [devtyr.com](http://devtyr.com "DevTyr"):

	<div class="container">

	  <div class="row-fluid">
	      <div class="span2">
	      	  <ul class="nav nav-list">
	      	  	<li><span class="label label-primary">{{current.category}}</span></li>
	      	  </ul>
			  <ul class="nav nav-list">
			  	{{#current.categorypages}}
			  		<li><a href="{{url}}">{{title}}</a></li>
			  	{{/current.categorypages}}
			  </ul>
		  </div>

		  <div class="span8">
		  	{{{current.content}}}
		  </div>

	  </div>

	  <hr>

	</div>

> The complete templates and input files for [devtyr.com](http://www.devtyr.com "DevTyr") can be viewed [here](https://github.com/devtyr/devtyrcom "GitHub DevTyr Website Repository"). This should help you to get an overview of how Gullap works.
