using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Magic.Imaging;
using Magic.UI.Helpers;
using Magic.UI.SelectFigures.Events;
using Magic.UI.SelectFigures.Views;

namespace Magic.UI.SelectFigures.ViewModels
{
	public class DrawingViewModel : IDisposable
	{
		private readonly PdfPage _page;
		private readonly EventAggregator _messageBus;
		private TemporaryBitmapFile _tempBitmapFile;
		private readonly IDictionary<int, Figure> _figureMap = new Dictionary<int, Figure>();

		public Observable<bool> IsGeneratingImage { get; set; }
		public Observable<TransformedBitmap> Image { get; set; }
		
		public DrawingViewModel(
			PdfPage page,
			EventAggregator messageBus)
		{
			_page = page;
			_messageBus = messageBus;

			Image = new Observable<TransformedBitmap>(new TransformedBitmap());
			IsGeneratingImage = new Observable<bool>(true);
		}

		public void FigureSelected(FigureSelectedEventArgs arg)
		{
			var sourceRect = new RelativeSelection(
				_page.PageDimensions,
				arg.RelativeOffsetX,
				arg.RelativeOffsetY,
				arg.RelativeWidth,
				arg.RelativeHeight);

			var figure = _page.CreateFigure(sourceRect);
			_figureMap.Add(arg.Id, figure);

			var figureSelectedEvent = new FigureSelectedEvent(
				figure,
				() => {
					_page.RemoveFigure(_figureMap[arg.Id]);
					_figureMap.Remove(arg.Id);
					arg.Undo();
				});

			_messageBus.Publish(figureSelectedEvent);
		}

		public void FigureUpdated(PageRotatedEventArgs arg)
		{
			Figure figure = _figureMap[arg.Id];
			figure.SetRotation(arg.Rotation);
			_messageBus.Publish(new FigureRotatedEvent(figure));		
		}

		public async void Initialize()
		{
			_tempBitmapFile = await _page.GenerateThumbnail();

			Uri uri = new Uri(_tempBitmapFile.FileName, UriKind.Absolute);
			var image = new BitmapImage();
			image.BeginInit();
			image.CacheOption = BitmapCacheOption.OnLoad;
			image.UriSource = uri;
			image.EndInit();

			TransformedBitmap transformed = new TransformedBitmap(image, new RotateTransform(0));
			Image.Value = transformed;
			IsGeneratingImage.Value = false;
		}

		public void Dispose()
		{
			_tempBitmapFile.Dispose();
		}
	}
}