using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Magic.Utilities;

namespace Magic.Imaging.GhostScript
{
	public class GhostscriptWrapper : IPdfRasterizer
	{
		[DllImport("gsdll32.dll", EntryPoint = "gsapi_new_instance")]
		private static extern int CreateAPIInstance(out IntPtr pinstance, IntPtr caller_handle);

		[DllImport("gsdll32.dll", EntryPoint = "gsapi_init_with_args")]
		private static extern int InitAPI(IntPtr instance, int argc, string[] argv);

		[DllImport("gsdll32.dll", EntryPoint = "gsapi_exit")]
		private static extern int ExitAPI(IntPtr instance);

		[DllImport("gsdll32.dll", EntryPoint = "gsapi_delete_instance")]
		private static extern void DeleteAPIInstance(IntPtr instance);

		private static readonly string[] ARGS = new string[] {
			                                                     // Keep gs from writing information to standard output
			                                                     "-q",                     
			                                                     "-dQUIET",
               
			                                                     "-dPARANOIDSAFER",       // Run this command in safe mode
			                                                     "-dBATCH",               // Keep gs from going into interactive mode
			                                                     "-dNOPAUSE",             // Do not prompt and pause for each page
			                                                     "-dNOPROMPT",            // Disable prompts for user interaction           
			                                                     "-dMaxBitmap=500000000", // Set high for better performance
			                                                     "-dNumRenderingThreads=4", // Multi-core, come-on!
                
			                                                     // Configure the output anti-aliasing, resolution, etc
			                                                     "-dAlignToPixels=0",
			                                                     "-dGridFitTT=0",
			                                                     "-dTextAlphaBits=4",
			                                                     "-dGraphicsAlphaBits=4"
		                                                     };

		private readonly TaskFactory _taskFactory;

		public GhostscriptWrapper()
		{
			LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(1);
			_taskFactory = new TaskFactory(lcts);
		}

		public Task GenerateOutput(string inputPath, string outputPath, GhostscriptSettings settings)
		{
			return CallAPI(GetArgs(inputPath, outputPath, settings));
		}

		private Task CallAPI(string[] args)
		{
			return _taskFactory.StartNew(() => 
			{
				IntPtr gsInstancePtr;
			
				CreateAPIInstance(out gsInstancePtr, IntPtr.Zero);
				try
				{
					int result = InitAPI(gsInstancePtr, args.Length, args);

					if (result < 0)
					{
						throw new ExternalException("Ghostscript conversion error", result);
					}
				}
				finally
				{
					Cleanup(gsInstancePtr);
				}      
			});
		}

		private void Cleanup(IntPtr gsInstancePtr)
		{
			ExitAPI(gsInstancePtr);
			DeleteAPIInstance(gsInstancePtr);
		}

		private static string[] GetArgs(string inputPath,
		                                string outputPath,
		                                GhostscriptSettings settings)
		{
			var args = new System.Collections.ArrayList(ARGS);

			if (settings.Device == GhostscriptDevices.UNDEFINED)
			{
				throw new ArgumentException("An output device must be defined for Ghostscript", "GhostscriptSettings.Device");
			}

			if (settings.Page.AllPages == false && (settings.Page.Start <= 0 && settings.Page.End < settings.Page.Start))
			{
				throw new ArgumentException("Pages to be printed must be defined.", "GhostscriptSettings.Pages");
			}

			if (settings.Resolution.IsEmpty)
			{
				throw new ArgumentException("An output resolution must be defined", "GhostscriptSettings.Resolution");
			}

			if (settings.Size.Native == GhostscriptPageSizes.UNDEFINED && settings.Size.Manual.IsEmpty)
			{
				throw new ArgumentException("Page size must be defined", "GhostscriptSettings.Size");
			}

			// Output device
			args.Add(String.Format("-sDEVICE={0}", settings.Device));

			// Pages to output
			if (settings.Page.AllPages)
			{
				args.Add("-dFirstPage=1");
			}
			else
			{
				args.Add(String.Format("-dFirstPage={0}", settings.Page.Start));
				if (settings.Page.End >= settings.Page.Start)
				{
					args.Add(String.Format("-dLastPage={0}", settings.Page.End));
				}
			}

			// Page size
			if (settings.Size.Native == GhostscriptPageSizes.UNDEFINED)
			{
				args.Add(String.Format("-dDEVICEWIDTHPOINTS={0}", settings.Size.Manual.Width));
				args.Add(String.Format("-dDEVICEHEIGHTPOINTS={0}", settings.Size.Manual.Height));
			}
			else
			{
				args.Add(String.Format("-sPAPERSIZE={0}", settings.Size.Native.ToString()));
			}

			// Page resolution
			args.Add(String.Format("-dDEVICEXRESOLUTION={0}", settings.Resolution.Width));
			args.Add(String.Format("-dDEVICEYRESOLUTION={0}", settings.Resolution.Height));

			// Files
			args.Add(String.Format("-sOutputFile={0}", outputPath));
			args.Add("-dFIXEDMEDIA");
			args.Add("-dPDFFitPage");
			args.Add(inputPath);

			return (string[])args.ToArray(typeof(string));

		}
	}
}