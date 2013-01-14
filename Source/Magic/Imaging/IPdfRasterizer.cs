using System.Threading.Tasks;
using Magic.Imaging.GhostScript;

namespace Magic.Imaging
{
	public interface IPdfRasterizer
	{
		Task GenerateOutput(string inputPath, string outputPath, GhostscriptSettings settings);
	}
}