using System;
using System.Globalization;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     The check box.
	/// </summary>
	public class CheckBox : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBox" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public CheckBox(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Gets the label.
		/// </summary>
		/// <value>The label.</value>
		public IWebElement Label
		{
			get
			{
				try
				{
					return WrappedElement.FindElement(By.XPath("following-sibling::label"));
				}
				catch
				{
					return null;
				}
			}
		}

		/// <summary>
		///     Gets the label text.
		/// </summary>
		/// <value>The label text.</value>
		public string LabelText
		{
			get
			{
				var label = Label;
				return label == null ? null : label.Text;
			}
		}

		/// <summary>
		///     Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return LabelText; }
		}

		/// <summary>
		///     Gets the id.
		/// </summary>
		/// <value>The id.</value>
		public Guid Id
		{
			get { return new Guid(Value); }
		}

		/// <summary>
		///     The select.
		/// </summary>
		public virtual void Select()
		{
			if (!Selected)
			{
				Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Check the checkbox '{0}'", Name));
				WrappedElement.Click();
			}
		}

		/// <summary>
		///     The deselect.
		/// </summary>
		public virtual void Deselect()
		{
			if (Selected)
			{
				Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Uncheck the checkbox '{0}'", Name));
				WrappedElement.Click();
			}
		}

		/// <summary>
		///     The set.
		/// </summary>
		/// <param name="value">
		///     The value.
		/// </param>
		public void Set(bool value)
		{
			if (value)
			{
				Select();
			}
			else
			{
				Deselect();
			}
		}
	}
}