using System;
using System.Linq;
using System.Reflection;

namespace Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium
{
	/// <summary>
	///     PageFactory class.
	/// </summary>
	public static class PageFactory
	{
		/// <summary>
		///     Inits the elements.
		/// </summary>
		/// <param name="decorator">The decorator.</param>
		/// <param name="page">The page.</param>
		public static void InitElements(IPropertyDecorator decorator, object page)
		{
			var proxyIn = page.GetType();
			while (proxyIn != typeof(object))
			{
				ProxyFields(decorator, page, proxyIn);
				proxyIn = proxyIn.BaseType;
			}
		}

		/// <summary>
		///     Proxies the fields.
		/// </summary>
		/// <param name="decorator">The decorator.</param>
		/// <param name="page">The page.</param>
		/// <param name="proxyIn">The proxy in.</param>
		private static void ProxyFields(IPropertyDecorator decorator, object page, Type proxyIn)
		{
			var fields =
				proxyIn.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
				                      BindingFlags.DeclaredOnly).Where(p => p.CanRead && p.CanWrite).ToArray();
			foreach (var property in fields)
			{
				var value = decorator.Decorate(property);
				if (value != null)
				{
					property.SetValue(page, value);
				}
			}
		}
	}
}