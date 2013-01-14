
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using StructureMap;

namespace Magic.Imaging
{
	public class FigureExportBuilder : IDisposable
	{
		private readonly IContainer _container;
		private PdfFile[] _sourcePdfs;
		public ImageOutputSetting ImageOutputSetting { get; private set; }

		public FigureExportBuilder(IContainer container)
		{
			_container = container;
			ImageOutputSetting = new OhimImageOutputSettingses();
		}

		public void SetExportConfiguration(ImageOutputSetting imageOutputSetting)
		{
			if (ImageOutputSetting == null)
				throw new ArgumentNullException("imageOutputSetting");

			ImageOutputSetting = imageOutputSetting;
		}

		public void SetSourcePdf(string[] files)
		{
			if (_sourcePdfs != null)
				throw new InvalidOperationException("Source pdfs can only be set once bro.");

			_sourcePdfs = files.Select(x => new PdfFile(_container, x)).ToArray();
		}

		public Task<PdfPage[]> GetPdfPages(IProgress<PdfPage> loadPagesProgress = null)
		{
			return Task.Factory.StartNew(async () =>
			{
				var result = new List<PdfPage>();

				foreach (var sourcePdf in _sourcePdfs)
				{
					IEnumerable<PdfPage> pages = await sourcePdf.GetPagesAsync();

					foreach (var pdfPage in pages)
					{
						result.Add(pdfPage);

						if(loadPagesProgress != null)
							loadPagesProgress.Report(pdfPage);
					}
				}

				return result.ToArray();
			}).Unwrap();
		}

		public void Dispose()
		{
		}

		public async Task Export(string targetDirectory)
		{
			PdfPage[] pdfPages = await GetPdfPages();
			Directory.CreateDirectory(targetDirectory);

			int figureIndex = 1;
			foreach (var figure in pdfPages.SelectMany(x => x.Figures))
			{
				string filePath = Path.Combine(targetDirectory, string.Format("fig{0:D2}.jpg", figureIndex++));
				await figure.Export(filePath, ImageOutputSetting);
			}
		}
	}
}
