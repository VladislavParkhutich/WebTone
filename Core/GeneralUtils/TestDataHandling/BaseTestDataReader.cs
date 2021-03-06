using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.GeneralUtils.TestDataHandling
{
	/// <summary>
	///     BaseTestDataReader class.
	/// </summary>
	public abstract class BaseTestDataReader : ITestDataReader
	{
		protected const string TestDataFolderName = "TestData";

		/// <summary>
		///     Reads the specified file.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="file">The file.</param>
		/// <param name="testDataIds">The test data ids.</param>
		/// <returns>Object with T type.</returns>
		public abstract T Read<T>(string file, params string[] testDataIds);

		/// <summary>
		///     Reads the specified assembly.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="assembly">The assembly.</param>
		/// <param name="file">The file.</param>
		/// <param name="testDataIds">The test data ids.</param>
		/// <returns>Object with T type.</returns>
		public abstract T Read<T>(Assembly assembly, string file, params string[] testDataIds);

		/// <summary>
		///     Loads the resource stream.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="shortResourceName">Short name of the resource.</param>
		/// <returns>The Stream.</returns>
		protected Stream LoadResourceStream(Assembly assembly, string shortResourceName)
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
		protected bool ResourceExists(string resourceName, Assembly executingAssembly)
		{
			var resourceNames = executingAssembly.GetManifestResourceNames();
			return resourceNames.Contains(resourceName);
		}
	}
}