using System.Drawing.Imaging;

namespace Magic.Imaging
{
	public class FigureThumbnail : ImageOutputSetting
	{
		public override Dimensions TargetDimension
		{
			get { return new Dimensions(72 * 5); } // 5 inches ~ 12 cm
		}

		public override ImageCodecInfo Codec
		{
			get { return GetEncoder(ImageFormat.Png); }
		}

		public override EncoderParameters Params
		{
			get
			{
				EncoderParameter quality = new EncoderParameter(Encoder.Quality, 100L);
				EncoderParameters encoderParams = new EncoderParameters(1);
				encoderParams.Param[0] = quality;
				return encoderParams;
			}
		}

		public override int Dpi
		{
			get { return 96; }
		}
	}
}