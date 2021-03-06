﻿using System;
using System.Reflection;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     IFillableElement interface.
	/// </summary>
	public interface IFillableElement
	{
		/// <summary>
		///     Gets The type of the web element.
		/// </summary>
		/// <value>The type of the element.</value>
		Type ElementType { get; }

		/// <summary>
		///     Enters the model property value to page object.
		/// </summary>
		/// <param name="pageObject">The page object.</param>
		/// <param name="matchedPageObjectProperty">The matched page object property.</param>
		/// <param name="modelValue">The model value.</param>
		void EnterModelPropertyValueToPageObject(object pageObject, PropertyInfo matchedPageObjectProperty, object modelValue);
	}
}