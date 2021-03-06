using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Core.SeleniumUtils;

namespace Core.GeneralUtils.TestDataHandling.Placeholders
{
	/// <summary>
	///     The numeric range replacer.
	/// </summary>
	public class NumericRangeReplacer : IPlaceholderReplacer
	{
		/// <summary>
		///     Dictionary with pairs: key = regex, value = instance of method to random string base on with given range.
		/// </summary>
		private readonly Dictionary<string, Func<int, int, decimal>> regexNumericReplacers =
			new Dictionary<string, Func<int, int, decimal>> {{@"(<Decimal\((\d+,\d+)\)>)", RandomUtils.RandomDecimal}};

		/// <summary>
		///     The replace placeholders.
		/// </summary>
		/// <param name="input">
		///     The input.
		/// </param>
		/// <param name="isSpecific">
		///     Is data for specific type.
		/// </param>
		/// <returns>
		///     Parsed text with replaced decimal.
		/// </returns>
		public string ReplacePlaceholders(string input, bool isSpecific)
		{
			return regexNumericReplacers.Aggregate(input,
				(current, replacer) => ReplaceStringRange(replacer.Key, current, replacer.Value));
		}

		/// <summary>
		///     The replace string range.
		/// </summary>
		/// <param name="pattern">
		///     The pattern.
		/// </param>
		/// <param name="text">
		///     Text to parse.
		/// </param>
		/// <param name="numberFromRange">
		///     Function returning a random value based on passed in range (min, max).
		/// </param>
		/// <returns>
		///     Parsed text with updated values is returned.
		/// </returns>
		private string ReplaceStringRange(string pattern, string text, Func<int, int, decimal> numberFromRange)
		{
			var matches = new Regex(pattern, RegexOptions.IgnoreCase).Matches(text);
			foreach (Match match in matches)
			{
				var wholeToken = match.Groups[1].Value;
				var range = match.Groups[2].ToString().Split(',');

				if (range.Length > 1)
				{
					var newTokenValue = numberFromRange(Convert.ToInt32(range[0], CultureInfo.InvariantCulture),
						Convert.ToInt32(range[1], CultureInfo.InvariantCulture));
					text = text.ReplaceFirst(wholeToken, newTokenValue.ToString(CultureInfo.InvariantCulture));
				}
			}

			return text;
		}
	}
}