using System;
using System.Diagnostics;
using DevTyr.Gullap.Templating;

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
				
				if (!string.IsNullOrWhiteSpace(options.SiteConfiguration.Templater)) 
				{
					var templaterType = Type.GetType(options.SiteConfiguration.Templater, false, true);
					if (templaterType != null) 
					{
						var templater = Activator.CreateInstance(templaterType) as ITemplater;
						if (templater != null) 
						{
							converter.SetTemplater(templater);
						}
					}
				}
				
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