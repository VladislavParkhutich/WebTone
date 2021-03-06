using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Core.SeleniumUtils.Core.HtmlElements.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Core.SeleniumUtils.Core.HtmlElements.Utils
{
	/// <summary>
	///     HtmlElementUtils class.
	/// </summary>
	public static class HtmlElementUtils
	{
		private const char PathDelimeter = '/';

		private const char NamespaceDelimeter = '.';

		/// <summary>
		///     News the instance.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="type">The type.</param>
		/// <param name="args">The args.</param>
		/// <returns>The result.</returns>
		public static T NewInstance<T>(Type type, params object[] args)
		{
			var resultType = typeof(T);
			if (type.IsClass && resultType.IsAssignableFrom(type))
			{
				return (T) Activator.CreateInstance(type, args);
			}

			return default(T);
		}

		/// <summary>
		///     Determines whether [is on remote web driver] [the specified element].
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The result.</returns>
		public static bool IsOnRemoteWebDriver(IWebElement element)
		{
			return IsRemoteWebElement(element);
		}

		/// <summary>
		///     Determines whether [is remote web element] [the specified element].
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The result.</returns>
		public static bool IsRemoteWebElement(IWebElement element)
		{
			return element.GetType() == typeof(RemoteWebElement);
		}

		/// <summary>
		///     Determines whether [is exist in assembly] [the specified file name].
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>The result.</returns>
		public static bool IsExistInAssembly(string fileName)
		{
			var assembly = Assembly.GetEntryAssembly();
			var info = assembly.GetManifestResourceInfo(fileName);
			return info != null;
		}

		/// <summary>
		///     Extracts the resource.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>The result.</returns>
		public static string ExtractResource(Uri uri)
		{
			var hostAssembly = uri.Host;
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.GetName().Name == hostAssembly)
				{
					var nameAndNamespacePath = uri.AbsolutePath;
					var namespaceStr = nameAndNamespacePath.Substring(0, nameAndNamespacePath.IndexOf(PathDelimeter));
					var nameStr = nameAndNamespacePath.Substring(nameAndNamespacePath.IndexOf(PathDelimeter) + 1,
						nameAndNamespacePath.Length);
					var nameAndNamespace = namespaceStr + NamespaceDelimeter + nameStr;

					var resource = assembly.GetManifestResourceStream(nameAndNamespace);
					if (resource != null)
					{
						var tempFilePath = Path.GetTempPath() + Path.PathSeparator + Path.GetRandomFileName() + Path.PathSeparator +
						                   nameStr;
						CopyResource(resource, tempFilePath);
						return tempFilePath;
					}
				}
			}

			return null;
		}

		/// <summary>
		///     Copies the resource.
		/// </summary>
		/// <param name="from">The From.</param>
		/// <param name="to">The To.</param>
		private static void CopyResource(Stream from, string to)
		{
			using (from)
			{
				using (var fileStream = new FileStream(to, FileMode.Create))
				{
					for (var i = 0; i < from.Length; i++)
					{
						fileStream.WriteByte((byte) from.ReadByte());
					}
				}
			}
		}

		/// <summary>
		///     Determines whether [is web element] [the specified property].
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static bool IsWebElement(PropertyInfo property)
		{
			return IsWebElement(property.PropertyType);
		}

		/// <summary>
		///     Determines whether [is web element] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		public static bool IsWebElement(Type type)
		{
			return typeof(IWebElement).IsAssignableFrom(type);
		}

		/// <summary>
		///     Determines whether [is web element list] [the specified property].
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static bool IsWebElementList(PropertyInfo property)
		{
			return IsWebElementList(property.PropertyType);
		}

		/// <summary>
		///     Determines whether [is web element list] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		public static bool IsWebElementList(Type type)
		{
			if (!IsParametrizedGenericList(type))
			{
				return false;
			}

			var collectionParameterType = GetGenericParameterType(type);
			return IsWebElement(collectionParameterType);
		}

		/// <summary>
		///     Determines whether [is typified element] [the specified property].
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static bool IsTypifiedElement(PropertyInfo property)
		{
			return IsTypifiedElement(property.PropertyType);
		}

		/// <summary>
		///     Determines whether [is typified element] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		private static bool IsTypifiedElement(Type type)
		{
			return typeof(TypifiedElement).IsAssignableFrom(type);
		}

		/// <summary>
		///     Determines whether [is typified element list] [the specified property].
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static bool IsTypifiedElementList(PropertyInfo property)
		{
			return IsTypifiedElementList(property.PropertyType);
		}

		/// <summary>
		///     Determines whether [is typified element list] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		private static bool IsTypifiedElementList(Type type)
		{
			if (!IsParametrizedGenericList(type))
			{
				return false;
			}

			var collectionParameterType = GetGenericParameterType(type);
			return IsTypifiedElement(collectionParameterType);
		}

		/// <summary>
		///     Determines whether [is HTML element] [the specified property].
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static bool IsHtmlElement(PropertyInfo property)
		{
			return IsHtmlElement(property.PropertyType);
		}

		/// <summary>
		///     Determines whether [is HTML element] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		public static bool IsHtmlElement(Type type)
		{
			return typeof(HtmlElement).IsAssignableFrom(type);
		}

		/// <summary>
		///     Determines whether [is HTML element] [the specified instance].
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <returns>The result.</returns>
		public static bool IsHtmlElement(object instance)
		{
			return typeof(HtmlElement).IsAssignableFrom(instance.GetType());
		}

		/// <summary>
		///     Determines whether [is HTML element list] [the specified property].
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static bool IsHtmlElementList(PropertyInfo property)
		{
			return IsHtmlElementList(property.PropertyType);
		}

		/// <summary>
		///     Determines whether [is HTML element list] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		public static bool IsHtmlElementList(Type type)
		{
			if (!IsParametrizedGenericList(type))
			{
				return false;
			}

			var collectionParameterType = GetGenericParameterType(type);
			return IsHtmlElement(collectionParameterType);
		}

		/// <summary>
		///     Determines whether [is parametrized generic list] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		private static bool IsParametrizedGenericList(Type type)
		{
			return IsGenericList(type) && !HasUndefinedGenericParameters(type);
		}

		/// <summary>
		///     Determines whether [is generic list] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		private static bool IsGenericList(Type type)
		{
			return type.IsGenericType && typeof(IList<>) == type.GetGenericTypeDefinition();
		}

		/// <summary>
		///     Determines whether [has undefined generic parameters] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		private static bool HasUndefinedGenericParameters(Type type)
		{
			return type.ContainsGenericParameters;
		}

		/// <summary>
		///     Gets the type of the generic parameter.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns>The result.</returns>
		public static Type GetGenericParameterType(FieldInfo field)
		{
			return GetGenericParameterType(field.FieldType);
		}

		/// <summary>
		///     Gets the type of the generic parameter.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static Type GetGenericParameterType(PropertyInfo property)
		{
			return GetGenericParameterType(property.PropertyType);
		}

		/// <summary>
		///     Gets the type of the generic parameter.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		public static Type GetGenericParameterType(Type type)
		{
			if (HasUndefinedGenericParameters(type))
			{
				return null;
			}

			return type.GenericTypeArguments[0];
		}

		/// <summary>
		///     Gets the name of the element.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The result.</returns>
		public static string GetElementName(PropertyInfo property)
		{
			var name = property.GetCustomAttribute<NameAttribute>(false);
			if (name != null)
			{
				return name.Name;
			}

			name = property.PropertyType.GetCustomAttribute<NameAttribute>(false);
			if (name != null)
			{
				return name.Name;
			}

			return SplitCamelCase(property.Name);
		}

		/// <summary>
		///     Gets the name of the element.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The result.</returns>
		public static string GetElementName(Type type)
		{
			var name = type.GetCustomAttribute<NameAttribute>(false);
			if (name != null)
			{
				return name.Name;
			}

			return SplitCamelCase(type.Name);
		}

		/// <summary>
		///     Splits the camel case.
		/// </summary>
		/// <param name="camel">The camel.</param>
		/// <returns>The result.</returns>
		private static string SplitCamelCase(string camel)
		{
			var regex =
				new Regex(string.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}", "(?<=[A-Z])(?=[A-Z][a-z])",
					"(?<=[^A-Z])(?=[A-Z])", "(?<=[A-Za-z])(?=[^A-Za-z])"));
			var separateWords = regex.Replace(camel, " ");
			return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(separateWords);
		}
	}
}