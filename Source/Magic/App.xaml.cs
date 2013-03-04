using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Magic
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			Dispatcher.UnhandledException += (sender, args) => LogError(args.Exception);
		}

		private void LogError(Exception exception)
		{
			var installDir = new DirectoryInfo(Assembly.GetEntryAssembly().Location).FullName;
			var logFile = Path.Combine(installDir, "Crash.txt");
			File.WriteAllText(logFile, exception.ToString());
		}
	}
}
