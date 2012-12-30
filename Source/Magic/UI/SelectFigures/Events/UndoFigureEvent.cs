namespace Magic.UI.SelectFigures.Events
{
	public class UndoFigureEvent
	{
		public int[] IdsToUndo { get; private set; }

		public UndoFigureEvent(int[] ids)
		{
			IdsToUndo = ids;
		}

		public UndoFigureEvent(int id)
		{
			IdsToUndo = new[] {id};
		}
	}
}