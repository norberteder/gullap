using System;
using System.Diagnostics;

namespace DevTyr.Gullap.GullapConsole
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            var cmdOptions = new Options();
            var parser = new CommandLine.Parser();
            if (parser.ParseArguments(args, cmdOptions))
            {
                if (cmdOptions.Verbose)
                    ConfigureTrace();
                
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
                    Console.WriteLine("Site [{0}] generated", cmdOptions.SitePath);
                }

                if (cmdOptions.GenerateSite)
                {
                    converter.ConvertAll();
                }

                if (!string.IsNullOrWhiteSpace(cmdOptions.FileToGenerate))
                {
                    converter.ConvertSingleFile(cmdOptions.FileToGenerate);
                }

                watch.Stop();

                Console.WriteLine("");
                Console.WriteLine("Finished in {0}", watch.Elapsed);
            }
            else
            {
                Console.WriteLine(cmdOptions.GetUsage());
                Environment.Exit(1);
            }
        }

        private static void ConfigureTrace()
        {
            var listener = new ConsoleTraceListener();
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
        }
    }
}