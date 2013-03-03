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
					DoneText.Text = "Installation failed - see install log for more information.";
				}                                      
			}, CancellationToken.None, TaskContinuationOptions.None, ui);
		}

		private void ChooseInstallDirectory(object sender, RoutedEventArgs e)
		{
			var browser = new FolderBrowserDialog();
			browser.ShowNewFolderButton = true;
			browser.Description = "Choose installation directory";

			var result = browser.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				_selectedInstallPath = browser.SelectedPath;
				ChooseDirButton.Text = _selectedInstallPath;
			}
		}
	}
}
