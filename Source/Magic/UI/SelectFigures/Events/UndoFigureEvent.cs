using Magic.Imaging;

namespace Magic.UI.SelectFigures.Events
{
	public class UndoFigureEvent
	{
		public Figure Figure { get; private set; }

		public UndoFigureEvent(Figure figure)
		{
			Figure = figure;
		}
	}
}