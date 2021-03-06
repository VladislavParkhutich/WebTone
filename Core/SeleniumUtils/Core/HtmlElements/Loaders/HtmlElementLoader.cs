using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium;
using Core.SeleniumUtils.Core.HtmlElements.Utils;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Loaders
{
	/// <summary>
	///     HtmlElementLoader class.
	/// </summary>
	public static class HtmlElementLoader
	{
		/// <summary>
		///     Creates the specified driver.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="driver">The driver.</param>
		/// <returns>The value.</returns>
		public static T Create<T>(IWebDriver driver)
		{
			object result;
			var type = typeof(T);
			if (HtmlElementUtils.IsHtmlElement(type))
			{
				result = HtmlElementFactory.CreateHtmlElementInstance(type);
				PopulateHtmlElement((HtmlElement) result, new HtmlElementLocatorFactory(driver));
			}
			else
			{
				result = (T) HtmlElementFactory.CreatePageObjectInstance(type, driver);
				PopulatePageObject(result, new HtmlElementLocatorFactory(driver));
			}

			return (T) result;
		}

		/// <summary>
		///     Populates the specified instance.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="driver">The driver.</param>
		public static void Populate(object instance, IWebDriver driver)
		{
			if (HtmlElementUtils.IsHtmlElement(instance))
			{
				var htmlElement = (HtmlElement) instance;
				PopulateHtmlElement(htmlElement, driver);
			}
			else
			{
				PopulatePageObject(instance, driver);
			}
		}

		/// <summary>
		///     Creates the HTML element.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="searchContext">The search context.</param>
		/// <returns>The value.</returns>
		public static T CreateHtmlElement<T>(ISearchContext searchContext) where T : HtmlElement
		{
			var htmlElementInstance = (T) HtmlElementFactory.CreateHtmlElementInstance(typeof(T));
			PopulateHtmlElement(htmlElementInstance, new HtmlElementLocatorFactory(searchContext));
			return htmlElementInstance;
		}

		/// <summary>
		///     Creates the page object.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="driver">The driver.</param>
		/// <returns>The value.</returns>
		public static T CreatePageObject<T>(IWebDriver driver)
		{
			var page = (T) HtmlElementFactory.CreatePageObjectInstance(typeof(T), driver);
			PopulatePageObject(page, new HtmlElementLocatorFactory(driver));
			return page;
		}

		/// <summary>
		///     Populates the HTML element.
		/// </summary>
		/// <param name="htmlElement">The HTML element.</param>
		/// <param name="searchContext">The search context.</param>
		public static void PopulateHtmlElement(HtmlElement htmlElement, ISearchContext searchContext)
		{
			PopulateHtmlElement(htmlElement, new HtmlElementLocatorFactory(searchContext));
		}

		/// <summary>
		///     Populates the HTML element.
		/// </summary>
		/// <param name="htmlElement">The HTML element.</param>
		/// <param name="locatorFactory">The locator factory.</param>
		private static void PopulateHtmlElement(HtmlElement htmlElement, CustomElementLocatorFactory locatorFactory)
		{
			var htmlElementType = htmlElement.GetType();

			// Create locator that will handle Block annotation
			var locator = locatorFactory.CreateLocator(htmlElementType);

			// The next line from Java code located here till I find a solution to replace ClassLoader
			// ClassLoader htmlElementClassLoader = htmlElement.getClass().getClassLoader();

			// Initialize block with IWebElement proxy and set its name
			var elementName = HtmlElementUtils.GetElementName(htmlElementType);

			var elementToWrap = HtmlElementFactory.CreateNamedProxyForWebElement(locator, elementName);
			htmlElement.WrappedElement = elementToWrap;
			htmlElement.Name = elementName;

			// Initialize elements of the block
			PageFactory.InitElements(new HtmlElementDecorator(elementToWrap), htmlElement);
		}

		/// <summary>
		///     Populates the page object.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="driver">The driver.</param>
		public static void PopulatePageObject(object page, IWebDriver driver)
		{
			PopulatePageObject(page, new HtmlElementLocatorFactory(driver));
		}

		/// <summary>
		///     Populates the page object.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="locatorFactory">The locator factory.</param>
		public static void PopulatePageObject(object page, CustomElementLocatorFactory locatorFactory)
		{
			PageFactory.InitElements(new HtmlElementDecorator(locatorFactory), page);
		}
	}
}