﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DevTyr.Gullap.Model;
using DevTyr.Gullap.Yaml;

namespace DevTyr.Gullap.IO
{
    public class WorkspaceInfo
    {
        public SitePaths Paths { get; private set; }

        public WorkspaceInfo(SitePaths paths)
        {
            Paths = paths;
        }

        public WorkspaceData GetPages()
        {
            if (!Directory.Exists(Paths.PagesPath))
                throw new DirectoryNotFoundException();

            var workspaceFiles = new WorkspaceData();

            var sourceFiles = Directory.GetFiles(Paths.PagesPath, "*.*", SearchOption.AllDirectories);
            var pageParser = new PageParser();

            foreach (var file in sourceFiles)
            {
                var content = File.ReadAllText(file);
                MetaPage metaPage = null;
                try
                {
                    Trace.TraceInformation("Reading file {0}", file);
                    var page = pageParser.Parse(content);

                    metaPage = new MetaPage(file) {Page = page};

                    metaPage.Page.Url = metaPage.GetTargetFileName(Paths).Replace(Paths.OutputPath, "").Replace('\\', '/');
                } 
                catch (InvalidPageException ipe)
                {
                    Trace.TraceWarning("Invalid YAML Front Matter for file " + file);
                    workspaceFiles.DoNotParseFiles.Add(file);
                }

                if (metaPage != null)
                {
                    workspaceFiles.YamlFiles.Add(metaPage);
                }
            }

            return workspaceFiles;
        }
    }
}
