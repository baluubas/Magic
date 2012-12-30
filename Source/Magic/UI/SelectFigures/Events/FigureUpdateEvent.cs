using System.Windows;

namespace Magic.UI.SelectFigures.Events
{
	public class FigureUpdateEvent
	{
		public int Id { get; private set; }
		public Int32Rect SourceRect { get; private set; }
		public int Rotation { get; private set; }

		public FigureUpdateEvent(int id, Int32Rect sourceRect, int rotation)
		{
			Id = id;
			SourceRect = sourceRect;
			Rotation = rotation;
		}
	}
}