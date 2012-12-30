using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Magic.UI.SelectFigures.Events
{
	public class FigureSelectionDoneEvent
	{
		public IEnumerable<CroppedBitmap> Figures { get; private set; }

		public FigureSelectionDoneEvent(IEnumerable<CroppedBitmap> figures)
		{
			Figures = figures;
		}
	}
}