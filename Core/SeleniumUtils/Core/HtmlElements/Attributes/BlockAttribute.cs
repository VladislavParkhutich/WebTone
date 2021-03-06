using System;
using System.ComponentModel;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.SeleniumUtils.Core.HtmlElements.Attributes
{
	/// <summary>
	///     BlockAttribute class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum,
		Inherited = false)]
	public sealed class BlockAttribute : Attribute
	{
		/// <summary>
		///     Gets or sets the how.
		/// </summary>
		/// <value>The how.</value>
		[DefaultValue(How.Id)]
		public How How { get; set; }

		/// <summary>
		///     Gets or sets the using.
		/// </summary>
		/// <value>The using.</value>
		public string Using { get; set; }

		/// <summary>
		///     Gets or sets the priority.
		/// </summary>
		/// <value>The priority.</value>
		[DefaultValue(0)]
		public int Priority { get; set; }

		/// <summary>
		///     Gets or sets the type of the custom finder.
		/// </summary>
		/// <value>The type of the custom finder.</value>
		public Type CustomFinderType { get; set; }

		/// <summary>
		///     Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public FindsByAttribute Value
		{
			get
			{
				var result = new FindsByAttribute
				{
					How = How,
					Using = Using,
					Priority = Priority,
					CustomFinderType = CustomFinderType
				};
				return result;
			}
		}
	}
}