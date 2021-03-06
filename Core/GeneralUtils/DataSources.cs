using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Core.GeneralUtils
{
	/// <summary>
	///     DataSources class.
	/// </summary>
	public class DataSources
	{
		private const string TestDataFolderName = "TestData";

		/// <summary>
		///     Loads the json.
		/// </summary>
		/// <typeparam name="TDataSource">The type of the T data source.</typeparam>
		/// <param name="file">The file.</param>
		/// <returns>The object of TDataSource type.</returns>
		public TDataSource LoadJson<TDataSource>(string file)
		{
			var assembly = Assembly.GetEntryAssembly();
			using (
				var stream = LoadResourceStream(assembly,
					string.Format(CultureInfo.InvariantCulture, "{0}.{1}.json", TestDataFolderName, file)))
			using (var reader = new StreamReader(stream))
			{
				return JsonConvert.DeserializeObject<TDataSource>(reader.ReadToEnd());
			}
		}

		/// <summary>
		///     Loads the resource stream.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="shortResourceName">Short name of the resource.</param>
		/// <returns>The stream.</returns>
		public static Stream LoadResourceStream(Assembly assembly, string shortResourceName)
		{
			var resourceName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", assembly.GetName().Name, shortResourceName);
			if (string.IsNullOrEmpty(resourceName))
			{
				throw new ArgumentException("resourceName",
					string.Format(CultureInfo.InvariantCulture, "{0} parameter cannot be null or empty", "resourceName"));
			}

			if (!ResourceExists(resourceName, assembly))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Resource '{0}' doesn't exist", resourceName));
			}

			return assembly.GetManifestResourceStream(resourceName);
		}

		/// <summary>
		///     Resources the exists.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		/// <param name="executingAssembly">The executing assembly.</param>
		/// <returns>The flag.</returns>
		public static bool ResourceExists(string resourceName, Assembly executingAssembly)
		{
			var resourceNames = executingAssembly.GetManifestResourceNames();
			return resourceNames.Contains(resourceName);
		}

		/// <summary>
		///     Gets the test data resource path.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>The result value.</returns>
		public static string GetTestDataResourcePath(string fileName)
		{
			var fullPath = Path.Combine(Environment.CurrentDirectory,
				string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", TestDataFolderName, fileName));
			if (!File.Exists(fullPath))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File '{0}' doesn't exist", fileName));
			}

			return fullPath;
		}
	}
}