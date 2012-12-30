using System;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using Magic.UI.Helpers;
using Magic.UI.SelectFigures.Events;
using Magic.UI.SelectFigures.ViewModels;
using Magic.UI.SelectInput;
using Magic.UI.SelectOutput;
using StructureMap;

namespace Magic.UI
{
	public class ShellViewModel 
		: Conductor<Screen>.Collection.OneActive, 
		IShellViewModel, 
		IHandle<FilesSelectedEvent>,
		IHandle<StartOverEvent>,
		IHandle<FigureSelectionDoneEvent>
	{
		private readonly SelectInputViewModel _input;
		private readonly IContainer _container;
		private readonly EventAggregator _messageBus;
		public WizardSteps[] Steps { get; set; }

		public ShellViewModel(
			SelectInputViewModel input,
			IContainer container,
			EventAggregator messageBus)
		{
			_input = input;
			_container = container;
			_messageBus = messageBus;
			DisplayName = "";
 
			_messageBus.Subscribe(this);

			Steps = new[]
			{
				new WizardSteps { IsEnabled = new Observable<bool>(true)},
				new WizardSteps { IsEnabled = new Observable<bool>(false)},
				new WizardSteps { IsEnabled = new Observable<bool>(false)}
			};

			foreach (FontFamily fontFamily in Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./Fonts/"))
			{
				// Perform action.
			}
		}

		protected override void OnInitialize()
		{
			ActivateItem(_input);
		}

		public void Handle(FilesSelectedEvent selectedFiles)
		{
			Steps[1].IsEnabled.Value = true;
			var figureViewModel = _container.With(selectedFiles.Files).GetInstance<SelectFiguresViewModel>();
			ActivateItem(figureViewModel);
		}

		public void Handle(StartOverEvent _)
		{
			ActivateItem(_input);
			Steps[1].IsEnabled.Value = false;
			Steps[2].IsEnabled.Value = false;
		}

		public void Handle(FigureSelectionDoneEvent @event)
		{
			var outputVM = _container.With(@event.Figures).GetInstance<SelectOutputViewModel>();
			ActivateItem(outputVM);
			Steps[2].IsEnabled.Value = true;
		}
	}
}