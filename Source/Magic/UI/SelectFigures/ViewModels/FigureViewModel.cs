using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Magic.UI.Helpers;
using Magic.UI.SelectFigures.Events;
using Action = System.Action;

namespace Magic.UI.SelectFigures.ViewModels
{
	public class FigureViewModel : IHandle<FigureUpdateEvent>
	{
		private readonly BitmapSource _sourceImage;
		private readonly Action _undoFigure;
		private readonly EventAggregator _messageBus;

		public int Id { get; private set; }
		public Observable<CroppedBitmap> Image { get; set; }
		public Observable<bool> IsLoading { get; set; }

		public FigureViewModel(
			int id,
			Int32Rect sourceRect,
			BitmapSource sourceImage,
			Action undoFigure,
			EventAggregator messageBus)
		{
			Id = id;
			_sourceImage = sourceImage;
			_undoFigure = undoFigure;
			_messageBus = messageBus;

			_messageBus.Subscribe(this);

			var croppedBitmap = new CroppedBitmap(sourceImage, sourceRect);
			Image = new Observable<CroppedBitmap>(croppedBitmap);
			IsLoading = new Observable<bool>(false);
		}	

		public void UndoFigure()
		{
			_undoFigure();
			_messageBus.Publish(new UndoFigureEvent(Id));
			Image.Value = null;
		}

		public void Handle(FigureUpdateEvent message)
		{
			if (message.Id != Id)
				return;

			Image.Value = new CroppedBitmap(_sourceImage, message.SourceRect);
		}
	}
}