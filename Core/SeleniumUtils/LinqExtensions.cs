using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.SeleniumUtils
{
	/// <summary>
	///     LinqExtensions class.
	/// </summary>
	public static class LinqExtensions
	{
		/// <summary>
		///     Used to modify properties of an object returned from a LINQ query.
		/// </summary>
		/// <typeparam name="TSource">The type of the T source.</typeparam>
		/// <param name="input">The input.</param>
		/// <param name="updater">The updater.</param>
		/// <returns>The object of TSource type.</returns>
		public static TSource Set<TSource>(this TSource input, Action<TSource> updater)
		{
			updater(input);
			return input;
		}

		/// <summary>
		///     Indexeses the where.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The collection.</returns>
		public static IEnumerable<int> IndexesWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			var index = 0;
			foreach (var element in source)
			{
				if (predicate(element))
				{
					yield return index;
				}

				index++;
			}
		}

		/// <summary>
		///     Repeats the action.
		/// </summary>
		/// <param name="repeatCount">The repeat count.</param>
		/// <param name="action">The action.</param>
		public static void RepeatAction(int repeatCount, Action action)
		{
			for (var i = 0; i < repeatCount; i++)
			{
				action();
			}
		}

		/// <summary>
		///     Orderby with direction.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="keySelector">The key selector.</param>
		/// <param name="ascending">if set to <c>true</c> then order by ascending, otherwise - descending.</param>
		/// <returns>The collection.</returns>
		public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey>(this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector, bool ascending)
		{
			return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
		}
	}
}