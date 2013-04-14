using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Magic.Imaging
{
    public class MinimumBoundingBox
    {
        private readonly Bitmap _image;

        public MinimumBoundingBox(Bitmap image)
        {
            _image = image;
        }

        public Rectangle Find(Rectangle boundingRectangle) 
        {
            Rectangle rect = new Rectangle(0, 0, _image.Width, _image.Height);

            System.Drawing.Imaging.BitmapData bmpData = _image.LockBits(
                rect,
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                _image.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride*_image.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            byte red = 0;
            byte green = 0;
            byte blue = 0;

            int maxY = 0;
            int minY = _image.Height;
            int maxX = 0;
            int minX = _image.Width;

            int upperX = boundingRectangle.X + boundingRectangle.Width;
            int upperY = boundingRectangle.Y + boundingRectangle.Height;
            for (int x = boundingRectangle.X; x < upperX; x++)
            {
                for (int y = boundingRectangle.Y; y < upperY; y++)
                {
                    int position = (y*bmpData.Stride) + (x*3);
                    blue = rgbValues[position];
                    green = rgbValues[position + 1];
                    red = rgbValues[position + 2];
                    
                    if (blue != 255 || red != 255 || green != 255)
                    {
                        maxY = Math.Max(maxY, y);
                        minY = Math.Min(minY, y);

                        maxX = Math.Max(maxX, x);
                        minX = Math.Min(minX, x);
                    }
                }
            }
            _image.UnlockBits(bmpData);
            
            return new Rectangle(minX, minY, maxX-minX, maxY-minY);
        }
    }
}
