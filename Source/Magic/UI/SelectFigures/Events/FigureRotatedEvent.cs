using System.Windows.Media.Imaging;
using Magic.Imaging;

namespace Magic.UI.SelectFigures.Events
{
	public class FigureRotatedEvent
	{
		public Figure Figure { get; private set; }

		public FigureRotatedEvent(Figure figure)
		{
			Figure = figure;
		}
	}
}