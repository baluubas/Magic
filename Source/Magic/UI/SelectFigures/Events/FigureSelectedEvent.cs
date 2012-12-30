using System;
using System.Windows.Media.Imaging;

namespace Magic.UI.SelectFigures.Events
{
	public class FigureSelectedEvent
	{
		public int Id { get; set; }
		public CroppedBitmap CroppedImage { get; set; }
		public Action Undo { get; private set; }

		public FigureSelectedEvent(
			int id,
			CroppedBitmap croppedImage,
			Action undo)
		{
			Id = id;
			CroppedImage = croppedImage;
			Undo = undo;
		}
	}
}