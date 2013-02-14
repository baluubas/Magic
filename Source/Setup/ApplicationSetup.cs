
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Magic.Setup
{
	public class SetupApplication : BootstrapperApplication
	{		
		static public Dispatcher BootstrapperDispatcher { get; private set; }
	
		protected override void Run()
		{
			Engine.Log(LogLevel.Verbose, "Launching Custom Setup UX");
			BootstrapperDispatcher = Dispatcher.CurrentDispatcher;
			
			Splash splash = new Splash(this);
			Engine.Detect();
			splash.Closed += (sender, e) => BootstrapperDispatcher.InvokeShutdown();
			splash.Show();
			Dispatcher.Run();
			Engine.Quit(0);
		}
	}
}
