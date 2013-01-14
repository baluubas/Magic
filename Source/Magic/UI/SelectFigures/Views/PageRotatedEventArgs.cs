using System.Windows;

namespace Magic.UI.SelectFigures.Views
{
	public class PageRotatedEventArgs : RoutedEventArgs
	{
		public int Id { get; set; }
		public int Rotation { get; set; }

		public PageRotatedEventArgs(
			RoutedEvent @event,
			object source,
			int id,
			int rotation) 
			: base(@event, source)
		{
			Id = id;
			Rotation = rotation;
		}
	}
}