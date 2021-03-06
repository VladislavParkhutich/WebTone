using System;
using System.Reflection;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories
{
	/// <summary>
	///     CustomElementLocatorFactory class.
	/// </summary>
	public abstract class CustomElementLocatorFactory : IElementLocatorFactory
	{
		/// <summary>
		///     Creates the locator.
		/// </summary>
		/// <param name="propertyInfo">The property info.</param>
		/// <returns>The result.</returns>
		public virtual IElementLocator CreateLocator(PropertyInfo propertyInfo)
		{
			return null;
		}

		/// <summary>
		///     Creates the locator.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		public virtual IElementLocator CreateLocator(Type type)
		{
			return null;
		}
	}
}