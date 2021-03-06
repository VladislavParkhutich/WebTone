using System.Reflection;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium
{
	/// <summary>
	///     IElementLocatorFactory interface.
	/// </summary>
	public interface IElementLocatorFactory
	{
		/// <summary>
		///     Creates the locator.
		/// </summary>
		/// <param name="propertyInfo">The property info.</param>
		/// <returns>The result.</returns>
		IElementLocator CreateLocator(PropertyInfo propertyInfo);
	}
}