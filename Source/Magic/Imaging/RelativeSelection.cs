using System;

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
	}
}