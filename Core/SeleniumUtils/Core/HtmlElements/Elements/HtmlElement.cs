using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using Core.GeneralUtils.Container;
using Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators.ProxyHandlers;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories;
using Core.SeleniumUtils.Core.Objects;
using ImpromptuInterface;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     Base class for block of elements.
	/// </summary>
	public class HtmlElement : UIInfrastructureObject, IWebElement, IWrapsElement, INamed
	{
		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		///     Clicks this element.
		/// </summary>
		/// <remarks>
		///     <para>
		///         Click this element. If the click causes a new page to load, the
		///         <see cref="M:OpenQA.Selenium.IWebElement.Click" />
		///         method will attempt to block until the page has loaded. After calling the.
		///         <see cref="M:OpenQA.Selenium.IWebElement.Click" /> method, you should discard all references to this.
		///         element unless you know that the element and the page will still be present.
		///         Otherwise, any further operations performed on this element will have an undefined.
		///         behavior.
		///     </para>
		///     <para>
		///         If this element is not clickable, then this operation is ignored. This allows you to.
		///         simulate a users to accidentally missing the target when clicking.
		///     </para>
		/// </remarks>
		public void Click()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click element '{0}'", Name));
			WrappedElement.Click();
		}

		/// <summary>
		///     Submits this element to the web server.
		/// </summary>
		/// <remarks>
		///     If this current element is a form, or an element within a form,.
		///     then this will be submitted to the web server. If this causes the current.
		///     page to change, then this method will block until the new page is loaded.
		/// </remarks>
		public void Submit()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Submit element '{0}'", Name));
			WrappedElement.Submit();
		}

		/// <summary>
		///     Simulates typing text into the element.
		/// </summary>
		/// <param name="text">The text to type into the element.</param>
		/// <remarks>
		///     The text to be typed may include special characters like arrow keys,.
		///     backspaces, function keys, and so on. Valid special keys are defined in.
		///     <see cref="T:OpenQA.Selenium.Keys" />.
		/// </remarks>
		/// <seealso cref="T:OpenQA.Selenium.Keys" />
		public void SendKeys(string text)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Type '{0}' to element '{1}'", text, Name));
			WrappedElement.SendKeys(text);
		}

		/// <summary>
		///     Clears the content of this element.
		/// </summary>
		/// <remarks>
		///     If this element is a text entry element, the <see cref="M:OpenQA.Selenium.IWebElement.Clear" />
		///     method will clear the value. It has no effect on other elements. Text entry elements.
		///     are defined as elements with INPUT or TEXTAREA tags.
		/// </remarks>
		public void Clear()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear element '{0}'", Name));
			WrappedElement.Clear();
		}

		/// <summary>
		///     Gets the tag name of this element.
		/// </summary>
		/// <remarks>
		///     The <see cref="P:OpenQA.Selenium.IWebElement.TagName" /> property returns the tag name of the.
		///     element, not the value of the name attribute. For example, it will return.
		///     "input" for an element specified by the HTML markup &lt;input name="foo" /&gt;.
		/// </remarks>
		/// <value>The value.</value>
		public string TagName
		{
			get { return WrappedElement.TagName; }
		}

		/// <summary>
		///     Gets the value of the specified attribute for this element.
		/// </summary>
		/// <param name="attributeName">The name of the attribute.</param>
		/// <remarks>
		///     The <see cref="M:OpenQA.Selenium.IWebElement.GetAttribute(System.String)" />
		///     method will return the current value
		///     of the attribute, even if the value has been modified after the page has been
		///     loaded. Note that the value of the following attributes will be returned even
		///     if
		///     there is no explicit attribute on the element:
		///     <list type="table">
		///         <listheader>
		///             <term>Attribute name</term>
		///             <term>
		///                 Value returned
		///                 if not explicitly specified
		///             </term>
		///             <term>Valid element types</term>
		///         </listheader>
		///         <item>
		///             <description>checked</description><description>checked</description>
		///             <description>
		///                 Check
		///                 Box
		///             </description>
		///         </item>
		///         <item>
		///             <description>selected</description>
		///             <description>
		///                 selected
		///             </description>
		///             <description>Options in Select elements</description>
		///         </item>
		///         <item>
		///             <description>disabled</description><description>disabled</description>
		///             <description>
		///                 Input
		///                 and other UI elements
		///             </description>
		///         </item>
		///     </list>
		/// </remarks>
		/// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">
		///     Thrown when
		///     the target element is no longer valid in the document DOM.
		/// </exception>
		/// <returns>
		///     The attribute's current value. Returns a <see langword="null" /> if the
		///     value is not set.
		/// </returns>
		public string GetAttribute(string attributeName)
		{
			return WrappedElement.GetAttribute(attributeName);
		}

		/// <summary>
		///     Gets a value indicating whether or not this element is selected.
		/// </summary>
		/// <remarks>
		///     This operation only applies to input elements such as checkboxes,.
		///     options in a select element and radio buttons.
		/// </remarks>
		/// <value>The value.</value>
		public bool Selected
		{
			get { return WrappedElement.Selected; }
		}

		/// <summary>
		///     Gets a value indicating whether or not this element is enabled.
		/// </summary>
		/// <remarks>
		///     The <see cref="P:OpenQA.Selenium.IWebElement.Enabled" /> property will generally.
		///     return <see langword="true" /> for everything except explicitly disabled input elements.
		/// </remarks>
		/// <value>The value.</value>
		public bool Enabled
		{
			get { return WrappedElement.Enabled; }
		}

		/// <summary>
		///     Gets the innerText of this element, without any leading or trailing whitespace,.
		///     and with other whitespace collapsed.
		/// </summary>
		/// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">
		///     Thrown when
		///     the target element is no longer valid in the document DOM.
		/// </exception>
		/// <value>The value.</value>
		public string Text
		{
			get { return WrappedElement.Text; }
		}

		/// <summary>
		///     Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context.
		///     using the given mechanism.
		/// </summary>
		/// <param name="by">The locating mechanism to use.</param>
		/// <returns>
		///     A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of all
		///     <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
		///     matching the current criteria, or an empty list if nothing matches.
		/// </returns>
		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			return WrappedElement.FindElements(by);
		}

		/// <summary>
		///     Finds the first <see cref="T:OpenQA.Selenium.IWebElement" /> using the given method.
		/// </summary>
		/// <param name="by">The locating mechanism to use.</param>
		/// <returns>
		///     The first matching <see cref="T:OpenQA.Selenium.IWebElement" /> on the current context.
		/// </returns>
		public IWebElement FindElement(By by)
		{
			return WrappedElement.FindElement(by);
		}

		/// <summary>
		///     Gets a value indicating whether or not this element is displayed.
		/// </summary>
		/// <remarks>
		///     The <see cref="P:OpenQA.Selenium.IWebElement.Displayed" /> property avoids the problem.
		///     of having to parse an element's "style" attribute to determine.
		///     visibility of an element.
		/// </remarks>
		/// <value>The value.</value>
		public bool Displayed
		{
			get
			{
				try
				{
					return WrappedElement.Displayed;
				}
				catch (WebDriverTimeoutException)
				{
					return false;
				}
			}
		}

		/// <summary>
		///     Gets a <see cref="T:System.Drawing.Point" /> object containing the coordinates of the upper-left corner.
		///     of this element relative to the upper-left corner of the page.
		/// </summary>
		/// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">
		///     Thrown when
		///     the target element is no longer valid in the document DOM.
		/// </exception>
		/// <value>The value.</value>
		public Point Location
		{
			get { return WrappedElement.Location; }
		}

		/// <summary>
		///     Gets a <see cref="P:OpenQA.Selenium.IWebElement.Size" /> object containing the height and width of this element.
		/// </summary>
		/// <exception cref="T:OpenQA.Selenium.StaleElementReferenceException">
		///     Thrown when
		///     the target element is no longer valid in the document DOM.
		/// </exception>
		/// <value>The value.</value>
		public Size Size
		{
			get { return WrappedElement.Size; }
		}

		/// <summary>
		///     Gets the value of a CSS property of this element.
		/// </summary>
		/// <param name="propertyName">The name of the CSS property to get the value of.</param>
		/// <returns>
		///     The value of the specified CSS property.
		/// </returns>
		/// <remarks>
		///     The value returned by the <see cref="M:OpenQA.Selenium.IWebElement.GetCssValue(System.String)" />
		///     method is likely to be unpredictable in a cross-browser environment.
		///     Color values should be returned as hex strings. For example, a.
		///     "background-color" property set as "green" in the HTML source, will.
		///     return "#008000" for its value.
		/// </remarks>
		public string GetCssValue(string propertyName)
		{
			return WrappedElement.GetCssValue(propertyName);
		}

		/// <summary>
		///     Gets or sets the <see cref="T:OpenQA.Selenium.IWebElement" /> wrapped by this object.
		/// </summary>
		/// <value>The value.</value>
		public IWebElement WrappedElement { get; set; }

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		///     A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		///     Displayeds the with timeout.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns>The value.</returns>
		public bool DisplayedWithTimeout(TimeSpan time)
		{
			var webElementNamedProxyHandler = WrappedElement.UndoActLike() as INamedElementLocatorHandler;
			var ajaxElementLocator = (AjaxElementLocator) webElementNamedProxyHandler.Locator;
			ajaxElementLocator.TimeoutInSeconds = time.Seconds;
			ajaxElementLocator.ErrorHandlingEnabled = false;
			var isDisplayed = Displayed;
			ajaxElementLocator.RestoreDefaults();
			return isDisplayed;
		}
	}
}