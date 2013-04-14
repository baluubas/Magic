
using System.Drawing;
using System.Threading;
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
			ImageOutputSetting settings,
			CancellationToken ct)
		{
			var trimmedSelection = await GetTrimmedSelection();
			var scaleFactor = trimmedSelection.ScaleRatioForDimension(settings.TargetDimension);

			ThrowIfCancelled(ct);

			using (var renderedPageImageFile = await _page.RenderImage(scaleFactor, settings.Dpi))
			using (Bitmap pageBitmap = new Bitmap(renderedPageImageFile.FileName))
			{
				ThrowIfCancelled(ct);

				var cropRectangle = CalculateCropRectangleInPixels(trimmedSelection, pageBitmap.Width, pageBitmap.Height);
				using (Bitmap cropped = pageBitmap.Clone(cropRectangle, pageBitmap.PixelFormat))
				{
					ThrowIfCancelled(ct);
					cropped.RotateFlip(trimmedSelection.Rotation.ToRotateFlipType());
					cropped.Save(filename, settings.Codec, settings.Params);
				}
			}
		}

		private static void ThrowIfCancelled(CancellationToken ct)
		{
			if (ct.IsCancellationRequested)
			{
				throw new TaskCanceledException();
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
			using (var largeThumb = await _page.RenderImage(1))
			{
				using (var bitmap = new Bitmap(largeThumb.FileName))
				{
				    var mbb = new MinimumBoundingBox(bitmap);

				    Rectangle rect = _selection.ToPixelsFromActualImage(bitmap.Height, bitmap.Width);
                    var minimumRect = mbb.Find(rect);
				    return _selection.ResizeInPixels(rect, minimumRect);
				}
			}
		}
	}
}