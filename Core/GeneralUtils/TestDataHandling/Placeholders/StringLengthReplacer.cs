using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Core.SeleniumUtils;

namespace Core.GeneralUtils.TestDataHandling.Placeholders
{
	/// <summary>
	///     String Length Replacer.
	/// </summary>
	public class StringLengthReplacer : IPlaceholderReplacer
	{
		/// <summary>
		///     Dictionary with pairs: key = regex, value = instanse of method to random string base on its length.
		/// </summary>
		private readonly Dictionary<string, Func<int, string>> regexReplacers = new Dictionary<string, Func<int, string>>
		{
			{@"(<AlphabeticalString\((\d+)\)>)", RandomUtils.RandomizeAlphabeticalString},
			{@"(<AlphanumericString\((\d+)\)>)", RandomUtils.RandomizeAlphanumericString},
			{@"(<NumericString\((\d+)\)>)", RandomUtils.RandomizeNumericString},
			{@"(<SpecialSymbolsString\((\d+)\)>)", RandomUtils.RandomizeSpecialSymbolsString},
			{@"(<SpacesString\((\d+)\)>)", RandomUtils.RandomizeSpacesString},
			{@"(<AlphanumericStringWithSpecialSymbols\((\d+)\)>)", RandomUtils.RandomizeAlphanumericStringWithSpecialSymbols}
		};

		/// <summary>
		///     Replaces the placeholders.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="isSpecific">Is data for specific type.</param>
		/// <returns>String value.</returns>
		public string ReplacePlaceholders(string input, bool isSpecific)
		{
			return regexReplacers.Aggregate(input,
				(current, replacer) => ReplaceStringLength(replacer.Key, current, replacer.Value));
		}

		/// <summary>
		///     Replaces the length of the string.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="text">The text.</param>
		/// <param name="randomLength">The random length.</param>
		/// <returns>String value.</returns>
		private string ReplaceStringLength(string pattern, string text, Func<int, string> randomLength)
		{
			var matches = new Regex(pattern, RegexOptions.IgnoreCase).Matches(text);
			foreach (Match match in matches)
			{
				var wholeToken = match.Groups[1].Value;
				var length = match.Groups[2].Value;
				var newTokenValue = randomLength(Convert.ToInt32(length, CultureInfo.InvariantCulture));
				text = text.ReplaceFirst(wholeToken, newTokenValue);
			}

			return text;
		}
	}
}