using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Magic.Setup
{
	public partial class UninstallWindow : WindowBase
	{
		public UninstallWindow(BootstrapperApplication bootstrapper) 
			: base(bootstrapper)
		{
			InitializeComponent();
		}

		private void ExitSetup(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void PerformUninstall(object sender, RoutedEventArgs e)
		{
			CloseButton.IsEnabled = false;
			PreInstall.Visibility = Visibility.Collapsed;
			UninstallProgress.Visibility = Visibility.Visible;

			var ui = TaskScheduler.FromCurrentSynchronizationContext();
			PerformLaunchAction(LaunchAction.Uninstall).ContinueWith(t =>
			{
				CloseButton.IsEnabled = true;
				UninstallProgress.Visibility = Visibility.Collapsed;
				PostUninstall.Visibility = Visibility.Visible;
				
				if (t.IsFaulted)
				{
					DoneText.Text = "Uninstall failed";

					Error.Text = t.Exception.InnerException.Message;
					Error.Visibility = Visibility.Visible;
				}                                      
			}, CancellationToken.None, TaskContinuationOptions.None, ui);
		}
	}
}
