namespace Core.GeneralUtils.TestDataHandling.Placeholders
{
	/// <summary>
	///     IPlaceholderReplacer interface.
	/// </summary>
	public interface IPlaceholderReplacer
	{
		/// <summary>
		///     Replaces the placeholders.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="isSpecific">Is data for specific type.</param>
		/// <returns>String value.</returns>
		string ReplacePlaceholders(string input, bool isSpecific);
	}
}