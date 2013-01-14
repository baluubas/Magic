using System;
using System.Windows;

namespace Magic.UI.SelectFigures.Views
{
	public class FigureSelectedEventArgs : RoutedEventArgs
	{
		public int Id { get; set; }
		public Action Undo { get; set; }

		public double RelativeOffsetY { get; set; }
		public double RelativeOffsetX { get; set; }
		public double RelativeWidth { get; set; }
		public double RelativeHeight { get; set; }

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