using System;
using System.Linq;
using System.Threading.Tasks;
using StructureMap;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Magic.Imaging
{
	public class PdfFile
	{
		private readonly IContainer _container;
		public string File { get; private set; }
		public Lazy<Task<PdfPage[]>> _lazyPages;

		public PdfFile(IContainer container, string file)
		{
			_container = container;
			File = file;
			_lazyPages = new Lazy<Task<PdfPage[]>>(InternalGetPagesAsync);
		}

		public Task<PdfPage[]> GetPagesAsync()
		{
			return _lazyPages.Value;
		}

		private Task<PdfPage[]> InternalGetPagesAsync()
		{
			return Task.Factory.StartNew(() =>
			{
				var reader = new PdfReader(File);

				return Enumerable
					.Range(1, reader.NumberOfPages)
					.Select(pageIndex => CreatePage(pageIndex, reader))
					.ToArray();
			});	
		}

		private PdfPage CreatePage(int pageIndex, PdfReader reader)
		{
			Rectangle pageSize = reader.GetPageSize(pageIndex);
			
			return new PdfPage(
				_container,
				_container.GetInstance<IPdfRasterizer>(), 
				this,
				pageIndex,
				pageSize.Width,
				pageSize.Height);
		}
	}
}