using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Magic.UI.SelectFigures.Events
{
	public class FigureSelectedEvent
	{
		public int Id { get; set; }
		public Int32Rect SourceRect { get; set; }
		public int Rotation { get; set; }
		public BitmapSource SourceImage { get; set; }
		public Action Undo { get; private set; }

		public FigureSelectedEvent(
			int id,
			Int32Rect sourceRect,
			int rotation,
			BitmapSource sourceImage,
			Action undo)
		{
			Id = id;
			SourceRect = sourceRect;
			Rotation = rotation;
			SourceImage = sourceImage;
			Undo = undo;
		}
	}
}