using Core.SeleniumUtils.Core.Objects;

namespace Core.SeleniumUtils.WindowsHandling.Windows
{
	/// <summary>
	///     ClosingWindow class.
	/// </summary>
	public abstract class ClosingWindow : UIInfrastructureObject
	{
		private readonly string[] PossibleNames;

		/// <summary>
		///     Initializes a new instance of the <see cref="ClosingWindow" /> class.
		/// </summary>
		/// <param name="possibleNames">The possible names.</param>
		protected ClosingWindow(params string[] possibleNames)
		{
			PossibleNames = possibleNames;
		}

		/// <summary>
		///     Closes this instance.
		/// </summary>
		public abstract void Close();

		/// <summary>
		///     Gets the name of the possible.
		/// </summary>
		/// <returns>Strings array.</returns>
		protected string[] GetPossibleName()
		{
			return PossibleNames;
		}
	}
}