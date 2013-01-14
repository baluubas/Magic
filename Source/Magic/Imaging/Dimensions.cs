
using System;
using System.Drawing;

namespace Magic.Imaging
{
	public class Dimensions
	{
		public double Width { get; private set; }
		public double Height { get; private set; }

		public Dimensions(double width, double height)
		{
			Width = width;
			Height = height;
		}
		
		public Dimensions(double side)
		{
			Width = side;
			Height = side;
		}

		public Dimensions ScaleKeepRatio(Dimensions maxDimensionses)
		{
			var ratioX = maxDimensionses.Width / Width;
			var ratioY = maxDimensionses.Height / Height;
			var ratio = Math.Min(ratioX, ratioY);

			var newWidth = Width * ratio;
			var newHeight = Height * ratio;

			return new Dimensions(newWidth, newHeight);
		}

		public Size ToSize()
		{
			return new Size((int)Width, (int)Height);
		}

		public Dimensions Scale(double scaleFactor)
		{
			return new Dimensions(Width * scaleFactor, Height * scaleFactor);
		}

		public Dimensions ToPixels(int dpiX, int dpiY)
		{
			double pixelWidth = Width * (72 / (double)dpiX);
			double pixelHeigth = Height* (72 / (double)dpiX);
			return new Dimensions(pixelWidth, pixelHeigth);
		}
	}
}
