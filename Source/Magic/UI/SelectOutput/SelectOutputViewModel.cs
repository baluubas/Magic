using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

		public Observable<bool> ShowSelection { get; private set; }
		public Observable<bool> ShowSaving { get; private set; }
		public Observable<bool> ShowCompleted { get; private set; }

		public Observable<string> ErrorMessage { get; private set; }
		public Observable<string> SavingMessage { get; private set; }

		public SelectOutputViewModel(FigureExportBuilder figureExportBuilder, EventAggregator messageBus)
		{
			_figureExportBuilder = figureExportBuilder;
			_messageBus = messageBus;
			ShowSelection = new Observable<bool>(false);
			ShowSaving = new Observable<bool>(false);
			ShowCompleted = new Observable<bool>(false);
			
			ErrorMessage = new Observable<string>();
			SavingMessage = new Observable<string>("");

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

		public void ExportForOHIM()
		{
			Export(new OhimImageOutputSettingses(), "OHIM");
		}

		public void ExportForWIPO()
		{
			Export(new WipoImageOutputSetting(), "WIPO");
		}

		private async Task Export(ImageOutputSetting settings, string name)
		{
			ShowSaving.Value = true;
			try
			{
				_figureExportBuilder.SetExportConfiguration(settings);
				SavingMessage.Value = string.Format("Hold on - making figures for a {0} application", name);
				var cancellationTokenSource = new CancellationTokenSource();
				await _figureExportBuilder.Export(_targetDirectory, cancellationTokenSource.Token);
			}
			catch (TaskCanceledException) { }
			catch (Exception)
			{
			}
			finally
			{
				ShowCompleted.Value = true;
			}
		}
	}
}
