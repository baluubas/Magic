using System;
using Caliburn.Micro;
using Magic.Imaging;
using Microsoft.Win32;

namespace Magic.UI.SelectInput
{
	public class SelectInputViewModel : Screen
	{
		private readonly FigureExportBuilder _figureBuilder;
		private readonly EventAggregator _eventBus;

		public SelectInputViewModel(FigureExportBuilder figureBuilder, EventAggregator eventBus)
		{
			_figureBuilder = figureBuilder;
			_eventBus = eventBus;
		}

		public void SelectFile()
		{
			var openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			openFileDialog.Filter = "Pdf files|*.pdf";
			openFileDialog.Multiselect = true;
			bool? dialogResult = openFileDialog.ShowDialog();
			
			if (dialogResult.Value)
			{
				_figureBuilder.SetSourcePdf(openFileDialog.FileNames);
				_eventBus.Publish(new FilesSelectedEvent(openFileDialog.FileNames));	
			}
		}
	}
}
