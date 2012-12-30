
namespace Magic.UI.SelectInput
{
	public class FilesSelectedEvent
	{
		public string[] Files { get; set; }

		public FilesSelectedEvent(string[] files)
		{
			Files = files;
		}
	}
}
