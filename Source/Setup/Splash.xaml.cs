using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Magic.Setup
{
	/// <summary>
	/// Interaction logic for Splash.xaml
	/// </summary>
	public partial class Splash : Window
	{
		private readonly BootstrapperApplication _bootstrapper;

		public Splash(BootstrapperApplication bootstrapper)
		{
			_bootstrapper = bootstrapper;
			_bootstrapper.ApplyComplete += OnApplyComplete;
			_bootstrapper.DetectPackageComplete += OnDetectPackageComplete;
			_bootstrapper.PlanComplete += OnPlanComplete;
			_bootstrapper.Error += (sender, args) => MessageBox.Show("Error bitch: " + args.ErrorMessage);
			InitializeComponent();
		}

		private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
		{
			MessageBox.Show("Plan complete: " + e.Status);
			if (e.Status >= 0)
				_bootstrapper.Engine.Apply(IntPtr.Zero);
		}

		private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
		{
			
		}

		private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
		{
			MessageBox.Show("Apply complete");
			Close();	
		}

		private void CloseApplication(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ExecuteInstall(object sender, RoutedEventArgs e)
		{
			_bootstrapper.Engine.Plan(LaunchAction.Install);
			MessageBox.Show("Planning");
		}
	}
}
