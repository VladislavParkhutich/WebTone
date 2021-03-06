using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.SeleniumUtils.Core.ErrorHandling;
using Microsoft.CSharp.RuntimeBinder;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories
{
	public delegate IWebElement FindElement();

	public delegate ReadOnlyCollection<IWebElement> FindElements();

	public delegate bool IsElementUsable(IWebElement element);

	/// <summary>
	///     An element locator that will wait for the specified number of seconds for an element to appear,.
	///     rather than failing instantly if it's not present. This works by polling the UI on a regular.
	///     basis. The element returned will be present on the DOM, but may not actually be visible: override.
	///     <see cref="IsElementUsable" /> if this is important to you.
	/// </summary>
	public class AjaxElementLocator : DefaultElementLocator
	{
		private IClock clock;

		private TimeSpan sleepInterval;

		/// <summary>
		///     Initializes a new instance of the <see cref="AjaxElementLocator" /> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		/// <param name="locator">The locator.</param>
		public AjaxElementLocator(ISearchContext searchContext, By locator)
			: base(searchContext, locator)
		{
			RestoreDefaults();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="AjaxElementLocator" /> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		/// <param name="attributesHandler">The attributes handler.</param>
		public AjaxElementLocator(ISearchContext searchContext, AttributesHandler attributesHandler)
			: base(searchContext, attributesHandler)
		{
			RestoreDefaults();
		}

		/// <summary>
		///     Gets or sets the total wait timeout in seconds.
		/// </summary>
		/// <value>
		///     The total wait timeout in seconds.
		/// </value>
		public int TimeoutInSeconds { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether error handling enabled.
		/// </summary>
		/// <value>
		///     <c>true</c> if error handling enabled; otherwise, <c>false</c>.
		/// </value>
		public bool ErrorHandlingEnabled { get; set; }

		/// <summary>
		///     Gets the sleep interval.
		/// </summary>
		/// <value>
		///     The sleep interval.
		/// </value>
		protected virtual TimeSpan SleepInterval
		{
			get { return sleepInterval; }
		}

		/// <summary>
		///     Finds the element.
		///     Will poll the interface on a regular basis until the element is present.
		///     If element not present after polling then perform error handling and try load element again.
		/// </summary>
		/// <returns>
		///     <see cref="IWebElement" /> instance.
		/// </returns>
		public override IWebElement FindElement()
		{
			var loadingElement = new SlowLoadingElement(clock, TimeoutInSeconds, SleepInterval, base.FindElement, IsElementUsable);
			try
			{
				return loadingElement.Load().Element;
			}
			catch (Exception exception)
			{
				if (exception is WebDriverException || exception is RuntimeBinderException)
				{
					var reloadElement = false;
					var lastException = exception;
					if (!ErrorHandlingEnabled)
					{
						throw lastException;
					}
					foreach (var errorHandler in UIErrorHandlersContainer.ErrorHandlers)
					{
						if (errorHandler.ErrorExists)
						{
							reloadElement = true;
						}
						errorHandler.HandleError();
					}
					if (reloadElement)
					{
						return loadingElement.Load().Element;
					}
					throw lastException;
				}
				throw;
			}
		}

		/// <summary>
		///     Finds the elements.
		///     Will poll the interface on a regular basis until the element is present.
		///     If element not present after polling then perform error handling and try load element again.
		/// </summary>
		/// <returns>
		///     Collection of <see cref="IWebElement" />s.
		/// </returns>
		public override ReadOnlyCollection<IWebElement> FindElements()
		{
			var loadingElement = new SlowLoadingElements(clock, TimeoutInSeconds, SleepInterval, base.FindElements);
			try
			{
				return loadingElement.Load().Elements;
			}
			catch (Exception exception)
			{
				if (exception is WebDriverException || exception is RuntimeBinderException)
				{
					var reloadElement = false;
					if (!ErrorHandlingEnabled)
					{
						return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
					}
					foreach (var errorHandler in UIErrorHandlersContainer.ErrorHandlers)
					{
						if (errorHandler.ErrorExists)
						{
							reloadElement = true;
						}
						errorHandler.HandleError();
					}
					if (!reloadElement)
					{
						return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
					}
					try
					{
						return loadingElement.Load().Elements;
					}
					catch (Exception exception1)
					{
						if (exception1 is WebDriverException || exception1 is RuntimeBinderException)
						{
							return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
						}
						throw;
					}
				}
				throw;
			}
		}

		/// <summary>
		///     Restores the defaults values.
		/// </summary>
		public void RestoreDefaults()
		{
			clock = new SystemClock();
			TimeoutInSeconds = Configurations.Timeout;
			sleepInterval = TimeSpan.FromMilliseconds(250);
			ErrorHandlingEnabled = true;
		}

		/// <summary>
		///     Determines whether element is usable.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		protected virtual bool IsElementUsable(IWebElement element)
		{
			return true;
		}

		private class SlowLoadingElement : SlowLoadableComponent<SlowLoadingElement>
		{
			private readonly FindElement elementFindingFunc;

			private readonly IsElementUsable elementIsUsableFunc;

			public SlowLoadingElement(IClock clock, int timeOutInSeconds, TimeSpan sleepInterval, FindElement elementFindingFunc,
				IsElementUsable elementIsUsableFunc)
				: base(TimeSpan.FromSeconds(timeOutInSeconds), clock)
			{
				this.elementFindingFunc = elementFindingFunc;
				this.elementIsUsableFunc = elementIsUsableFunc;
				SleepInterval = sleepInterval;
			}

			public IWebElement Element { get; private set; }

			protected override void ExecuteLoad()
			{
				// Does nothing
			}

			protected override bool EvaluateLoadedStatus()
			{
				try
				{
					Element = elementFindingFunc();
					if (!elementIsUsableFunc(Element))
					{
						throw new NoSuchElementException("elements is not usable");
					}
					return true;
				}
				catch (StaleElementReferenceException)
				{
					return false;
				}
				catch (NoSuchElementException e)
				{
					// Should use JUnit's AssertionError, but it may not be present
					throw new NoSuchElementException("Unable to locate the element", e);
				}
			}
		}

		private class SlowLoadingElements : SlowLoadableComponent<SlowLoadingElements>
		{
			private readonly FindElements elementFindingFunc;

			public SlowLoadingElements(IClock clock, int timeOutInSeconds, TimeSpan sleepInterval,
				FindElements elementFindingFunc)
				: base(TimeSpan.FromSeconds(timeOutInSeconds), clock)
			{
				this.elementFindingFunc = elementFindingFunc;
				SleepInterval = sleepInterval;
			}

			public ReadOnlyCollection<IWebElement> Elements { get; private set; }

			protected override void ExecuteLoad()
			{
				// Does nothing
			}

			protected override bool EvaluateLoadedStatus()
			{
				try
				{
					Elements = elementFindingFunc();
					if (Elements.Count == 0)
					{
						throw new NoSuchElementException("elements is not usable");
					}
					return true;
				}
				catch (NoSuchElementException e)
				{
					// Should use JUnit's AssertionError, but it may not be present
					throw new NoSuchElementException("Unable to locate the elements", e);
				}
			}
		}
	}
}