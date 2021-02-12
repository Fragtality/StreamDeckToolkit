using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CONFIG = Microsoft.Extensions.Configuration;

namespace StreamDeckLib.Config
{
	public class ConfigurationBuilder : IDisposable
	{

		private ConfigurationBuilder(string[] args) {

		}

		private static ConfigurationBuilder Instance;

		public static ConfigurationBuilder BuildDefaultConfiguration(string[] args) {

			var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			Directory.SetCurrentDirectory(dir);

			var configuration = new CONFIG.ConfigurationBuilder()
													.AddJsonFile("logsettings.json") //CHANGED
													.Build();

			Log.Logger = new LoggerConfiguration()
									 .ReadFrom.Configuration(configuration)
									 .CreateLogger();

			Instance = new ConfigurationBuilder(args)
			{
				LoggerFactory = new LoggerFactory()
				.AddSerilog(Log.Logger)
			};
			
			return Instance;

		}

		public ILoggerFactory LoggerFactory { get; private set; }


		public void Dispose()
		{

		#if DEBUG
			if (Debugger.IsAttached)
			{
				// If a debugger is attached, give the developer a last chance to inspect
				// variables, state, etc. before the process terminates.
				Debugger.Break();
			}
#endif

		}
	}
}
