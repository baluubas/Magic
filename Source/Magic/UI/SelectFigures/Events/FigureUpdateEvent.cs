using System.Windows.Media.Imaging;

namespace Magic.UI.SelectFigures.Events
{
	public class FigureUpdateEvent
	{
		public int Id { get; private set; }
		public CroppedBitmap CroppedImage { get; private set; }

		public FigureUpdateEvent(int id, CroppedBitmap croppedImage)
		{
			Id = id;
			CroppedImage = croppedImage;
		}
	}
}