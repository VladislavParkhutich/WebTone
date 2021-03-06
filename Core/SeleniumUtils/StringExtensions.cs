using System;
using System.Text;

namespace Core.SeleniumUtils
{
	/// <summary>
	///     StringExtensions class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		///     Replaces the first.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="search">The search.</param>
		/// <param name="replace">The replace.</param>
		/// <returns>The value.</returns>
		public static string ReplaceFirst(this string text, string search, string replace)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			var pos = text.IndexOf(search, StringComparison.Ordinal);
			if (pos < 0)
			{
				return text;
			}

			return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}

		/// <summary>
		///     Determines whether [contains] [the specified source].
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="toCheck">To check.</param>
		/// <param name="comp">The comp.</param>
		/// <returns>The flag.</returns>
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source.IndexOf(toCheck, comp) >= 0;
		}

		/// <summary>
		///     Replaces at.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="index">The index.</param>
		/// <param name="newCharacter">The new character.</param>
		/// <returns>The value.</returns>
		public static string ReplaceAt(this string input, int index, char newCharacter)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}

			var builder = new StringBuilder(input);
			builder[index] = newCharacter;
			return builder.ToString();
		}

		/// <summary>
		///     Removes currency sign and brackets.
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <returns>The value.</returns>
		public static string RemoveCurrencySignAndBrackets(this string value)
		{
			return value.RemoveCurrencySign().RemoveBrackets();
		}

		/// <summary>
		///     Removes brackets.
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <returns>The value.</returns>
		public static string RemoveBrackets(this string value)
		{
			return value.Replace("(", string.Empty).Replace(")", string.Empty);
		}

		/// <summary>
		///     Removes currency sign.
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <returns>The value.</returns>
		public static string RemoveCurrencySign(this string value)
		{
			return value.Replace("$", string.Empty).Replace("£", string.Empty);
		}

		/// <summary>
		///		Removes escape codes.
		/// </summary>
		/// <param name="value">The string value.</param>
		/// <returns>The value.</returns>
		public static string RemoveEscapeCodes(this string value)
		{
			return value.Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);
		}
	}
}