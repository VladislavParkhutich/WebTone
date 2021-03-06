using System;
using System.Reflection;
using Core.SeleniumUtils.Core.HtmlElements.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories;
using Core.SeleniumUtils.Core.HtmlElements.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators
{
	/// <summary>
	///     HtmlElementPropertyAttributesHandler class.
	/// </summary>
	public class HtmlElementPropertyAttributesHandler : DefaultPropertyAttributesHandler
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="HtmlElementPropertyAttributesHandler" /> class.
		/// </summary>
		/// <param name="property">The property.</param>
		public HtmlElementPropertyAttributesHandler(PropertyInfo property) : base(property)
		{
		}

		/// <summary>
		///     Builds the by.
		/// </summary>
		/// <returns>The value.</returns>
		public override By BuildBy()
		{
			if (HtmlElementUtils.IsHtmlElement(Property) || HtmlElementUtils.IsHtmlElementList(Property))
			{
				var type = HtmlElementUtils.IsHtmlElementList(Property)
					? HtmlElementUtils.GetGenericParameterType(Property)
					: Property.PropertyType;
				return BuildByFromHtmlElementAttributes(type);
			}
			return base.BuildBy();
		}

		/// <summary>
		///     Builds the by from HTML element attributes.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The value.</returns>
		private By BuildByFromHtmlElementAttributes(Type type)
		{
			var findBys = (FindsByAttribute[]) Property.GetCustomAttributes(typeof(FindsByAttribute));
			if (findBys.Length > 0)
			{
				return BuildByFromFindsByValues(findBys);
			}

			var blocks = (BlockAttribute[]) type.GetCustomAttributes(typeof(BlockAttribute), true);
			if (blocks.Length > 0)
			{
				var block = blocks[0];
				var findsBy = block.Value;
				return BuildByFromFindsBy(findsBy);
			}

			return BuildByFromDefault();
		}
	}
}