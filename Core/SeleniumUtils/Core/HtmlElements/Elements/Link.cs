using System.Globalization;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     Link class.
	/// </summary>
	public class Link : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Link" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Link(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return WrappedElement.Text; }
		}

		/// <summary>
		///     Gets the reference.
		/// </summary>
		/// <returns>The result.</returns>
		public string GetReference()
		{
			return WrappedElement.GetAttribute("href");
		}

		/// <summary>
		///     Clicks this instance.
		/// </summary>
		public void Click()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click the link '{0}'", Name));
			WrappedElement.Click();
		}

		/// <summary>
		///     Clicks the and wait ajax.
		/// </summary>
		public void ClickAndWaitAjax()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click the link '{0}'", Name));
			WrappedElement.Click();
			Browser.WaitAjax();
		}
	}
}