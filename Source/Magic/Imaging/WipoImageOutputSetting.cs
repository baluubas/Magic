using System.Drawing.Imaging;

namespace Magic.Imaging
{
	public class WipoImageOutputSetting : ImageOutputSetting
	{
		public override Dimensions TargetDimension
		{
			get { return new Dimensions(453); } // 16 x 16 cm 
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