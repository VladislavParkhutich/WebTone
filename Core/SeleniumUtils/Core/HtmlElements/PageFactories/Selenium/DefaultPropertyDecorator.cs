using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium.ProxyHandlers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium
{
	/// <summary>
	///     DefaultPropertyDecorator class.
	/// </summary>
	public class DefaultPropertyDecorator : IPropertyDecorator
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultPropertyDecorator" /> class.
		/// </summary>
		/// <param name="locatorFactory">The locator factory.</param>
		public DefaultPropertyDecorator(IElementLocatorFactory locatorFactory)
		{
			Factory = locatorFactory;
		}

		/// <summary>
		///     Gets the factory.
		/// </summary>
		/// <value>The factory.</value>
		protected IElementLocatorFactory Factory { get; private set; }

		/// <summary>
		///     Decorates the specified property info.
		/// </summary>
		/// <param name="propertyInfo">The property info.</param>
		/// <returns>The value.</returns>
		public virtual object Decorate(PropertyInfo propertyInfo)
		{
			if (!(typeof(IWebElement).IsAssignableFrom(propertyInfo.PropertyType) || IsDecoratableList(propertyInfo)))
			{
				return null;
			}

			var locator = Factory.CreateLocator(propertyInfo);
			if (locator == null)
			{
				return null;
			}

			if (typeof(IWebElement).IsAssignableFrom(propertyInfo.PropertyType))
			{
				return ProxyForLocator(locator);
			}
			if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
			{
				return ProxyForListLocator(locator);
			}
			return null;
		}

		/// <summary>
		///     Determines whether [is decoratable list] [the specified property].
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The value.</returns>
		private bool IsDecoratableList(PropertyInfo property)
		{
			var type = property.PropertyType;
			if (!typeof(IList<>).IsAssignableFrom(type))
			{
				return false;
			}

			var listType = type.GenericTypeArguments[0];

			if (typeof(IWebElement) != listType)
			{
				return false;
			}

			if (property.GetCustomAttributes(typeof(FindsByAttribute), false).Length == 0)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		///     Proxies for locator.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <returns>The value.</returns>
		private object ProxyForLocator(IElementLocator locator)
		{
			return WebElementProxyHandler.NewInstance(locator);
		}

		/// <summary>
		///     Proxies for list locator.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <returns>The value.</returns>
		private object ProxyForListLocator(IElementLocator locator)
		{
			return WebElementListProxyHandler.NewInstance(locator);
		}
	}
}