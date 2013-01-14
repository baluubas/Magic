using System;
using System.IO;

namespace Magic.Imaging
{
	public class TemporaryBitmapFile : IDisposable
	{
		public string FileName { get; private set; }

		public TemporaryBitmapFile(string extension)
		{
			FileName = RandomFile("Magic-", "." + extension);
		}

		private string RandomFile(string prefix, string extension)
		{
			string randomPart = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 7);
			string fileName = prefix + randomPart + extension;
			return Path.Combine(Path.GetTempPath(), fileName);
		}

		public void Dispose()
		{
			if (File.Exists(FileName))
				File.Delete(FileName);
		}
	}
}