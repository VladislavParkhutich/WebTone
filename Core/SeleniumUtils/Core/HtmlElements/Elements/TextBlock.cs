using System;
using System.Globalization;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     The class that represents read-only text, e.g. label.
	/// </summary>
	public class TextBlock : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TextBlock" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public TextBlock(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				var textContentValue = WrappedElement.GetAttribute("textContent");
				if (!string.IsNullOrEmpty(textContentValue))
				{
					return textContentValue;
				}

				return WrappedElement.GetAttribute("value") ?? WrappedElement.Text;
			}
		}

		/// <summary>
		///     Sends the keys not allowed.
		/// </summary>
		/// <param name="keys">The keys sequesnce.</param>
		/// <exception cref="System.ArgumentException">
		///     The text of element + this.Name +  was changed from ' + initialTextValue +  to ' +.
		///     keys + '.
		/// </exception>
		public void SendKeysNotAllowed(string keys)
		{
			var initialTextValue = Text;
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Type '{0}' to input '{1}'", keys, Name));
			WrappedElement.SendKeys(keys);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
			if (initialTextValue.Equals(keys))
			{
				throw new ArgumentException("The text of element" + Name + " was changed from '" + initialTextValue + " to '" + keys +
				                            "'");
			}
		}
	}
}