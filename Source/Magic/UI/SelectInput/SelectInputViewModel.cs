using System;
using Caliburn.Micro;
using Microsoft.Win32;

namespace Magic.UI.SelectInput
{
	public class SelectInputViewModel : Screen
	{
		private readonly EventAggregator _eventBus;

		public SelectInputViewModel(EventAggregator eventBus)
		{
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
				_eventBus.Publish(new FilesSelectedEvent(openFileDialog.FileNames));	
			}
		}
	}
}
