using System.Globalization;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     WebDriver button.
	/// </summary>
	public class Button : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Button" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Button(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Gets Button inner text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return WrappedElement.Text; }
		}

		/// <summary>
		///     Method for click on button.
		/// </summary>
		public void Click()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click the button '{0}'", Name));
			WrappedElement.Click();
		}

		/// <summary>
		///     Method to click on button and wait all ajax requests complete.
		/// </summary>
		public void ClickAndWaitAjax()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click the button '{0}'", Name));
			WrappedElement.Click();
			Browser.WaitAjax();
		}
	}
}