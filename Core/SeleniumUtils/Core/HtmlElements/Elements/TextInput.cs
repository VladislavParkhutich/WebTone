using System.Globalization;
using System.Linq;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     TextInput class.
	/// </summary>
	public class TextInput : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TextInput" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public TextInput(IWebElement element) : base(element)
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
				if ("textarea" == WrappedElement.TagName)
				{
					return WrappedElement.Text;
				}

				var enteredText = WrappedElement.GetAttribute("value");
				if (enteredText == null)
				{
					return string.Empty;
				}

				return enteredText;
			}
		}

		/// <summary>
		///     The clear using key and send keys.
		/// </summary>
		/// <param name="keys">
		///     The keys.
		/// </param>
		/// <param name="clearKey">
		///     The clear key.
		/// </param>
		private void ClearUsingKeyAndSendKeys(string keys, string clearKey)
		{
			TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text with {0} and type '{1}' to input '{2}'",
				clearKey, keys, Name));
			Enumerable.Range(0, Text.Length).ToList().ForEach(arg => WrappedElement.SendKeys(clearKey));

			WrappedElement.Clear();

			WrappedElement.SendKeys(keys);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
		}

		/// <summary>
		///     Clears this instance.
		/// </summary>
		public void Clear()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear input '{0}'", Name));
			WrappedElement.Clear();
		}

		/// <summary>
		///     Clears the using backspace.
		/// </summary>
		public void ClearUsingBackspace()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear input '{0}' with Backspace", Name));
			Enumerable.Range(0, Text.Length).ToList().ForEach(arg => WrappedElement.SendKeys(Keys.Backspace));
		}

		/// <summary>
		///     Sends the keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public virtual void SendKeys(string keys)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Type '{0}' to input '{1}'", keys, Name));
			WrappedElement.SendKeys(keys);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
		}

		/// <summary>
		///     Clears the and send keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public virtual void ClearAndSendKeys(string keys)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text and type '{0}' to input '{1}'", keys, Name));
			WrappedElement.Clear();

			if (Text.Length > 0)
			{
				WrappedElement.Clear();
			}

			WrappedElement.SendKeys(keys);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
		}

		/// <summary>
		///     The clear using backspace and send keys.
		/// </summary>
		/// <param name="keys">
		///     The keys.
		/// </param>
		public virtual void ClearUsingBackspaceAndSendKeys(string keys)
		{
			ClearUsingKeyAndSendKeys(keys, Keys.Backspace);
		}

		/// <summary>
		///     The clear using delete and send keys.
		/// </summary>
		/// <param name="keys">
		///     The keys.
		/// </param>
		public virtual void ClearUsingDeleteAndSendKeys(string keys)
		{
			ClearUsingKeyAndSendKeys(keys, Keys.Delete);
		}

		/// <summary>
		///     Clears the and send one key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void ClearAndSendOneKey(string key)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text and type '{0}' to input '{1}'", key, Name));

			if (Text.Length > 0)
			{
				WrappedElement.Clear();
			}

			WrappedElement.SendKeys(key);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
		}

		/// <summary>
		///     Clears the and send keys without focus lost.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public void ClearAndSendKeysWithoutFocusLost(string keys)
		{
			WrappedElement.SendKeys(Keys.Control + "a");
			WrappedElement.SendKeys(Keys.Backspace);
			WrappedElement.SendKeys(keys);
		}
	}
}