using System;
using System.Collections.Generic;
using System.Globalization;

namespace Core.GeneralUtils.TestDataHandling.Placeholders
{
	/// <summary>
	///     Period Type enum.
	/// </summary>
	public enum PeriodType
	{
		/// <summary>
		///     The Hour.
		/// </summary>
		Hour,

		/// <summary>
		///     The Day.
		/// </summary>
		Day,

		/// <summary>
		///     The Month.
		/// </summary>
		Month,

		/// <summary>
		///     The Year.
		/// </summary>
		Year
	}

	/// <summary>
	///     Date Matcher Model class.
	/// </summary>
	public class DateMatcherModel
	{
		/// <summary>
		///     Gets or sets the whole regexp token.
		/// </summary>
		/// <value>The whole regexp token.</value>
		public string WholeRegexpToken { get; set; }

		/// <summary>
		///     Gets or sets the date time format.
		/// </summary>
		/// <value>The date time format.</value>
		public string DateTimeFormat { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether this instance is matter data.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance is matter data; otherwise, <c>false</c>.
		/// </value>
		public bool IsMatterData { get; set; }

		/// <summary>
		///     Gets or sets the operation periods.
		/// </summary>
		/// <value>The operation periods.</value>
		public IList<OperationPeriodModel> OperationPeriods { get; set; }

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			const string DefaultDateTimeFormat = "yyyy-MM-dd";

			var dateTimeValue = DateTime.Now;
			foreach (var operationPeriod in OperationPeriods)
			{
				if (!string.IsNullOrEmpty(operationPeriod.Operation) && operationPeriod.Period != null)
				{
					var periodsAmountAsInt = Convert.ToInt32(operationPeriod.PeriodsAmount, CultureInfo.InvariantCulture);
					switch (operationPeriod.Period)
					{
						case PeriodType.Hour:
							dateTimeValue = dateTimeValue.AddHours(periodsAmountAsInt);
							break;
						case PeriodType.Day:
							dateTimeValue = dateTimeValue.AddDays(periodsAmountAsInt);
							break;
						case PeriodType.Month:
							dateTimeValue = dateTimeValue.AddMonths(periodsAmountAsInt);
							break;
						case PeriodType.Year:
							dateTimeValue = dateTimeValue.AddYears(periodsAmountAsInt);
							break;
					}
				}
			}

			return dateTimeValue.ToString(string.IsNullOrEmpty(DateTimeFormat) ? DefaultDateTimeFormat : DateTimeFormat,
				CultureInfo.InvariantCulture);
		}
	}
}