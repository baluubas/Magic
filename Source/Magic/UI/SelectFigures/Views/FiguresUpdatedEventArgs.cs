using System.Windows;

namespace Magic.UI.SelectFigures.Views
{
	public class FiguresUpdatedEventArgs : RoutedEventArgs
	{
		public int Id { get; set; }
		public Rect Bounds { get; set; }
		public int Rotation { get; set; }

		public FiguresUpdatedEventArgs(
			RoutedEvent @event,
			object source,
			int id,
			Rect newBounds,
			int rotation) 
			: base(@event, source)
		{
			Id = id;
			Bounds = newBounds;
			Rotation = rotation;
		}
	}
}