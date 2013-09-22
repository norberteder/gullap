using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DevTyr.Gullap.GullapConsole
{
    internal class Options
    {
        public bool InitializeSite { get; set; }
        public string SitePath { get; set; }
        public bool GenerateSite { get; set; }
    }

    internal class MainClass
    {
        public static void Main(string[] args)
        {
            ShowInfo();

            ConfigureTrace();

            var cmdOptions = GenerateOptionsFromArguments(args);

            if (string.IsNullOrWhiteSpace(cmdOptions.SitePath) ||
                (!cmdOptions.GenerateSite && !cmdOptions.InitializeSite))
            {
                ShowHelp();
                Environment.Exit(1);
            }

            var options = new ConverterOptions
            {
                SitePath = cmdOptions.SitePath,
                SiteConfiguration = SiteConfigurationParser.LoadSiteConfiguration("config.yml")
            };

            var watch = new Stopwatch();
            watch.Start();

            var converter = new Converter(options);

            if (cmdOptions.InitializeSite)
            {
                converter.InitializeSite();
                Trace.TraceInformation("Site [{0}] generated", cmdOptions.SitePath);
            }

            if (cmdOptions.GenerateSite)
            {
                converter.ConvertAll();
            }

            watch.Stop();

            Trace.WriteLine("");
            Trace.TraceInformation("Finished in {0}", watch.Elapsed);
        }

        private static Options GenerateOptionsFromArguments(IEnumerable<string> args)
        {
            var cmdOptions = new Options();

            foreach (string arg in args)
            {
                if (arg == "-i")
                {
                    cmdOptions.InitializeSite = true;
                    continue;
                }
                if (arg == "-g")
                {
                    cmdOptions.GenerateSite = true;
                    continue;
                }
                if (Directory.Exists(arg))
                {
                    cmdOptions.SitePath = arg;
                }
                else
                {
                    Trace.TraceInformation("Site directory [{0}] must be created manually", arg);
                }
            }

            if (string.IsNullOrWhiteSpace(cmdOptions.SitePath))
                cmdOptions.SitePath = Environment.CurrentDirectory;

            return cmdOptions;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("GullapConsole -i [SitePath]  Initialize Site");
            Console.WriteLine("GullapConsole -g [SitePath]  Generate Site");
            Console.WriteLine("GullapConsole -g             Generate Site [current location]");
        }

        private static void ShowInfo()
        {
            Console.WriteLine("DevTyr Gullap {0} | {1}", Assembly.GetExecutingAssembly().GetName().Version, "http://devtyr.com");
            Console.WriteLine();
        }

        private static void ConfigureTrace()
        {
            var listener = new ConsoleTraceListener();
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
        }
    }
}