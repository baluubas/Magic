using Caliburn.Micro;
using Magic.UI.Helpers;

namespace Magic.UI
{
	public class WizardSteps
	{
		public Observable<bool> IsEnabled { get; set; }
		public Screen ViweModel { get; set; }
	}
}