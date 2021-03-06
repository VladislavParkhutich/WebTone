using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium
{
	/// <summary>
	///     IElementLocator interface.
	/// </summary>
	public interface IElementLocator
	{
		/// <summary>
		///     Finds the element.
		/// </summary>
		/// <returns>The result.</returns>
		IWebElement FindElement();

		/// <summary>
		///     Finds the elements.
		/// </summary>
		/// <returns>The result.</returns>
		ReadOnlyCollection<IWebElement> FindElements();
	}
}