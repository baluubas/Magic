using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Magic.Imaging;
using Magic.UI.Helpers;
using Magic.UI.SelectFigures.Events;
using Action = System.Action;

namespace Magic.UI.SelectFigures.ViewModels
{
	public class FigureViewModel : IHandle<FigureRotatedEvent>
	{
		private readonly Action _undoFigure;
		private readonly EventAggregator _messageBus;

		public Observable<BitmapSource> Image { get; set; }
		public Observable<bool> IsLoading { get; set; }

		public Figure Figure { get; private set; }		 

		public FigureViewModel(
			Figure figure,
			Action undoFigure,
			EventAggregator messageBus)
		{
			Figure = figure;
			_undoFigure = undoFigure;
			_messageBus = messageBus;

			_messageBus.Subscribe(this);

			Image = new Observable<BitmapSource>();
			IsLoading = new Observable<bool>(true);
		}

		public Task Initialize()
		{
			return UpdateThumbnail();
		}

		public void UndoFigure()
		{
			_undoFigure();
			_messageBus.Publish(new UndoFigureEvent(Figure));
			Image.Value = null;
		}

		public async void Handle(FigureRotatedEvent message)
		{
			if (message.Figure != Figure)
				return;

			await UpdateThumbnail();
		}

		private async Task UpdateThumbnail()
		{
			IsLoading.Value = true;
			Image.Value = null;

			using (var tempBitmapFile = new TemporaryBitmapFile("png"))
			{
				await Figure.Export(tempBitmapFile.FileName, new FigureThumbnail(), CancellationToken.None);
				Uri uri = new Uri(tempBitmapFile.FileName, UriKind.Absolute);
				var image = new BitmapImage();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.UriSource = uri;
				image.EndInit();

				Image.Value = image;
				IsLoading.Value = false;
			}
		}
	}
}