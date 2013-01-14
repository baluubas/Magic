using System;
using System.Diagnostics;
using System.IO;
using Caliburn.Micro;
using Magic.Imaging;
using Magic.UI.Helpers;

namespace Magic.UI.SelectOutput
{
	public class SelectOutputViewModel : Screen
	{
		private readonly FigureExportBuilder _figureExportBuilder;
		private readonly EventAggregator _messageBus;
		private readonly string _targetDirectory;

		public Observable<bool> IsSaving { get; set; }
		public Observable<string> ErrorMessage { get; set; } 

		public SelectOutputViewModel(FigureExportBuilder figureExportBuilder, EventAggregator messageBus)
		{
			_figureExportBuilder = figureExportBuilder;
			_messageBus = messageBus;
			IsSaving = new Observable<bool>(true);
			ErrorMessage = new Observable<string>();

			string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			_targetDirectory = Path.Combine(desktop, "Figures");

		}

		public void BrowseResult()
		{
			Process.Start(_targetDirectory);
		}

		public void StartOver()
		{
			_messageBus.Publish(new StartOverEvent());
		}

		protected async override void OnActivate()
		{
			try
			{
				await _figureExportBuilder.Export(_targetDirectory);
			}
			catch (Exception ex)
			{
				
			}
			finally
			{
				IsSaving.Value = false;	
			}
		}
	}
}
