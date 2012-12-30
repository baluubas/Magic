using System;
using System.Windows;

namespace Magic.UI.SelectFigures.Views
{
	public class FigureSelectedEventArgs : RoutedEventArgs
	{
		public int Id { get; set; }
		public Action Undo { get; set; }

		public double Y { get; set; }
		public double X { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }

		public FigureSelectedEventArgs(
			RoutedEvent @event,
			object source,
			int id,
			Action undoSelection) 
			: base(@event, source)
		{
			Id = id;
			Undo = undoSelection;
		}
	}
}