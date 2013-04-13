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
			var installDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
			var logFile = Path.Combine(installDir, "Crash.txt");
			File.WriteAllText(logFile, exception.ToString());
		}
	}
}
