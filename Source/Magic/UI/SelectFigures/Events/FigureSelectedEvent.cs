using System;
using Magic.Imaging;

namespace Magic.UI.SelectFigures.Events
{
	public class FigureSelectedEvent
	{
		public Figure Figure { get; set; }
		public Action Undo { get; private set; }

		public FigureSelectedEvent(
			Figure figure,
			Action undo)
		{
			Figure = figure;
			Undo = undo;
		}
	}
}