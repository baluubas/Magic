
namespace Magic.Imaging.GhostScript
{
	/// <summary>
	/// Ghostscript settings
	/// </summary>
	public class GhostscriptSettings
	{
		private GhostscriptDevices _device;
		private GhostscriptPages _pages = new GhostscriptPages();
		private System.Drawing.Size _resolution;
		private GhostscriptPageSize _size = new GhostscriptPageSize();

		public GhostscriptDevices Device
		{
			get { return this._device; }
			set { this._device = value; }
		}

		public GhostscriptPages Page
		{
			get { return this._pages; }
			set { this._pages = value; }
		}

		public System.Drawing.Size Resolution
		{
			get { return this._resolution; }
			set { this._resolution = value; }
		}

		public GhostscriptPageSize Size
		{
			get { return this._size; }
			set { this._size = value; }
		}
	}
}