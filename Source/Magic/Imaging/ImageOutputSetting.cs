using System.Drawing.Imaging;
using System.Linq;

namespace Magic.Imaging
{
	public abstract class ImageOutputSetting
	{
		public abstract Dimensions TargetDimension { get; }
		public abstract ImageCodecInfo Codec { get; }
		public abstract EncoderParameters Params { get; }
		public abstract int Dpi { get; }

		protected ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
		}
	}
}