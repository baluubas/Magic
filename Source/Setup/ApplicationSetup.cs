
using System.Windows;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Magic.Setup
{
	public class SetupApplication : BootstrapperApplication
	{		
		static public Dispatcher BootstrapperDispatcher { get; private set; }
	
		protected override void Run()
		{
			Engine.Log(LogLevel.Verbose, "Launching Magic Setup UX");
			BootstrapperDispatcher = Dispatcher.CurrentDispatcher;
			
			Window window = GetSetupWindow();
			
			Engine.Detect();
			window.Closed += (sender, e) => BootstrapperDispatcher.InvokeShutdown();
			window.Show();
			Dispatcher.Run();
			Engine.Quit(0);
		}

		private Window GetSetupWindow()
		{
			if(Command.Action == LaunchAction.Uninstall)
				return new UninstallWindow(this);

			return new InstallWindow(this);
		}
	}
}
