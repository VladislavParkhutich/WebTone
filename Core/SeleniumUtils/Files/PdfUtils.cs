using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Core.SeleniumUtils.Files
{
	/// <summary>
	///     PdfUtils class.
	/// </summary>
	public static class PdfUtils
	{
		/// <summary>
		///     PDFs the text.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The result.</returns>
		public static string PdfText(string path)
		{
			using (var reader = new PdfReader(path))
			{
				var text = new StringBuilder();

				for (var i = 1; i <= reader.NumberOfPages; i++)
				{
					text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
				}

				return text.ToString();
			}
		}

		/// <summary>
		///     PDFs the text.
		/// </summary>
		/// <param name="pdfFileContent">Content of the PDF file.</param>
		/// <returns>The result.</returns>
		public static string PdfText(byte[] pdfFileContent)
		{
			using (var reader = new PdfReader(pdfFileContent))
			{
				var text = new StringBuilder();
				for (var i = 1; i <= reader.NumberOfPages; i++)
				{
					text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
				}

				return text.ToString();
			}
		}
	}
}