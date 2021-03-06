using System.Collections.Generic;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     Form class.
	/// </summary>
	public class Form : TypifiedElement
	{
		private const string TextInputType = "text";

		private const string PasswordInputType = "password";

		private const string CheckboxType = "checkbox";

		private const string RadioType = "radio";

		/// <summary>
		///     Initializes a new instance of the <see cref="Form" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Form(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Fills the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Fill(IDictionary<string, object> data)
		{
			foreach (var key in data.Keys)
			{
				var elementToFill = FindElementByKey(key);
				if (elementToFill != null)
				{
					FillElement(elementToFill, data[key]);
				}
			}
		}

		/// <summary>
		///     Submits this instance.
		/// </summary>
		public void Submit()
		{
			WrappedElement.Submit();
		}

		/// <summary>
		///     Finds the element by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value.</returns>
		private IWebElement FindElementByKey(string key)
		{
			IList<IWebElement> elements = WrappedElement.FindElements(By.Name(key));
			if (elements.Count <= 0)
			{
				return null;
			}

			return elements[0];
		}

		/// <summary>
		///     Fills the element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="value">The value.</param>
		protected void FillElement(IWebElement element, object value)
		{
			if (value == null)
			{
				return;
			}

			if (IsInput(element))
			{
				var inputType = element.GetAttribute("type");
				if (inputType == null || inputType == TextInputType || inputType == PasswordInputType)
				{
					element.SendKeys(value.ToString());
				}
				else if (inputType == CheckboxType)
				{
					var checkBox = new CheckBox(element);
					checkBox.Set(bool.Parse(value.ToString()));
				}
				else if (inputType == RadioType)
				{
					var radio = new Radio(element);
					radio.SelectByValue(value.ToString());
				}
			}
			else if (IsSelect(element))
			{
				var select = new Select(element);
				select.SelectByValue(value.ToString());
			}
			else if (IsTextArea(element))
			{
				element.SendKeys(value.ToString());
			}
		}

		/// <summary>
		///     Determines whether the specified element is input.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The value.</returns>
		private bool IsInput(IWebElement element)
		{
			return "input" == element.TagName;
		}

		/// <summary>
		///     Determines whether the specified element is select.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The value.</returns>
		private bool IsSelect(IWebElement element)
		{
			return "select" == element.TagName;
		}

		/// <summary>
		///     Determines whether [is text area] [the specified element].
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The value.</returns>
		private bool IsTextArea(IWebElement element)
		{
			return "textarea" == element.TagName;
		}
	}
}