using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core
{
	/// <summary>
	///     Supplies a set of common conditions that can be waited for using <see cref="WebDriverWait" />.
	/// </summary>
	/// <example>
	///     <code>
	/// IWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3)).
	/// IWebElement element = wait.Until(ExpectedConditions.ElementExists(By.Id("foo")));.
	/// </code>
	/// </example>
	public sealed class ExpectedConditions
	{
		#region Constructors and Destructors

		/// <summary>
		///     Prevents a default instance of the <see cref="ExpectedConditions" /> class from being created.
		/// </summary>
		private ExpectedConditions()
		{
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     An expectation for checking that an element is present on the DOM of a.
		///     page. This does not necessarily mean that the element is visible.
		/// </summary>
		/// <param name="locator">The locator used to find the element.</param>
		/// <returns>The <see cref="IWebElement" /> once it is located.</returns>
		public static Func<IWebDriver, IWebElement> ElementExists(By locator)
		{
			return driver => { return driver.FindElement(locator); };
		}

		/// <summary>
		///     An expectation for checking that an element is present on the DOM of a page.
		///     and enabled. .
		/// </summary>
		/// <param name="locator">The locator used to find the element.</param>
		/// <returns>The <see cref="IWebElement" /> once it is located and enabled.</returns>
		public static Func<IWebDriver, IWebElement> ElementIsEnabled(By locator)
		{
			return driver =>
			{
				try
				{
					return ElementIfEnabled(driver.FindElement(locator));
				}
				catch (StaleElementReferenceException)
				{
					return null;
				}
				catch (NoSuchElementException)
				{
					return null;
				}
			};
		}

		/// <summary>
		///     An expectation for checking that an element is present on the DOM of a page.
		///     and visible. Visibility means that the element is not only displayed but.
		///     also has a height and width that is greater than 0.
		/// </summary>
		/// <param name="locator">The locator used to find the element.</param>
		/// <returns>The <see cref="IWebElement" /> once it is located and visible.</returns>
		public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
		{
			return driver =>
			{
				try
				{
					return ElementIfVisible(driver.FindElement(locator));
				}
				catch (StaleElementReferenceException)
				{
					return null;
				}
				catch (NoSuchElementException)
				{
					return null;
				}
			};
		}

		/// <summary>
		///     Elementses the are enabled.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <returns>The function.</returns>
		public static Func<IWebDriver, IList<IWebElement>> ElementsAreEnabled(By locator)
		{
			return driver =>
			{
				try
				{
					return ElementsIfEnabled(driver.FindElements(locator));
				}
				catch (StaleElementReferenceException)
				{
					return null;
				}
			};
		}

		/// <summary>
		///     Elementses the are visible.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <returns>The function.</returns>
		public static Func<IWebDriver, IList<IWebElement>> ElementsAreVisible(By locator)
		{
			return driver =>
			{
				try
				{
					return ElementsIfVisible(driver.FindElements(locator));
				}
				catch (StaleElementReferenceException)
				{
					return null;
				}
			};
		}

		/// <summary>
		///     An expectation for checking that the title of a page contains a case-sensitive substring.
		/// </summary>
		/// <param name="title">The fragment of title expected.</param>
		/// <returns><see langword="true" /> when the title matches; otherwise, <see langword="false" />.</returns>
		public static Func<IWebDriver, bool> TitleContains(string title)
		{
			return driver => { return driver.Title.Contains(title); };
		}

		/// <summary>
		///     An expectation for checking the title of a page.
		/// </summary>
		/// <param name="title">The expected title, which must be an exact match.</param>
		/// <returns><see langword="true" /> when the title matches; otherwise, <see langword="false" />.</returns>
		public static Func<IWebDriver, bool> TitleIs(string title)
		{
			return driver => { return title == driver.Title; };
		}

		#endregion

		#region Methods

		/// <summary>
		///     Elements if enabled.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The value.</returns>
		private static IWebElement ElementIfEnabled(IWebElement element)
		{
			if (element.Enabled)
			{
				return element;
			}
			return null;
		}

		/// <summary>
		///     Elements if visible.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The value.</returns>
		private static IWebElement ElementIfVisible(IWebElement element)
		{
			if (element.Displayed)
			{
				return element;
			}
			return null;
		}

		/// <summary>
		///     Elementses if enabled.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <returns>The value.</returns>
		private static IList<IWebElement> ElementsIfEnabled(IList<IWebElement> elements)
		{
			foreach (var element in elements)
			{
				if (element.Enabled)
				{
					return elements;
				}
				return null;
			}

			return null;
		}

		/// <summary>
		///     Elementses if visible.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <returns>The value.</returns>
		private static IList<IWebElement> ElementsIfVisible(IList<IWebElement> elements)
		{
			foreach (var element in elements)
			{
				if (element.Displayed)
				{
					return elements;
				}
				return null;
			}

			return null;
		}

		#endregion
	}
}