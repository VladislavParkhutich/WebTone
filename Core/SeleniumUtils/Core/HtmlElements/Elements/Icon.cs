using System.Globalization;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     The Icon UI element.
	/// </summary>
	public class Icon : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Icon" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Icon(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Performs a Click on the icon.
		/// </summary>
		public void Click()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click the icon '{0}'", Name));
			WrappedElement.Click();
		}

		/// <summary>
		///     Performs a Click on the icon using java script.
		/// </summary>
		public void ClickUsingJS()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click the icon '{0}'", Name));
			Browser.ExecuteJavaScript("arguments[0].click()", OriginalWebElement);
		}
	}
}