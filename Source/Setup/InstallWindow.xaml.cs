using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Magic.Setup
{
	public partial class InstallWindow : WindowBase
	{
		private string _selectedInstallPath;
		private const string BurnBundleInstallDirectoryVariable = "InstallFolder";

		public InstallWindow(BootstrapperApplication bootstrapper) 
			: base(bootstrapper)
		{
			InitializeComponent();
			_selectedInstallPath = System.IO.Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				"Magic");
		}

		private void ExitSetup(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void PerformInstall(object sender, RoutedEventArgs e)
		{
			CloseButton.IsEnabled = false;
			PreInstall.Visibility = Visibility.Collapsed;
			InstallProgess.Visibility = Visibility.Visible;

			Engine.StringVariables[BurnBundleInstallDirectoryVariable] = _selectedInstallPath;

			var ui = TaskScheduler.FromCurrentSynchronizationContext();
			PerformLaunchAction(LaunchAction.Install).ContinueWith(t =>
			{
				CloseButton.IsEnabled = true;
				InstallProgess.Visibility = Visibility.Collapsed;
				PostInstall.Visibility = Visibility.Visible;
				
				if (t.IsFaulted)
				{
					DoneText.Text = "Installation failed";
					
					Error.Text = t.Exception.InnerException.Message;
					Error.Visibility = Visibility.Visible;
				}                                      
			}, CancellationToken.None, TaskContinuationOptions.None, ui);
		}
	}
}
