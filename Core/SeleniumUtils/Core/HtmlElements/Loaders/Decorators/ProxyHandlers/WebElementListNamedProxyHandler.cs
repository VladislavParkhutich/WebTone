using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories.Selenium;
using ImpromptuInterface;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators.ProxyHandlers
{
	/// <summary>
	///     WebElementListNamedProxyHandler class.
	/// </summary>
	public class WebElementListNamedProxyHandler : DynamicObject, INamedElementLocatorHandler
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="WebElementListNamedProxyHandler" /> class.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="name">The name.</param>
		private WebElementListNamedProxyHandler(IElementLocator locator, string name)
		{
			Locator = locator;
			Name = name;
		}

		/// <summary>
		///     Gets the locator.
		/// </summary>
		/// <value>The locator.</value>
		public IElementLocator Locator { get; private set; }

		/// <summary>
		///     Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		///     News the instance.
		/// </summary>
		/// <param name="locator">The locator.</param>
		/// <param name="name">The name.</param>
		/// <returns>The collection.</returns>
		public static IList<IWebElement> NewInstance(IElementLocator locator, string name)
		{
			return new WebElementListNamedProxyHandler(locator, name).ActLike<IList<IWebElement>>();
		}

		/// <summary>
		///     Provides the implementation for operations that invoke a member. Classes
		///     derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override
		///     this method to specify dynamic behavior for operations such as calling a method.
		/// </summary>
		/// <param name="binder">
		///     Provides information about the dynamic operation. The binder.Name
		///     property provides the name of the member on which the dynamic operation is performed.
		///     For example, for the statement sampleObject.SampleMethod(100), where sampleObject
		///     is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" />
		///     class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies
		///     whether the member name is case-sensitive.
		/// </param>
		/// <param name="args">
		///     The arguments that are passed to the object member during
		///     the invoke operation. For example, for the statement sampleObject.SampleMethod(100),
		///     where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" />
		///     class, <paramref name="args[0]" /> is equal to 100.
		/// </param>
		/// <param name="result">The result of the member invocation.</param>
		/// <returns>
		///     true if the operation is successful; otherwise, false. If this method
		///     returns false, the run-time binder of the language determines the behavior. (In
		///     most cases, a language-specific run-time exception is thrown.).
		/// </returns>
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			IReadOnlyCollection<IWebElement> elements = Locator.FindElements();

			try
			{
				result = elements.GetType().GetMethod(binder.Name).Invoke(elements, args);
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}

		/// <summary>
		///     Provides the implementation for operations that get member values. Classes
		///     derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override
		///     this method to specify dynamic behavior for operations such as getting a value
		///     for a property.
		/// </summary>
		/// <param name="binder">
		///     Provides information about the object that called the dynamic
		///     operation. The binder.Name property provides the name of the member on which
		///     the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty)
		///     statement, where sampleObject is an instance of the class derived from the
		///     <see cref="T:System.Dynamic.DynamicObject" />
		///     class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies
		///     whether the member name is case-sensitive.
		/// </param>
		/// <param name="result">
		///     The result of the get operation. For example, if the method
		///     is called for a property, you can assign the property value to <paramref name="result" />.
		/// </param>
		/// <returns>
		///     true if the operation is successful; otherwise, false. If this method
		///     returns false, the run-time binder of the language determines the behavior. (In
		///     most cases, a run-time exception is thrown.).
		/// </returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			IReadOnlyCollection<IWebElement> elements = Locator.FindElements();

			try
			{
				result = elements.GetType().GetProperty(binder.Name).GetValue(elements);
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}

		/// <summary>
		///     Provides the implementation for operations that get a value by index.
		///     Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can
		///     override this method to specify dynamic behavior for indexing operations.
		/// </summary>
		/// <param name="binder">Provides information about the operation.</param>
		/// <param name="indexes">
		///     The indexes that are used in the operation. For example,
		///     for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where
		///     sampleObject is derived from the DynamicObject class, <paramref name="indexes[0]" />
		///     is equal to 3.
		/// </param>
		/// <param name="result">The result of the index operation.</param>
		/// <returns>
		///     true if the operation is successful; otherwise, false. If this method
		///     returns false, the run-time binder of the language determines the behavior. (In
		///     most cases, a run-time exception is thrown.).
		/// </returns>
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			var index = (int) indexes[0];
			IReadOnlyCollection<IWebElement> elements = Locator.FindElements();
			try
			{
				result = elements.ElementAt(index);
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return Name;
		}
	}
}