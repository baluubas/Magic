using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Magic.UI.Helpers;

namespace Magic.UI.SelectOutput
{
	public class SelectOutputViewModel : Screen
	{
		private readonly IEnumerable<CroppedBitmap> _figures;
		private readonly EventAggregator _messageBus;
		private string _targetDirectory;

		public Observable<bool> IsSaving { get; set; }

		public SelectOutputViewModel(IEnumerable<CroppedBitmap> figures, EventAggregator messageBus)
		{
			_figures = figures;
			_messageBus = messageBus;
			IsSaving = new Observable<bool>(true);

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

		protected override void OnActivate()
		{
			int figureIndex = 1;

			IList<Task> saveTasks = new List<Task>();
			foreach (var croppedBitmap in _figures)
			{
				int index = figureIndex;
				CroppedBitmap bitmap = croppedBitmap;
				var saveTask = SaveImage(bitmap, index);
				saveTasks.Add(saveTask);
				figureIndex++;
			}

			WaitAsync(saveTasks);
		}

		private async void WaitAsync(IEnumerable<Task> saveTasks)
		{
			await Task.WhenAll(saveTasks);
			IsSaving.Value = false;
		}

		private async Task SaveImage(BitmapSource image, int figureIndex)
		{
			string filePath = Path.Combine(_targetDirectory, string.Format("fig{0:D2}.jpg", figureIndex));
			Directory.CreateDirectory(_targetDirectory);
			
			var encoder = new JpegBitmapEncoder();
			using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			{
				encoder.Frames.Add(BitmapFrame.Create(image));
				encoder.Save(fs);
				fs.Flush();
				fs.Close();
			}
		}
	}
}
