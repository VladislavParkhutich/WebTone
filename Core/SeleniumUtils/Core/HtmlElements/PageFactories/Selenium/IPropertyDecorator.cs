using System.Reflection;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium
{
	/// <summary>
	///     IPropertyDecorator interface.
	/// </summary>
	public interface IPropertyDecorator
	{
		/// <summary>
		///     Decorates the specified property info.
		/// </summary>
		/// <param name="propertyInfo">The property info.</param>
		/// <returns>The result.</returns>
		object Decorate(PropertyInfo propertyInfo);
	}
}