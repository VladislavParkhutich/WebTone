using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Core.SeleniumUtils.Core
{
	/// <summary>
	///     Waiter utility to perform wait by different conditions.
	/// </summary>
	public class Waiter
	{
		#region Public Properties

		/// <summary>
		///     Gets a value indicating whether is satisfied.
		/// </summary>
		/// <value>The is satisfied.</value>
		public bool IsSatisfied
		{
			get { return isSatisfied; }
		}

		#endregion

		#region Methods

		/// <summary>
		///     Mins the specified left.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result.</returns>
		private static T Min<T>(T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) < 0 ? left : right;
		}

		#endregion

		#region Fields

		private readonly TimeSpan checkInterval;

		private readonly Stopwatch stopwatch;

		private readonly TimeSpan timeout;

		private bool isSatisfied = true;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="Waiter" /> class.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		private Waiter(TimeSpan timeout) : this(timeout, TimeSpan.FromSeconds(1))
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Waiter" /> class.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="checkInterval">The check interval.</param>
		private Waiter(TimeSpan timeout, TimeSpan checkInterval)
		{
			this.timeout = timeout;
			this.checkInterval = checkInterval;
			stopwatch = Stopwatch.StartNew();
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Wait for specific condtition with timeout and pollingInterval = 1 second.
		/// </summary>
		/// <param name="condition">The condtition to wait.</param>
		/// <param name="timeout">The Timeout.</param>
		/// <returns>True if condition is reached.</returns>
		public static bool SpinWait(Func<bool> condition, TimeSpan timeout)
		{
			return SpinWait(condition, timeout, TimeSpan.FromSeconds(1));
		}

		/// <summary>
		///     Wait for specific condtition with timeout and pollingInterval.
		/// </summary>
		/// <param name="condition">The condtition to wait.</param>
		/// <param name="timeout">The Timeout.</param>
		/// <param name="pollingInterval">The PollingInterval.</param>
		/// <returns>True if condition is reached.</returns>
		public static bool SpinWait(Func<bool> condition, TimeSpan timeout, TimeSpan pollingInterval)
		{
			return WithTimeout(timeout, pollingInterval).WaitFor(condition).IsSatisfied;
		}

		/// <summary>
		///     Wait for specific condtition with timeout and pollingInterval and throw Exception is the condition is not
		///     satisfied.
		/// </summary>
		/// <param name="condition">The condtition to wait.</param>
		/// <param name="timeout">The Timeout.</param>
		/// <param name="pollingInterval">The PollingInterval.</param>
		/// <param name="exceptionMessage">Failing mesage.</param>
		public static void SpinWaitEnsureSatisfied(Func<bool> condition, TimeSpan timeout, TimeSpan pollingInterval,
			string exceptionMessage)
		{
			WithTimeout(timeout, pollingInterval).WaitFor(condition).EnsureSatisfied(exceptionMessage);
		}

		/// <summary>
		///     Wait for Element to be enabled.
		/// </summary>
		/// <param name="webDriver">webDriver instance.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="locator">The locator of the IWebElement.</param>
		/// <returns>Web Element.</returns>
		public static IWebElement WaitEnabled(IWebDriver webDriver, TimeSpan timeout, By locator)
		{
			var wait = new WebDriverWait(webDriver, timeout);
			return wait.Until(ExpectedConditions.ElementIsEnabled(locator));
		}

		/// <summary>
		///     Wait for Element to be enabled.
		/// </summary>
		/// <param name="webDriver">webDriver instance.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="locator">locator of the IWebElement.</param>
		/// <returns>List of Web Elements.</returns>
		public static IList<IWebElement> WaitEnabledAll(IWebDriver webDriver, TimeSpan timeout, By locator)
		{
			var wait = new WebDriverWait(webDriver, timeout);
			return wait.Until(ExpectedConditions.ElementsAreEnabled(locator));
		}

		/// <summary>
		///     Waits for the element to disappear.
		/// </summary>
		/// <param name="webElement">The web element.</param>
		public static void WaitForTheElementToDisappear(IWebElement webElement)
		{
			var condition = new Func<bool>(() =>
			{
				try
				{
					var element = webElement.FindElement(By.XPath("self::*"));
					return !element.Displayed;
				}
				catch (WebDriverException)
				{
					return true;
				}
			});

			SpinWaitEnsureSatisfied(condition, TimeSpan.FromSeconds(Configurations.Timeout), TimeSpan.FromMilliseconds(250),
				"Element is still displayed");
		}

		/// <summary>
		///     Wait for Element to be visible.
		/// </summary>
		/// <param name="webDriver">webDriver instance.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="locator">The locator of the IWebElement.</param>
		/// <returns>Web Element.</returns>
		public static IWebElement WaitVisible(IWebDriver webDriver, TimeSpan timeout, By locator)
		{
			var wait = new WebDriverWait(webDriver, timeout);
			return wait.Until(ExpectedConditions.ElementIsVisible(locator));
		}

		/// <summary>
		///     Wait for Element List to be visible.
		/// </summary>
		/// <param name="webDriver">webDriver instance.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="locator">locator of the IWebElement.</param>
		/// <returns>List of Web Elements.</returns>
		public static IList<IWebElement> WaitVisibleAll(IWebDriver webDriver, TimeSpan timeout, By locator)
		{
			var wait = new WebDriverWait(webDriver, timeout);
			return wait.Until(ExpectedConditions.ElementsAreVisible(locator));
		}

		/// <summary>
		///     Create <see cref="Waiter" /> with certain timeout and pollingInterval.
		/// </summary>
		/// <param name="timeout">The Timeout.</param>
		/// <param name="pollingInterval">The PollingInterval.</param>
		/// <returns>The Waiter.</returns>
		public static Waiter WithTimeout(TimeSpan timeout, TimeSpan pollingInterval)
		{
			return new Waiter(timeout, pollingInterval);
		}

		/// <summary>
		///     Create <see cref="Waiter" /> with the certain timeout.
		/// </summary>
		/// <param name="timeout">The Timeout.</param>
		/// <returns>The Waiter.</returns>
		public static Waiter WithTimeout(TimeSpan timeout)
		{
			return new Waiter(timeout);
		}

		/// <summary>
		///     Throw <see cref="TimeoutException" /> if <see cref="isSatisfied" /> == false.
		/// </summary>
		public void EnsureSatisfied()
		{
			if (!isSatisfied)
			{
				throw new TimeoutException();
			}
		}

		/// <summary>
		///     Throw <see cref="TimeoutException" /> if <see cref="isSatisfied" /> == false and specify message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void EnsureSatisfied(string message)
		{
			if (!isSatisfied)
			{
				throw new TimeoutException(message);
			}
		}

		/// <summary>
		///     Base Wait which is used by other methods.
		/// </summary>
		/// <param name="condition">The Condition.</param>
		/// <returns>The Waiter.</returns>
		public Waiter WaitFor(Func<bool> condition)
		{
			if (!isSatisfied)
			{
				return this;
			}

			while (!condition())
			{
				var sleepAmount = Min(timeout - stopwatch.Elapsed, checkInterval);

				if (sleepAmount < TimeSpan.Zero)
				{
					isSatisfied = false;
					break;
				}

				Thread.Sleep(sleepAmount);
			}

			return this;
		}

		/// <summary>
		///     Tries the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>The result.</returns>
		public static bool Try(Action action)
		{
			Exception exception;

			return Try(action, out exception);
		}

		/// <summary>
		///     Tries the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>The result.</returns>
		public static bool Try(Action action, out Exception exception)
		{
			try
			{
				action();
				exception = null;

				return true;
			}
			catch (Exception e)
			{
				exception = e;

				return false;
			}
		}

		/// <summary>
		///     Makes the try.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>The result.</returns>
		public static Func<bool> MakeTry(Action action)
		{
			return () => Try(action);
		}

		#endregion
	}
}