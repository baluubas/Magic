using System;
using System.Drawing;

namespace Magic.Imaging
{
	public class Rotatation
	{
		private readonly int _angle;

		public Rotatation(int angle)
		{
			_angle = angle % 360;
		}

		public RotateFlipType ToRotateFlipType()
		{
			switch (_angle)
			{
				case 0: 
					return RotateFlipType.RotateNoneFlipNone;
				case 90:
					return RotateFlipType.Rotate90FlipNone;
				case 180:
					return RotateFlipType.Rotate180FlipNone;
				case 270:
					return RotateFlipType.Rotate270FlipNone;	
				default:
					throw new ArgumentException("Can only rotate in 90 degree increments.");
			}
		}
	}
}