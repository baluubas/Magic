using System;
using System.Collections.Generic;
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

		public Observable<bool> IsSaving { get; set; }

		public SelectOutputViewModel(IEnumerable<CroppedBitmap> figures)
		{
			_figures = figures;
			IsSaving = new Observable<bool>(true);
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
			string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			string directory = Path.Combine(desktop, "somename");
			string filePath = Path.Combine(directory, string.Format("fig{0:D2}.jpg", figureIndex));
 			Directory.CreateDirectory(directory);
			
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
