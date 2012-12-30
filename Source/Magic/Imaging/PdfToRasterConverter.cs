using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Magic.Imaging
{
	public class PdfToRasterConverter : IPdfToRasterConverter
	{
		public string GenerateRasterForPdf(string pdfPath)
		{
			string tempFileName = RandomFile("Magic-", ".png");
			GhostscriptWrapper.GeneratePageThumb(pdfPath, tempFileName, 1, 298, 421);
			return tempFileName;
		}

		private string RandomFile(string prefix, string extension)
		{
			string randomPart = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 7);
			string fileName = prefix + randomPart + extension;
			return Path.Combine(Path.GetTempPath(), fileName);
		}
	}
}