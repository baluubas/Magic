using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.Imaging.GhostScript;
using StructureMap;

namespace Magic.Imaging
{
	public class PdfPage
	{
		private readonly Dimensions _maxDimensions = new Dimensions(72*10); // 10 inches

		private readonly IContainer _container;
		private readonly IPdfRasterizer _pdfRendering;
		private readonly PdfFile _source;
		private readonly int _pageIndex;
		private readonly IList<Figure> _figures = new List<Figure>(); 

		public Dimensions PageDimensions { get; private set; }
		public IEnumerable<Figure> Figures { get { return _figures; }}

		public PdfPage(IContainer container, IPdfRasterizer pdfRendering, PdfFile source, int pageIndex, float widthPt, float heightPt)
		{
			_container = container;
			_pdfRendering = pdfRendering;
			_source = source;
			_pageIndex = pageIndex;
			PageDimensions = new Dimensions(widthPt, heightPt);
		}

		public Task<TemporaryBitmapFile> GenerateThumbnail()
		{
			var scaledSize = PageDimensions.ScaleKeepRatio(_maxDimensions);
			return CreateRasterFile(scaledSize, 96);
		}

		public Task<TemporaryBitmapFile> RenderImage(double scaleFactor, int dpi = 96)
		{
			var scaledSize = SettingsByScaleFactor(scaleFactor);
			return CreateRasterFile(scaledSize, dpi);
		}

		public Figure CreateFigure(RelativeSelection sourceRect)
		{
			var figure = _container.With(sourceRect).With(this).GetInstance<Figure>();
			_figures.Add(figure);
			return figure;
		}

		private async Task<TemporaryBitmapFile> CreateRasterFile(Dimensions scaledSize, int dpi)
		{
			var tempFile = new TemporaryBitmapFile("png");
			await _pdfRendering.GenerateOutput(_source.File, tempFile.FileName, CreateSetting(scaledSize, dpi));
			return tempFile;
		}

		private GhostscriptSettings CreateSetting(Dimensions dimensions, int dpi)
		{
			var result = new GhostscriptSettings();
			result.Device = GhostscriptDevices.png16m;
			result.Resolution = new System.Drawing.Size(dpi, dpi);
			result.Page = new GhostscriptPages
				              {
					              AllPages = false,
					              Start = _pageIndex,
					              End = _pageIndex
				              };

			result.Size = new GhostscriptPageSize
				              {
					              Native = GhostscriptPageSizes.UNDEFINED,
					              Manual = dimensions.ToSize()
				              };

			return result;
		}

		private Dimensions SettingsByScaleFactor(double scaleFactor)
		{
			double newWidth = PageDimensions.Width*scaleFactor;
			double newHeight = PageDimensions.Height*scaleFactor;
			return new Dimensions(newWidth, newHeight);
		}

		public void RemoveFigure(Figure figure)
		{
			_figures.Remove(figure);
		}
	}
}