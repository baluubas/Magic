
using System.Drawing;
using System.Threading.Tasks;

namespace Magic.Imaging
{
	public class Figure
	{
		private readonly PdfPage _page;
		private RelativeSelection _selection;

		public Figure(
			PdfPage page,
			RelativeSelection selection)
		{
			_page = page;
			_selection = selection;
		}

		public void SetRotation(int rotation)
		{
			_selection = new RelativeSelection(
				_page.PageDimensions,
				_selection.RelativeOffsetX,
				_selection.RelativeOffsetY,
				_selection.RelativeWidth,
				_selection.RelativeHeight,
				rotation);
		}

		public async Task Export(
			string filename,
			ImageOutputSetting settings)
		{
			var trimmedSelection = await GetTrimmedSelection();
			var scaleFactor = trimmedSelection.ScaleRatioForDimension(settings.TargetDimension);

			using (var renderedPageImageFile = await _page.RenderImage(scaleFactor, settings.Dpi))
			using (Bitmap pageBitmap = new Bitmap(renderedPageImageFile.FileName))
			{
				var cropRectangle = CalculateCropRectangleInPixels(trimmedSelection, pageBitmap.Width, pageBitmap.Height);
				using (Bitmap cropped = pageBitmap.Clone(cropRectangle, pageBitmap.PixelFormat))
				{
					cropped.RotateFlip(trimmedSelection.Rotation.ToRotateFlipType());
					cropped.Save(filename, settings.Codec, settings.Params);
				}
			}
		}

		private Rectangle CalculateCropRectangleInPixels(RelativeSelection selection, int width, int height)
		{
			return new Rectangle(
				(int)(width * selection.RelativeOffsetX),
				(int)(height * selection.RelativeOffsetY),
				(int)(width * selection.RelativeWidth),
				(int)(height * selection.RelativeHeight));
		}

		private async Task<RelativeSelection> GetTrimmedSelection()
		{
			using (var largeThumb = await _page.GenerateThumbnail())
			{
				using (var bitmap = new Bitmap(largeThumb.FileName))
				{
					// TODO: Do the actual calculations.
					return _selection;		
				}
			}
		}
	}
}