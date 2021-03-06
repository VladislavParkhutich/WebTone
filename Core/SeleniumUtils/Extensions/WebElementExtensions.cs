using System.Globalization;
using System.Linq;
using Core.GeneralUtils.Container;
using Core.SeleniumUtils.Core;
using Core.SeleniumUtils.Core.Constants;
using Microsoft.Practices.Unity;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Extensions
{
	/// <summary>
	///     WebElementExtensions class.
	/// </summary>
	public static class WebElementExtensions
	{
		private static readonly string keyA = "a";

		/// <summary>
		///     Sets the text.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="value">The value.</param>
		public static void SetText(this IWebElement element, string value)
		{
			if (element.TagName == TagNames.Input || element.TagName == TagNames.TextArea)
			{
				element.Clear();
			}
			else
			{
				element.SendKeys(Keys.LeftControl + keyA);
				element.SendKeys(Keys.Delete);
			}

			if (string.IsNullOrEmpty(value))
			{
				return;
			}

			element.SendKeys(value);

			var browser = UnityBootstrapper.Container.Resolve<Browser>();
			Waiter.Try(
				() =>
					browser.ExecuteJavaScript(
						string.Format(CultureInfo.InvariantCulture, "$(arguments[0]).{0}();", JavaScriptEvents.KeyUp), element));
		}

		/// <summary>
		///     Clears the text for element.
		/// </summary>
		/// <param name="element">The element.</param>
		public static void ClearText(this IWebElement element)
		{
			Enumerable.Range(0, element.GetAttribute("value").Length).ToList().ForEach(arg => element.SendKeys(Keys.Backspace));
		}

		/// <summary>
		///     Finds the inner web element safely.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="by">The by.</param>
		/// <returns>The value.</returns>
		public static IWebElement FindInnerWebElementSafely(this ISearchContext element, By by)
		{
			try
			{
				return element.FindElement(by);
			}
			catch (NoSuchElementException)
			{
				return null;
			}
		}
	}
}