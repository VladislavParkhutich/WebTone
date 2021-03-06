using System.Collections.ObjectModel;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories
{
	/// <summary>
	///     Base class to provide attributes handling for selenium locators.
	///     Added <see cref="DefaultElementLocator(ISearchContext, By)" /> to create locator using <see cref="By" />
	/// </summary>
	public class DefaultElementLocator : IElementLocator
	{
		private readonly By by;
		private readonly ISearchContext searchContext;

		private readonly bool shouldCache;

		private IWebElement cachedElement;

		private ReadOnlyCollection<IWebElement> cachedElementList;

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultElementLocator" /> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		/// <param name="attributesHandler">The attributes handler.</param>
		public DefaultElementLocator(ISearchContext searchContext, AttributesHandler attributesHandler)
		{
			this.searchContext = searchContext;
			shouldCache = attributesHandler.ShouldCache();
			by = attributesHandler.BuildBy();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultElementLocator" /> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		/// <param name="by">The by.</param>
		public DefaultElementLocator(ISearchContext searchContext, By by)
		{
			this.searchContext = searchContext;
			shouldCache = false;
			this.by = by;
		}

		/// <summary>
		///     Finds the element.
		/// </summary>
		/// <returns><see cref="IWebElement" /> instance.</returns>
		public virtual IWebElement FindElement()
		{
			if (cachedElement != null && shouldCache)
			{
				return cachedElement;
			}

			var element = searchContext.FindElement(by);
			if (shouldCache)
			{
				cachedElement = element;
			}

			return element;
		}

		/// <summary>
		///     Finds the elements.
		/// </summary>
		/// <returns>Collecteion of <see cref="IWebElement" />s.</returns>
		public virtual ReadOnlyCollection<IWebElement> FindElements()
		{
			if (cachedElementList != null && shouldCache)
			{
				return cachedElementList;
			}

			var elements = searchContext.FindElements(by);
			if (shouldCache)
			{
				cachedElementList = elements;
			}

			return elements;
		}
	}
}