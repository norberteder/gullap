using System;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace DevTyr.Gullap.GullapConsole
{
    internal class Options
    {
        private string sitePath = Environment.CurrentDirectory;

        [Option('i', "initialize", Required = false, HelpText = "Initialize a new site")]
        public bool InitializeSite { get; set; }

        [Option('s', "sitepath", Required = false, HelpText = "Define the site path")]
        public string SitePath
        {
            get
            {
                return sitePath;
            }
            set
            {
                sitePath = value;
            }

        }
        [Option('g', "generate", Required = false, HelpText = "Generate whole site")]
        public bool GenerateSite { get; set; }
        [Option('f', "file", Required = false, HelpText = "File to generate")]
        public string FileToGenerate { get; set; }
        [Option('v', "verbose", Required = false, HelpText = "Print all messages")]
        public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText()
            {
                Heading = new HeadingInfo("Gullap | http://devtyr.com", Assembly.GetExecutingAssembly().GetName().Version.ToString()),
                Copyright = new CopyrightInfo("Norbert Eder", 2013),
                AddDashesToOption = true
            };
            help.AddPreOptionsLine(" ");
            help.AddPreOptionsLine("Usage: GullapConsole -option value");
            help.AddOptions(this);
            return help;
        }
    }
}
