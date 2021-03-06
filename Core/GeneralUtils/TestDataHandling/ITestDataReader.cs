using System.Reflection;

namespace Core.GeneralUtils.TestDataHandling
{
	/// <summary>
	///     ITestDataReader interface.
	/// </summary>
	public interface ITestDataReader
	{
		/// <summary>
		///     Reads the specified file.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="file">The file.</param>
		/// <param name="testDataIds">The test data ids.</param>
		/// <returns>The object of T type.</returns>
		T Read<T>(string file, params string[] testDataIds);

		/// <summary>
		///     Reads the specified assembly.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="assembly">The assembly.</param>
		/// <param name="file">The file.</param>
		/// <param name="testDataIds">The test data ids.</param>
		/// <returns>The object of T type.</returns>
		T Read<T>(Assembly assembly, string file, params string[] testDataIds);
	}
}