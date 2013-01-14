using System;
using System.Linq;
using Caliburn.Micro;
using Magic.Imaging;
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
		private readonly IContainer _container;
		private IContainer _nestedContainer;

		private readonly EventAggregator _messageBus;
		public WizardSteps[] Steps { get; set; }

		public ShellViewModel(
			IContainer container,
			EventAggregator messageBus)
		{
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
		}

		protected override void OnInitialize()
		{
			StartOver();
		}

		public void Handle(FilesSelectedEvent selectedFiles)
		{
			Steps[1].IsEnabled.Value = true;
			ActivateItem(_nestedContainer.GetInstance<SelectFiguresViewModel>());
		}

		public void Handle(StartOverEvent _)
		{
			StartOver();
		}

		public void StartOver()
		{
			if (_nestedContainer != null)
			{
				_nestedContainer.Dispose();
			}

			_nestedContainer = _container.GetNestedContainer();

			ActivateItem(_nestedContainer.GetInstance<SelectInputViewModel>());
			Steps[1].IsEnabled.Value = false;
			Steps[2].IsEnabled.Value = false;
		}

		public void Handle(FigureSelectionDoneEvent @event)
		{
			ActivateItem(_nestedContainer.GetInstance<SelectOutputViewModel>());
			Steps[2].IsEnabled.Value = true;
		}
	}
}