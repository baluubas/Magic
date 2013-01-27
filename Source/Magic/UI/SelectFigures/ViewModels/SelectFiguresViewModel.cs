using System;
using System.Drawing;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Magic.Imaging;
using Magic.UI.SelectFigures.Events;
using StructureMap;

namespace Magic.UI.SelectFigures.ViewModels
{
	public class SelectFiguresViewModel : 
		Screen,
		IHandle<FigureSelectedEvent>,
		IHandle<UndoFigureEvent>
	{
		private readonly FigureExportBuilder _figureExportBuilder;
		private readonly EventAggregator _messageBus;
		private readonly IContainer _container;

		public ObservableCollection<DrawingViewModel> Drawings { get; set; }
		public ObservableCollection<FigureViewModel> Figures { get; set; }
		public ObservableCollection<Point[]> Selections { get; set; } 

		public SelectFiguresViewModel(
		FigureExportBuilder figureExportBuilder,
			EventAggregator messageBus,
			IContainer container)
		{
			_figureExportBuilder = figureExportBuilder;
			_messageBus = messageBus;
			_container = container;
			
			Drawings = new ObservableCollection<DrawingViewModel>();
			Selections = new ObservableCollection<Point[]>();
			Figures = new ObservableCollection<FigureViewModel>();

			_messageBus.Subscribe(this);
		}

		public void StartOver()
		{
			_messageBus.Publish(new StartOverEvent());
		}

		public void DoneSelectingFigures()
		{
			_messageBus.Publish(new FigureSelectionDoneEvent());
		}

		public void KeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && Figures.Any())
			{
				Figures.Last().UndoFigure();
			}
		}

		public void Handle(FigureSelectedEvent message)
		{
			var figureViewModel = _container
				.With(message.Figure)
				.With(message.Undo)
				.GetInstance<FigureViewModel>();

			Figures.Add(figureViewModel);
			figureViewModel.Initialize();
		}

		public void Handle(UndoFigureEvent message)
		{
			var vmToRemove = Figures.Where(fig => message.Figure == fig.Figure).ToArray();
			foreach (var vm in vmToRemove)
			{
				Figures.Remove(vm);
			}
		}

		protected override void OnActivate()
		{
			var loadPagesProgress = new Progress<PdfPage>(CreateNewDrawing);
			_figureExportBuilder.GetPdfPages(loadPagesProgress);
		}

		protected override void OnDeactivate(bool close)
		{
			_messageBus.Unsubscribe(this);
		}

		private void CreateNewDrawing(PdfPage page)
		{
			var drawingViewModel = _container.With(page).GetInstance<DrawingViewModel>();
			drawingViewModel.Initialize();
			Drawings.Add(drawingViewModel);
		}
	}
}
