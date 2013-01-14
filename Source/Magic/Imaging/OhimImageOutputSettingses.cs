using System.Drawing.Imaging;

namespace Magic.Imaging
{
	public class OhimImageOutputSettingses : ImageOutputSetting
	{
		public override Dimensions TargetDimension
		{
			get { return new Dimensions(481, 680); } // 17 x 24 cm (2008 x 2835)
		}

		public override ImageCodecInfo Codec
		{
			get { return GetEncoder(ImageFormat.Jpeg); }
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
			get { return 300; }
		}
	}
}