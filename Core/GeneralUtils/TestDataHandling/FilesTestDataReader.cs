using System.Globalization;
using System.IO;
using System.Reflection;
using Core.SeleniumUtils;

namespace Core.GeneralUtils.TestDataHandling
{
	/// <summary>
	///     FilesTestDataReader class.
	/// </summary>
	public class FilesTestDataReader
	{
		protected const string TestDataFolderName = "TestData";
		private readonly Assembly specifiedAssembly;

		/// <summary>
		///     Initializes a new instance of the <see cref="FilesTestDataReader" /> class.
		///     Note that for Unit Test project you should set entry assembly
		///     in the first test line using <see cref="AssemblyUtilities.SetEntryAssembly()" />.
		/// </summary>
		public FilesTestDataReader()
		{
			specifiedAssembly = Assembly.GetEntryAssembly();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="FilesTestDataReader" /> class.
		/// </summary>
		/// <param name="specifiedAssembly">The specified assembly.</param>
		public FilesTestDataReader(Assembly specifiedAssembly)
		{
			this.specifiedAssembly = specifiedAssembly;
		}

		/// <summary>
		///     Reads the test data from specified assembly.
		/// </summary>
		/// <typeparam name="T">Target Type.</typeparam>
		/// <param name="file">The full path to file with extension of a file.</param>
		/// <returns>Object of T type.</returns>
		public T Read<T>(string file)
		{
			using (
				var stream = DataSources.LoadResourceStream(specifiedAssembly,
					string.Format(CultureInfo.InvariantCulture, "{0}.{1}", TestDataFolderName, file)))
			{
				using (var memoryStream = new MemoryStream())
				{
					stream.CopyTo(memoryStream);
					return (T) (object) memoryStream.ToArray();
				}
			}
		}
	}
}