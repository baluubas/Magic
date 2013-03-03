
using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Magic.Setup
{
	public class WindowBase : Window
	{
		private TaskCompletionSource<bool> _completionSource; 

		protected Engine Engine { get; set; }

		public WindowBase(BootstrapperApplication bootstrapper)
		{
			bootstrapper.ApplyComplete += OnApplyComplete;
			bootstrapper.PlanComplete += OnPlanComplete;
			bootstrapper.Error += (sender, args) => MessageBox.Show("Darn, setup failed:\n" + args.ErrorMessage);
			
			Engine = bootstrapper.Engine;
		}

		private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
		{
			if (e.Status >= 0)
			{
				Engine.Apply(IntPtr.Zero);
			}
			else
			{
				_completionSource.TrySetException(new Exception("Unable to plan successfully."));
			}	
		}

		private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
		{
			_completionSource.TrySetResult(true);
		}

		public Task PerformLaunchAction(LaunchAction action)
		{
			_completionSource = new TaskCompletionSource<bool>();

			Engine.Plan(action);

			return _completionSource.Task;
		} 
	}
}
