# RELEASE NOTES

**1.2.0**

- Markdown and HTML files (html, htm) can be mixed. HTML files won't be parsed, just copied.

**1.1.0**

- Replaced current file header with a Yaml Front Matter (breaking!!)
- Removed MenuBuilder (this can be done via templating now)
- Available variables for templating changed, please see documentation
- If there is no command argument given for the site path, the current path is used
- Optional site configuration (Yaml)

**1.0.1**

- Added support of Date

**1.0**

- Support Markdown (default, possible to extend)
- Support Nustache (a Mustache port, templating engine) (default, possible to extend)
- Automatic menu generation, supports sidebars as well

