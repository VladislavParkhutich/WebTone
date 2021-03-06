using System;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories
{
	/// <summary>
	///     AttributesHandler class.
	/// </summary>
	public abstract class AttributesHandler
	{
		/// <summary>
		///     Builds the by.
		/// </summary>
		/// <returns>The value.</returns>
		public abstract By BuildBy();

		/// <summary>
		///     Shoulds the cache.
		/// </summary>
		/// <returns>The value.</returns>
		public abstract bool ShouldCache();

		/// <summary>
		///     Builds the by from finds by values.
		/// </summary>
		/// <param name="findByValues">The find by values.</param>
		/// <returns>The value.</returns>
		protected virtual By BuildByFromFindsByValues(FindsByAttribute[] findByValues)
		{
			AssertValidFindBys(findByValues);

			var arrayFoundBy = new By[findByValues.Length];
			for (var i = 0; i < findByValues.Length; i++)
			{
				arrayFoundBy[i] = BuildByFromFindsBy(findByValues[i]);
			}

			return new ByChained(arrayFoundBy);
		}

		/// <summary>
		///     Builds the by from finds by.
		/// </summary>
		/// <param name="findsBy">The finds by.</param>
		/// <returns>The value.</returns>
		protected virtual By BuildByFromFindsBy(FindsByAttribute findsBy)
		{
			AssertValidFindBy(findsBy);
			return BuildBy(findsBy);
		}

		/// <summary>
		///     Builds the by.
		/// </summary>
		/// <param name="findsBy">The finds by.</param>
		/// <returns>The value.</returns>
		protected virtual By BuildBy(FindsByAttribute findsBy)
		{
			var how = findsBy.How;
			var usingStr = findsBy.Using;
			var customType = findsBy.CustomFinderType;

			switch (how)
			{
				case How.Id:
					return By.Id(usingStr);
				case How.Name:
					return By.Name(usingStr);
				case How.TagName:
					return By.TagName(usingStr);
				case How.ClassName:
					return By.ClassName(usingStr);
				case How.CssSelector:
					return By.CssSelector(usingStr);
				case How.LinkText:
					return By.LinkText(usingStr);
				case How.PartialLinkText:
					return By.PartialLinkText(usingStr);
				case How.XPath:
					return By.XPath(usingStr);
				case How.Custom:
					var ctor = customType.GetConstructor(new[] {typeof(string)});
					var finder = ctor.Invoke(new object[] {usingStr}) as By;
					return finder;
				default:
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
						"Did not know how to construct How from how {0}, using {1}", how, usingStr));
			}
		}

		/// <summary>
		///     Asserts the valid find bys.
		/// </summary>
		/// <param name="findBys">The find bys.</param>
		private void AssertValidFindBys(FindsByAttribute[] findBys)
		{
			foreach (var findBy in findBys)
			{
				AssertValidFindBy(findBy);
			}
		}

		/// <summary>
		///     Asserts the valid find by.
		/// </summary>
		/// <param name="findBy">The find by.</param>
		private void AssertValidFindBy(FindsByAttribute findBy)
		{
			if (findBy.How == How.Custom)
			{
				if (findBy.CustomFinderType == null)
				{
					throw new ArgumentException("If you set the 'How' property to 'Custom' value, you must also set 'CustomFinderType'");
				}

				if (!findBy.CustomFinderType.IsSubclassOf(typeof(By)))
				{
					throw new ArgumentException("Custom finder type must be a descendent of the 'By' class");
				}

				var ctor = findBy.CustomFinderType.GetConstructor(new[] {typeof(string)});
				if (ctor == null)
				{
					throw new ArgumentException("Custom finder type must expose a public constructor with a string argument");
				}
			}
		}
	}
}