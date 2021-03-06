using System.Collections.Generic;
using System.Linq;

namespace Core.GeneralUtils.TestDataHandling.Placeholders
{
	/// <summary>
	///     The placeholder replacers container.
	/// </summary>
	public class PlaceholderReplacersContainer
	{
		private readonly List<IPlaceholderReplacer> propertyReplacers = new List<IPlaceholderReplacer>();

		/// <summary>
		///     Initializes a new instance of the <see cref="PlaceholderReplacersContainer" /> class.
		/// </summary>
		public PlaceholderReplacersContainer()
		{
			propertyReplacers.Add(new StringLengthReplacer());
			propertyReplacers.Add(new DateTimeReplacer());
			propertyReplacers.Add(new GuidReplacer());
			propertyReplacers.Add(new NumericRangeReplacer());
		}

		/// <summary>
		///     The replace tokens.
		/// </summary>
		/// <param name="input">
		///     The input.
		/// </param>
		/// <param name="isSpecific">
		///     Is data for specific type.
		/// </param>
		/// <returns>
		///     The <see cref="string" />.
		/// </returns>
		public string ReplaceTokens(string input, bool isSpecific)
		{
			return propertyReplacers.Aggregate(input,
				(current, placeholderReplacer) => placeholderReplacer.ReplacePlaceholders(current, isSpecific));
		}
	}
}