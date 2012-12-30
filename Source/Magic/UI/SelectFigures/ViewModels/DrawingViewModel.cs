using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Magic.Imaging;
using Magic.UI.Helpers;
using Magic.UI.SelectFigures.Events;
using Magic.UI.SelectFigures.Views;

namespace Magic.UI.SelectFigures.ViewModels
{
	public class DrawingViewModel
	{
		private double scale;

		private readonly string _pdfFile;
		private readonly EventAggregator _messageBus;
		private readonly IPdfToRasterConverter _pdfConverter;

		public Observable<bool> IsGeneratingImage { get; set; }
		public Observable<TransformedBitmap> Image { get; set; }
		
		public DrawingViewModel(
			string pdfFile,
			EventAggregator messageBus,
			IPdfToRasterConverter pdfConverter)
		{
			_pdfFile = pdfFile;
			_messageBus = messageBus;
			_pdfConverter = pdfConverter;
			Image = new Observable<TransformedBitmap>(new TransformedBitmap());
			IsGeneratingImage = new Observable<bool>(true);
		}

		public void ScaleChanged(Viewbox viewBox)
		{
			if(Image.Value == null) 
				return;
			
			scale = Image.Value.Width/viewBox.ActualWidth;
		}

		public void FigureSelected(FigureSelectedEventArgs arg)
		{
			var sourceRect = GetSourceRect(arg.X, arg.Y, arg.Width, arg.Height);

			var figureSelectedEvent = new FigureSelectedEvent(
				arg.Id,
				new CroppedBitmap(Image.Value, sourceRect), 
				arg.Undo);

			_messageBus.Publish(figureSelectedEvent);
		}

		public void FigureUpdated(FiguresUpdatedEventArgs arg)
		{
			Int32Rect sourceRect = GetSourceRect(arg.Bounds.X, arg.Bounds.Y, arg.Bounds.Width, arg.Bounds.Height);
			CroppedBitmap croppedImage = new CroppedBitmap(Image.Value, sourceRect);
			_messageBus.Publish(new FigureUpdateEvent(arg.Id, croppedImage));		
		}

		public async void Initialize()
		{
			string tempFileName = await Task<string>.Factory.StartNew(() => _pdfConverter.GenerateRasterForPdf(_pdfFile));

			Uri uri = new Uri(tempFileName, UriKind.Absolute);
			var image = new BitmapImage();
			image.BeginInit();
			image.UriSource = uri;
			image.EndInit();

			TransformedBitmap transformed = new TransformedBitmap(image, new RotateTransform(0));
			Image.Value = transformed;
			IsGeneratingImage.Value = false;
		}

		private Int32Rect GetSourceRect(double inputX, double inputY, double inputWidth, double inputHeight)
		{
			double dpiX = Image.Value.DpiX;
			double dpiY = Image.Value.DpiY;

			double width = inputWidth * scale;
			double height = inputHeight * scale;

			int pixelWidth = (int) (width*dpiX/96);
			int pixelHeight = (int) (height*dpiY/96);
			int pixelOffsetX = (int)(inputX * scale * dpiX / 96);
			int pixelOffsetY = (int)(inputY * scale * dpiY / 96);

			return new Int32Rect(pixelOffsetX, pixelOffsetY, pixelWidth, pixelHeight);
		}
	}
}