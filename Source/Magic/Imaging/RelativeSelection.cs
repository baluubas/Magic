using System;
using System.Drawing;
using System.Windows;

namespace Magic.Imaging
{
	public class RelativeSelection
	{
		private Dimensions _pageDimensions;

		public double RelativeOffsetX { get; private set; }
		public double RelativeOffsetY { get; private set; }
		public double RelativeWidth { get; private set; }
		public double RelativeHeight { get; private set; }

		public Rotatation Rotation { get; private set; }

		public RelativeSelection(
			Dimensions pageDimensions,
			double relativeOffsetX,
			double relativeOffsetY,
			double relativeWidth,
			double relativeHeight)
			: this(
				pageDimensions,
				relativeOffsetX,
				relativeOffsetY,
				relativeWidth,
				relativeHeight,
				0)
		{
		}

		public RelativeSelection(
			Dimensions pageDimensions,
			double relativeOffsetX,
			double relativeOffsetY,
			double relativeWidth,
			double relativeHeight,
			int rotation)
		{
			_pageDimensions = pageDimensions;
			RelativeOffsetX = relativeOffsetX;
			RelativeOffsetY = relativeOffsetY;
			RelativeWidth = relativeWidth;
			RelativeHeight = relativeHeight;
			Rotation = new Rotatation(rotation);
		}

		public double ScaleRatioForDimension(Dimensions maxDimension)
		{
			var ratioX = maxDimension.Width / (_pageDimensions.Width * RelativeWidth);
			var ratioY = maxDimension.Height / (_pageDimensions.Height * RelativeHeight);
			return Math.Min(ratioX, ratioY);
		}

        public Rectangle ToPixelsFromActualImage(int height, int width)
	    {
	        return new Rectangle(
	            (int)(width*RelativeOffsetX),
                (int)(height * RelativeOffsetY),
                (int)(width * RelativeWidth),
                (int)(height * RelativeHeight));
	    }

	    public RelativeSelection ResizeInPixels(Rectangle rect, Rectangle minimumRect)
	    {
            double changeX = (double)minimumRect.X / rect.X;
            double changeY = (double)minimumRect.Y / rect.Y;
	        double changeWidth =  (double)minimumRect.Width / rect.Width;
            double changeHeight = (double)minimumRect.Height / rect.Height;

	        return new RelativeSelection(_pageDimensions,
                RelativeOffsetX * changeX,
                RelativeOffsetY * changeY,
                RelativeWidth * changeWidth,
                RelativeHeight * changeHeight);
	    }
	}
}