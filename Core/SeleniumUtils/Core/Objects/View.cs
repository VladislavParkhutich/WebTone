using System;
using System.Collections.Generic;
using Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.Objects
{
	/// <summary>
	///     View class.
	/// </summary>
	public abstract class View : UIInfrastructureObject
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="View" /> class. PageObject HtmlElements fields using
		///     <see cref="HtmlElementDecorator" />.
		/// </summary>
		protected View()
		{
			PageFactory.InitElements(new HtmlElementDecorator(Browser.WebDriver.OriginalDriver), this);
		}

		#region Methods

		/// <summary>
		///     Refreshes the page.
		/// </summary>
		public void RefreshPage()
		{
			Browser.Refresh();
			Browser.WaitReadyState();
			Browser.WaitAjax();
		}

		/// <summary>
		///     Find element and check it is enabled with the timeout defined in the App.config.
		/// </summary>
		/// <param name="by">By Locator.</param>
		/// <returns>IWebElement instance.</returns>
		[Obsolete("FindEnabledElement is deprecated, please create element properties using [FindsBy] attribute")]
		protected virtual IWebElement FindEnabledElement(By by)
		{
			return Browser.WebDriver.FindElement(by);
		}

		/// <summary>
		///     Find elements list and check they are enabled with the timeout defined in the App.config.
		/// </summary>
		/// <param name="by">By Locator.</param>
		/// <returns>IWebElement instance.</returns>
		[Obsolete("FindEnabledElements is deprecated, please create element properties using [FindsBy] attribute")]
		protected virtual IList<IWebElement> FindEnabledElements(By by)
		{
			return Browser.WebDriver.FindElements(by);
		}

		/// <summary>
		///     Find element and check it is displayed with the timeout defined in the App.config.
		/// </summary>
		/// <param name="by">By Locator.</param>
		/// <returns>IWebElement instance.</returns>
		[Obsolete("FindVisibleElement is deprecated, please create element properties using [FindsBy] attribute")]
		protected virtual IWebElement FindVisibleElement(By by)
		{
			return Browser.WebDriver.FindElement(by);
		}

		/// <summary>
		///     Find elements list and check they are displayed with the timeout defined in the App.config.
		/// </summary>
		/// <param name="by">By Locator.</param>
		/// <returns>IWebElement instance.</returns>
		[Obsolete("FindVisibleElements is deprecated, please create element properties using [FindsBy] attribute")]
		protected virtual IList<IWebElement> FindVisibleElements(By by)
		{
			return Browser.WebDriver.FindElements(by);
		}

		#endregion
	}
}