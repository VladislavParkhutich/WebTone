using System;

namespace Core.GeneralUtils.TestDataHandling.Placeholders
{
	/// <summary>
	///     The Guid Replacer.
	/// </summary>
	public class GuidReplacer : IPlaceholderReplacer
	{
		/// <summary>
		///     The Guid replace holder.
		/// </summary>
		/// <param name="input">The input string of the Json file.</param>
		/// <param name="isSpecific">Is data for specific type.</param>
		/// <returns>The modified input.</returns>
		public string ReplacePlaceholders(string input, bool isSpecific)
		{
			input = input.Replace("<GuidNew>", Guid.NewGuid().ToString("N"));
			return input.Replace("<GuidEmpty>", Guid.Empty.ToString("N"));
		}
	}
}