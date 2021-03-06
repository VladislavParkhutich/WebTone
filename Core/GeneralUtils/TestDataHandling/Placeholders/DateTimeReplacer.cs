using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.SeleniumUtils;

namespace Core.GeneralUtils.TestDataHandling.Placeholders
{
	/// <summary>
	///     Date Time Replacer.
	/// </summary>
	public class DateTimeReplacer : IPlaceholderReplacer
	{
		/// <summary>
		///     Replaces the placeholders.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="isSpecific">Is data for specific type.</param>
		/// <returns>The result.</returns>
		public string ReplacePlaceholders(string input, bool isSpecific)
		{
			return ReplaceDateTimeNow(input, isSpecific);
		}

		/// <summary>
		///     Replaces the date time now.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="isMatter">Is data for MatterRequest type.</param>
		/// <returns>The result.</returns>
		private string ReplaceDateTimeNow(string input, bool isMatter)
		{
			const string DateTimeNowRegex =
				@"(<DateTime\.Now\(([A-Za-z:\-\\\/]*)\)(\s{0,1}([\+|\-])\s{0,1}(\d+)\s{0,1}(hour|day|month|year)s{0,1}|)*>)";
			var matches = new Regex(DateTimeNowRegex, RegexOptions.IgnoreCase).Matches(input);
			foreach (Match match in matches)
			{
				var operationPeriods = new List<OperationPeriodModel>();
				var operationsAmount = match.Groups[4].Captures.Count;
				for (var i = 0; i < operationsAmount; i++)
				{
					var operationPeriod = new OperationPeriodModel
					{
						Operation = match.Groups[4].Captures[i].Value,
						PeriodsAmount = match.Groups[4].Captures[i].Value + match.Groups[5].Captures[i].Value,
						Period =
							string.IsNullOrEmpty(match.Groups[6].Captures[i].Value)
								? null
								: (PeriodType?) Enum.Parse(typeof(PeriodType), match.Groups[6].Captures[i].Value, true)
					};
					operationPeriods.Add(operationPeriod);
				}

				var dateMatcherModel = new DateMatcherModel
				{
					WholeRegexpToken = match.Groups[1].Value,
					DateTimeFormat = match.Groups[2].Value,
					OperationPeriods = operationPeriods,
					IsMatterData = isMatter
				};
				input = input.ReplaceFirst(dateMatcherModel.WholeRegexpToken, dateMatcherModel.ToString());
			}

			return input;
		}
	}
}