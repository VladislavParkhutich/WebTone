using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     Radio class.
	/// </summary>
	public class Radio : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Radio" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Radio(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Verifies the index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="buttonCount">The button count.</param>
		private void VerifyIndex(int index, int buttonCount)
		{
			if (index < 0 || index >= buttonCount)
			{
				throw new NoSuchElementException(string.Format(CultureInfo.InvariantCulture,
					"Cannot locate radio button with index: {0}", index));
			}
		}

		/// <summary>
		///     Gets the buttons.
		/// </summary>
		/// <returns>The collection.</returns>
		public IList<IWebElement> GetButtons()
		{
			var radioName = WrappedElement.GetAttribute("name");

			string xpath;
			if (radioName == null)
			{
				xpath = "self::* | following::input[@type = 'radio'] | preceding::input[@type = 'radio']";
			}
			else
			{
				xpath = string.Format(CultureInfo.InvariantCulture,
					"self::* | following::input[@type = 'radio' and @name = '{0}'] | " +
					"preceding::input[@type = 'radio' and @name = '{0}']", radioName);
			}

			return WrappedElement.FindElements(By.XPath(xpath));
		}

		/// <summary>
		///     Gets the selected button.
		/// </summary>
		/// <returns>The result.</returns>
		public IWebElement GetSelectedButton()
		{
			foreach (var button in GetButtons())
			{
				if (button.Selected)
				{
					return button;
				}
			}

			throw new NoSuchElementException("No selected button");
		}

		/// <summary>
		///     Determines whether [has selected button].
		/// </summary>
		/// <returns>The result.</returns>
		public bool HasSelectedButton()
		{
			foreach (var button in GetButtons())
			{
				if (button.Selected)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		///     Selects the by value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void SelectByValue(string value)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture,
				"Select the radio button from the group '{0}' by value '{1}'", Name, value));
			foreach (var button in GetButtons())
			{
				var buttonValue = button.GetAttribute("value");
				if (value.Equals(buttonValue, StringComparison.OrdinalIgnoreCase))
				{
					SelectButton(button);
					return;
				}
			}

			throw new NoSuchElementException(string.Format(CultureInfo.InvariantCulture,
				"Cannot locate radio button with value: {0}", value));
		}

		/// <summary>
		///     Selects the index of the by.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SelectByIndex(int index)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture,
				"Select the radio button from the group '{0}' by index '{1}'", Name, index));
			var buttons = GetButtons();

			VerifyIndex(index, buttons.Count);

			SelectButton(buttons[index]);
		}

		/// <summary>
		///     Selects the button.
		/// </summary>
		/// <param name="button">The button.</param>
		private void SelectButton(IWebElement button)
		{
			if (!button.Selected)
			{
				button.Click();
			}
		}

		/// <summary>
		///     Returns if radio-button is selected.
		///     Created new method as the default not always is accurate. .
		/// </summary>
		/// <param name="buttonIndex">Index of the button.</param>
		/// <returns>The <see cref="bool" />.</returns>
		public bool IsSelectedByIndex(int buttonIndex)
		{
			var buttons = GetButtons();
			VerifyIndex(buttonIndex, buttons.Count);

			return
				buttons.Select(button => button.GetAttribute("outerHTML"))
					.Select(attributes => attributes.Contains("checked"))
					.FirstOrDefault();
		}

		/// <summary>
		///     Checks if radio-button with passed index is selected.
		///     Created new method as the default not always is accurate. .
		/// </summary>
		/// <param name="buttonIndex">Index of the button.</param>
		/// <returns>The <see cref="bool" />.</returns>
		public bool IsButtonByIndexSelected(int buttonIndex)
		{
			var buttons = GetButtons();
			VerifyIndex(buttonIndex, buttons.Count);
			return buttons[buttonIndex].GetAttribute("outerHTML").Contains("checked");
		}
	}
}