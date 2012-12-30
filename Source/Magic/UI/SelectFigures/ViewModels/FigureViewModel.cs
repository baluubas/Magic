using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Magic.UI.Helpers;
using Magic.UI.SelectFigures.Events;
using Action = System.Action;

namespace Magic.UI.SelectFigures.ViewModels
{
	public class FigureViewModel : IHandle<FigureUpdateEvent>
	{
		private readonly Action _undoFigure;
		private readonly EventAggregator _messageBus;

		public int Id { get; private set; }
		public Observable<CroppedBitmap> Image { get; set; }
		public Observable<bool> IsLoading { get; set; }

		public FigureViewModel(
			int id,
			CroppedBitmap croppedImage,
			Action undoFigure,
			EventAggregator messageBus)
		{
			Id = id;
			_undoFigure = undoFigure;
			_messageBus = messageBus;

			_messageBus.Subscribe(this);

			Image = new Observable<CroppedBitmap>(croppedImage);
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

			Image.Value = message.CroppedImage;
		}
	}
}