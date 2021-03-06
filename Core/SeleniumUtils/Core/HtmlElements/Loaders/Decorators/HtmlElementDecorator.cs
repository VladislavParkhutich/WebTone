using System;
using System.Reflection;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium;
using Core.SeleniumUtils.Core.HtmlElements.Utils;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators
{
	/// <summary>
	///     HtmlElementDecorator class.
	/// </summary>
	public class HtmlElementDecorator : DefaultPropertyDecorator
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="HtmlElementDecorator" /> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		public HtmlElementDecorator(ISearchContext searchContext) : this(new HtmlElementLocatorFactory(searchContext))
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HtmlElementDecorator" /> class.
		/// </summary>
		/// <param name="locatorFactory">The locator factory.</param>
		public HtmlElementDecorator(IElementLocatorFactory locatorFactory) : base(locatorFactory)
		{
		}

		/// <summary>
		///     Decorates the specified property info.
		/// </summary>
		/// <param name="propertyInfo">The property info.</param>
		/// <returns>The value.</returns>
		public override object Decorate(PropertyInfo propertyInfo)
		{
			if (!IsDecoratableProperty(propertyInfo))
			{
				return null;
			}

			var locator = Factory.CreateLocator(propertyInfo);
			if (locator == null)
			{
				return null;
			}

			var elementName = HtmlElementUtils.GetElementName(propertyInfo);

			if (HtmlElementUtils.IsTypifiedElement(propertyInfo))
			{
				var typifiedElementType = propertyInfo.PropertyType;
				return DecorateTypifiedElement(typifiedElementType, locator, elementName);
			}
			if (HtmlElementUtils.IsHtmlElement(propertyInfo))
			{
				var htmlElementType = propertyInfo.PropertyType;
				return DecorateHtmlElement(htmlElementType, locator, elementName);
			}
			if (HtmlElementUtils.IsWebElement(propertyInfo))
			{
				return DecorateWebElement(locator, elementName);
			}
			if (HtmlElementUtils.IsTypifiedElementList(propertyInfo))
			{
				var typifiedElementType = HtmlElementUtils.GetGenericParameterType(propertyInfo);
				return DecorateTypifiedElementList(propertyInfo.PropertyType, typifiedElementType, locator, elementName);
			}
			if (HtmlElementUtils.IsHtmlElementList(propertyInfo))
			{
				var htmlElementType = HtmlElementUtils.GetGenericParameterType(propertyInfo);
				return DecorateHtmlElementList(propertyInfo.PropertyType, htmlElementType, locator, elementName);
			}
			if (HtmlElementUtils.IsWebElementList(propertyInfo))
			{
				return DecorateWebElementList(locator, elementName);
			}

			return null;
		}

		/// <summary>
		///     Determines whether [is decoratable property] [the specified field].
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns>The value.</returns>
		private bool IsDecoratableProperty(PropertyInfo field)
		{
			// TODO Protecting wrapped element from initialization basing on its name is unsafe. Think of a better way.
			if (HtmlElementUtils.IsWebElement(field) && field.Name != "wrappedElement")
			{
				return true;
			}

			return HtmlElementUtils.IsWebElementList(field) || HtmlElementUtils.IsHtmlElement(field) ||
			       HtmlElementUtils.IsHtmlElementList(field) || HtmlElementUtils.IsTypifiedElement(field) ||
			       HtmlElementUtils.IsTypifiedElementList(field);
		}

		/// <summary>
		///     Decorates the typified element.
		/// </summary>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="locator">The locator.</param>
		/// <param name="elementName">Name of the element.</param>
		/// <returns>The value.</returns>
		private TypifiedElement DecorateTypifiedElement(Type elementType, IElementLocator locator, string elementName)
		{
			// Create typified element and initialize it with WebElement proxy
			var elementToWrap = HtmlElementFactory.CreateNamedProxyForWebElement(locator, elementName);
			var typifiedElementInstance = HtmlElementFactory.CreateTypifiedElementInstance(elementType, elementToWrap);
			typifiedElementInstance.Name = elementName;
			return typifiedElementInstance;
		}

		/// <summary>
		///     Decorates the HTML element.
		/// </summary>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="locator">The locator.</param>
		/// <param name="elementName">Name of the element.</param>
		/// <returns>The value.</returns>
		private HtmlElement DecorateHtmlElement(Type elementType, IElementLocator locator, string elementName)
		{
			// Create block and initialize it with WebElement proxy
			var elementToWrap = HtmlElementFactory.CreateNamedProxyForWebElement(locator, elementName);
			var htmlElementInstance = HtmlElementFactory.CreateHtmlElementInstance(elementType);
			htmlElementInstance.WrappedElement = elementToWrap;
			htmlElementInstance.Name = elementName;

			// Recursively initialize elements of the block
			PageFactory.InitElements(new HtmlElementDecorator(elementToWrap), htmlElementInstance);
			return htmlElementInstance;
		}

		/// <summary>
		///     Decorates the web element.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="elementName">Name of the element.</param>
		/// <returns>The value.</returns>
		private IWebElement DecorateWebElement(IElementLocator locator, string elementName)
		{
			return HtmlElementFactory.CreateNamedProxyForWebElement(locator, elementName);
		}

		/// <summary>
		///     Decorates the typified element list.
		/// </summary>
		/// <param name="listType">Type of the list.</param>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="locator">The locator.</param>
		/// <param name="listName">Name of the list.</param>
		/// <returns>The value.</returns>
		private object DecorateTypifiedElementList(Type listType, Type elementType, IElementLocator locator, string listName)
		{
			return HtmlElementFactory.CreateNamedProxyForTypifiedElementList(listType, elementType, locator, listName);
		}

		/// <summary>
		///     Decorates the HTML element list.
		/// </summary>
		/// <param name="listType">Type of the list.</param>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="locator">The locator.</param>
		/// <param name="listName">Name of the list.</param>
		/// <returns>The value.</returns>
		private object DecorateHtmlElementList(Type listType, Type elementType, IElementLocator locator, string listName)
		{
			return HtmlElementFactory.CreateNamedProxyForHtmlElementList(listType, elementType, locator, listName);
		}

		/// <summary>
		///     Decorates the web element list.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="listName">Name of the list.</param>
		/// <returns>The value.</returns>
		private object DecorateWebElementList(IElementLocator locator, string listName)
		{
			return HtmlElementFactory.CreateNamedProxyForWebElementList(locator, listName);
		}
	}
}