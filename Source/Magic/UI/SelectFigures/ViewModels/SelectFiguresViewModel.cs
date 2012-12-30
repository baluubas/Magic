using System.Drawing;
using System.Linq;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Magic.UI.SelectFigures.Events;
using Magic.UI.SelectFigures.Views;
using StructureMap;

namespace Magic.UI.SelectFigures.ViewModels
{
	public class SelectFiguresViewModel : 
		Screen,
		IHandle<FigureSelectedEvent>,
		IHandle<UndoFigureEvent>
	{
		private readonly EventAggregator _messageBus;
		private readonly IContainer _container;

		public ObservableCollection<DrawingViewModel> Drawings { get; set; }
		public ObservableCollection<FigureViewModel> Figures { get; set; }
		public ObservableCollection<Point[]> Selections { get; set; } 

		public SelectFiguresViewModel(
			string[] pdfFiles,
			EventAggregator messageBus,
			IContainer container)
		{
			_messageBus = messageBus;
			_container = container;
			
			Drawings = new ObservableCollection<DrawingViewModel>();
			Selections = new ObservableCollection<Point[]>();
			Figures = new ObservableCollection<FigureViewModel>();

			_messageBus.Subscribe(this);

			foreach (var pdfFile in pdfFiles)
			{
				CreateNewDrawing(pdfFile);
			}
		}

		public void StartOver()
		{
			_messageBus.Publish(new StartOverEvent());
		}

		public void DoneSelectingFigures()
		{
		//	_messageBus.Publish(new FigureSelectionDoneEvent(Figures.Select(x => x.Image.Value)));
		}

		public void Handle(FigureSelectedEvent message)
		{
			var figureViewModel = _container
				.With(message.Id)
				.With(message.SourceRect)
				.With(message.SourceImage)
				.With(message.Undo)
				.GetInstance<FigureViewModel>();

			Figures.Add(figureViewModel);
		}

		public void Handle(UndoFigureEvent message)
		{
			var vmToRemove = Figures.Where(fig => message.IdsToUndo.Contains(fig.Id)).ToArray();
			foreach (var vm in vmToRemove)
			{
				Figures.Remove(vm);
			}
		}

		protected override void OnDeactivate(bool close)
		{
			_messageBus.Unsubscribe(this);
		}

		private void CreateNewDrawing(string pdfFile)
		{
			var drawingViewModel = _container.With<string>(pdfFile).GetInstance<DrawingViewModel>();
			drawingViewModel.Initialize();
			Drawings.Add(drawingViewModel);
		}
	}
}
