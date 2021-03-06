using System;
using System.Globalization;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.HtmlElements.Exceptions;
using Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators.ProxyHandlers;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium;
using Core.SeleniumUtils.Core.HtmlElements.Utils;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators
{
	/// <summary>
	///     HtmlElementFactory class.
	/// </summary>
	public static class HtmlElementFactory
	{
		/// <summary>
		///     Creates the HTML element instance.
		/// </summary>
		/// <param name="elementType">Type of the element.</param>
		/// <returns>The value.</returns>
		public static HtmlElement CreateHtmlElementInstance(Type elementType)
		{
			if (typeof(HtmlElement).IsAssignableFrom(elementType))
			{
				return HtmlElementUtils.NewInstance<HtmlElement>(elementType);
			}

			throw new HtmlElementsException(string.Format(CultureInfo.InvariantCulture,
				"Type '{0}' is not a derivative type of 'HtmlElement'", elementType));
		}

		/// <summary>
		///     Creates the typified element instance.
		/// </summary>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="elementToWrap">The element to wrap.</param>
		/// <returns>The value.</returns>
		public static TypifiedElement CreateTypifiedElementInstance(Type elementType, IWebElement elementToWrap)
		{
			if (typeof(TypifiedElement).IsAssignableFrom(elementType))
			{
				return HtmlElementUtils.NewInstance<TypifiedElement>(elementType, elementToWrap);
			}

			throw new HtmlElementsException(string.Format(CultureInfo.InvariantCulture,
				"Type '{0}' isn't a derivative type of 'TypifiedElement'", elementType));
		}

		/// <summary>
		///     Creates the page object instance.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="driver">The driver.</param>
		/// <returns>The value.</returns>
		public static object CreatePageObjectInstance(Type type, IWebDriver driver)
		{
			return HtmlElementUtils.NewInstance<object>(type, driver);
		}

		/// <summary>
		///     Creates the named proxy for web element.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="elementName">Name of the element.</param>
		/// <returns>The value.</returns>
		public static IWebElement CreateNamedProxyForWebElement(IElementLocator locator, string elementName)
		{
			return WebElementNamedProxyHandler.NewInstance(locator, elementName);
		}

		/// <summary>
		///     Creates the named proxy for web element list.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="listName">Name of the list.</param>
		/// <returns>The value.</returns>
		internal static object CreateNamedProxyForWebElementList(IElementLocator locator, string listName)
		{
			return WebElementListNamedProxyHandler.NewInstance(locator, listName);
		}

		/// <summary>
		///     Creates the named proxy for typified element list.
		/// </summary>
		/// <param name="listType">Type of the list.</param>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="locator">The locator.</param>
		/// <param name="listName">Name of the list.</param>
		/// <returns>The value.</returns>
		internal static object CreateNamedProxyForTypifiedElementList(Type listType, Type elementType, IElementLocator locator,
			string listName)
		{
			return TypifiedElementListNamedProxyHandler.NewInstance(listType, elementType, locator, listName);
		}

		/// <summary>
		///     Creates the named proxy for HTML element list.
		/// </summary>
		/// <param name="listType">Type of the list.</param>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="locator">The locator.</param>
		/// <param name="listName">Name of the list.</param>
		/// <returns>The value.</returns>
		internal static object CreateNamedProxyForHtmlElementList(Type listType, Type elementType, IElementLocator locator,
			string listName)
		{
			return HtmlElementListNamedProxyHandler.NewInstance(listType, elementType, locator, listName);
		}
	}
}