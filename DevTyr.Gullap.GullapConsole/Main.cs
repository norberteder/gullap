using System;
using System.IO;
using System.Diagnostics;

namespace DevTyr.Gullap.GullapConsole
{
	class Options {
		public bool InitializeSite { get; set; }
		public string SitePath { get; set; }
		public bool GenerateSite { get; set; }
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			ShowInfo ();

			var cmdOptions = new Options ();

			foreach (var arg in args) {
				if (arg == "-i") {
					cmdOptions.InitializeSite = true;
					continue;
				}
				if (arg == "-g") {
					cmdOptions.GenerateSite = true;
					continue;
				}
				if (Directory.Exists (arg)) {
					cmdOptions.SitePath = arg;
				} else {
					Console.WriteLine ("Site directory [" + arg + "] must be created manually");
				}
			}

			if (string.IsNullOrWhiteSpace (cmdOptions.SitePath) || (!cmdOptions.GenerateSite && !cmdOptions.InitializeSite)) {
				ShowHelp ();
				Environment.Exit (1);
			}

			var options = new ConverterOptions
			{
				SitePath = cmdOptions.SitePath
			};
			
			var watch = new Stopwatch ();
			watch.Start ();
			
			var converter = new Converter (options);

			ConverterResult result = null;
			if (cmdOptions.GenerateSite) {
				result = converter.ConvertAll ();
			} else if (cmdOptions.InitializeSite) {
				converter.InitializeSite ();
				Console.WriteLine("Site [" + cmdOptions.SitePath + "] generated");
			}
			
			watch.Stop ();

			if (result != null) {
				if (result.HasError) {
					Console.WriteLine (result.ErrorMessage);
				} else {
					if (result.SuccessMessages != null) {
						foreach (var msg in result.SuccessMessages)
							Console.WriteLine (msg);
					}
				}
			}
			Console.WriteLine();
			Console.WriteLine("Finished in {0}", watch.Elapsed);
		}

	    private static void ShowHelp ()
		{
			Console.WriteLine ("GullapConsole -i [SitePath]\t\tInitialize Site");
			Console.WriteLine ("GullapConsole -g [SitePath]\t\tGenerate Site");
		}

	    private static void ShowInfo() {
			Console.WriteLine ("DevTyr Gullap");
			Console.WriteLine ("http://devtyr.com");
		}
	}
}
