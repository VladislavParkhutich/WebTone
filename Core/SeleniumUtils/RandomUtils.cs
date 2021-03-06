using System;
using System.Linq;

namespace Core.SeleniumUtils
{
	/// <summary>
	///     RandomUtils class.
	/// </summary>
	public static class RandomUtils
	{
		private static readonly Random rnd = new Random();

		/// <summary>
		///     Randomizes the GUID.
		/// </summary>
		/// <returns>The value.</returns>
		public static string RandomizeGuid()
		{
			return Guid.NewGuid().ToString("N");
		}

		/// <summary>
		///     Randomizes the Prism GUID (e.g. i + {Guid}).
		/// </summary>
		/// <returns>The value.</returns>
		public static string RandomizePrismGuid()
		{
			return "i" + Guid.NewGuid().ToString("N");
		}

		/// <summary>
		///     Randoms the boolean.
		/// </summary>
		/// <returns>Bool value.</returns>
		public static bool RandomBoolean()
		{
			return new Random().Next(2) == 0;
		}

		/// <summary>
		///     Generates random decimal.
		/// </summary>
		/// <param name="min">
		///     The min value.
		/// </param>
		/// <param name="max">
		///     The max value.
		/// </param>
		/// <returns>
		///     Returns randomly generated decimal value.
		/// </returns>
		public static decimal RandomDecimal(int min, int max)
		{
			var integerPart = (decimal) rnd.Next(min, max);
			var fractionPart = (decimal) rnd.Next(0, 99)/100;
			return integerPart + fractionPart;
		}

		/// <summary>
		///     Randoms the decimal with sizable fraction part.
		/// </summary>
		/// <param name="min">The minimum.</param>
		/// <param name="max">The maximum.</param>
		/// <param name="fractionSize">Size of the fract.</param>
		/// <returns>
		///     Decimal with fraction part.
		/// </returns>
		public static decimal RandomDecimalWithSizableFractionPart(int min, int max, int fractionSize)
		{
			var integerPart = (decimal) rnd.Next(min, max);
			var fractionPart = rnd.Next(0, (int) Math.Pow(10, fractionSize) - 1)/(decimal) Math.Pow(10, fractionSize);
			return integerPart + fractionPart;
		}

		/// <summary>
		///     The random double.
		/// </summary>
		/// <param name="min">
		///     The min.
		/// </param>
		/// <param name="max">
		///     The max.
		/// </param>
		/// <returns>
		///     The <see cref="double" />.
		/// </returns>
		public static double RandomDouble(int min, int max)
		{
			var integerPart = (double) rnd.Next(min, max);
			var fractionPart = (double) rnd.Next(0, 99)/100;
			return Math.Round(integerPart + fractionPart, 2);
		}

		/// <summary>
		///     The random int.
		/// </summary>
		/// <param name="min">
		///     The min.
		/// </param>
		/// <param name="max">
		///     The max.
		/// </param>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		public static int RandomNumeric(int min, int max)
		{
			return rnd.Next(min, max);
		}

		/// <summary>
		///     Randomizes the numeric string.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The value.</returns>
		public static string RandomizeNumericString(int size)
		{
			const string Chars = "0123456789";
			var randomNumericString = RandomizeString(size, Chars);

			while (randomNumericString.StartsWith("0", StringComparison.CurrentCultureIgnoreCase))
			{
				randomNumericString = randomNumericString.Replace("0", RandomizeString(1, Chars));
			}

			return randomNumericString;
		}

		/// <summary>
		///     Randomizes the alphabetical string.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The value.</returns>
		public static string RandomizeAlphabeticalString(int size)
		{
			const string Chars = "abcdefghigklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			return RandomizeString(size, Chars);
		}

		/// <summary>
		///     Randomizes the alphanumeric string.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The value.</returns>
		public static string RandomizeAlphanumericString(int size)
		{
			const string Chars = "abcdefghigklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return RandomizeString(size, Chars);
		}

		/// <summary>
		///     Randomizes the special symbols string.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The value.</returns>
		public static string RandomizeSpecialSymbolsString(int size)
		{
			const string Chars = "~`@#$%^&*()_+!;%:?*";
			return RandomizeString(size, Chars);
		}

		/// <summary>
		///     Randomizes the not english string.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The value.</returns>
		public static string RandomizeNotEnglishString(int size)
		{
			const string Chars =
				"¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýÿАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
			return RandomizeString(size, Chars);
		}

		/// <summary>
		///     Randomizes the alphanumeric string with special symbols.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The value.</returns>
		public static string RandomizeAlphanumericStringWithSpecialSymbols(int size)
		{
			const string Chars = "abcdefghigklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~`@#$%^&*()_+!;%:?*";
			return RandomizeString(size, Chars);
		}

		/// <summary>
		///     Randomizes the spaces string.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The value.</returns>
		public static string RandomizeSpacesString(int size)
		{
			const string Chars = " ";
			return RandomizeString(size, Chars);
		}

		/// <summary>
		///     General method to randomize strings.
		/// </summary>
		/// <param name="size">size of new string.</param>
		/// <param name="charsForRandom">Chars that will partcipate in random.</param>
		/// <returns>result random string.</returns>
		private static string RandomizeString(int size, string charsForRandom)
		{
			var buffer = new char[size];

			for (var i = 0; i < size; i++)
			{
				buffer[i] = charsForRandom[rnd.Next(charsForRandom.Length)];
			}

			return new string(buffer);
		}

		/// <summary>
		///     Generates the random number with exception.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		/// <param name="exclude">The exclude.</param>
		/// <returns>Generated number.</returns>
		public static int GenerateRandomNumberWithException(int start, int end, params int[] exclude)
		{
			var range = Enumerable.Range(start, end).Where(i => !exclude.Contains(i));
			var rand = new Random();
			var index = rand.Next(start, end - exclude.Length);
			return range.ElementAt(index);
		}
	}
}