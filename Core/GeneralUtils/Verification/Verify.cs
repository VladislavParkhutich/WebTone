using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using Core.GeneralUtils.Container;
using Core.SeleniumUtils;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.GeneralUtils.Verification
{
	/// <summary>
	///     Class for verification.
	/// </summary>
	[Serializable]
	public class Verify
	{
		private readonly List<UnitTestVerifyException> exceptions = new List<UnitTestVerifyException>();

		/// <summary>
		///     Gets or sets the verify failed.
		/// </summary>
		/// <value>The verify failed.</value>
		public Action<UnitTestVerifyException> VerifyFailed { get; set; }

		/// <summary>
		///     Gets a value indicating whether exceptions exist.
		/// </summary>
		/// <value>The has fails.</value>
		public bool HasFails
		{
			get { return exceptions.Count != 0; }
		}

		/// <summary>
		///     Arethe different objects equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="membersToIgnore">The members to ignore.</param>
		/// <param name="ignoreOrder">The ignore order.</param>
		/// <param name="errorMessage">The error message.</param>
		public void AreDifferentObjectsEqual(object expected, object actual, IList<string> membersToIgnore = null,
			bool ignoreOrder = true, string errorMessage = "")
		{
			var config = new ComparisonConfig
			{
				MaxDifferences = 100,
				IgnoreObjectTypes = true,
				IgnoreCollectionOrder = ignoreOrder,
				CustomComparers = new List<BaseTypeComparer> {new CustomGlobalDateComparer()}
			};

			if (membersToIgnore != null)
			{
				config.MembersToIgnore = membersToIgnore.ToList();
			}

			var compareLogic = new CompareLogic {Config = config};
			var result = compareLogic.Compare(expected, actual);
			IsTrue(result.AreEqual, errorMessage + ": " + result.DifferencesString);
		}

		/// <summary>
		///     Are the collections equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreOrder">The ignore order.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="membersToIgnore">The members to ignore.</param>
		public void AreCollectionsEqual<T>(IList<T> expected, IList<T> actual, bool ignoreOrder = true,
			string errorMessage = "", IList<string> membersToIgnore = null)
		{
			var config = new ComparisonConfig
			{
				MaxDifferences = 100,
				IgnoreCollectionOrder = ignoreOrder,
				CustomComparers = new List<BaseTypeComparer> {new CustomGlobalDateComparer()}
			};

			if (membersToIgnore != null)
			{
				config.MembersToIgnore = membersToIgnore.ToList();
			}

			var compareLogic = new CompareLogic {Config = config};
			var result = compareLogic.Compare(expected, actual);
			IsTrue(result.AreEqual, errorMessage + ": " + result.DifferencesString);
		}

		/// <summary>
		///     Verify is the collection contains values from another collection.
		/// </summary>
		/// <typeparam name="T">The Type of collections.</typeparam>
		/// <param name="expected">The Collection with expected values.</param>
		/// <param name="actual">The Collection that must contains expected values.</param>
		/// <param name="errorMessage">The error message.</param>
		public void CollectionContains<T>(IList<T> expected, IList<T> actual, string errorMessage = "")
		{
			var errors = new StringBuilder();

			foreach (var value in expected.Where(value => !actual.Contains(value)))
			{
				errors.AppendFormat(CultureInfo.InvariantCulture, "The following value is missing: {0}", value);
				errors.AppendLine();
			}

			AreEqual(0, errors.Length,
				errors.AppendFormat(CultureInfo.InvariantCulture, errorMessage)
					.AppendLine()
					.AppendFormat(CultureInfo.InvariantCulture, "Actual values are:")
					.AppendLine()
					.AppendFormat(CultureInfo.InvariantCulture, string.Join("\n", actual.ToArray()))
					.ToString());
		}

		/// <summary>
		///     Verify <see cref="actualValue" /> contains <see cref="expectedStringSegment" />.
		/// </summary>
		/// <param name="expectedStringSegment">The expected string segment.</param>
		/// <param name="actualValue">The actual string.</param>
		/// <param name="errorMessage">The error message.</param>
		public void StringContains(object expectedStringSegment, string actualValue, string errorMessage = "")
		{
			IsTrue(actualValue.Contains(expectedStringSegment.ToString()),
				errorMessage + " The actual string '" + actualValue + "' not contains expected string segment '" +
				expectedStringSegment + "'");
		}

		/// <summary>
		///     Determines whether [is collection empty] [the specified actual].
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="actual">The actual.</param>
		/// <param name="errorMessage">The error message.</param>
		public void IsCollectionEmpty<T>(ICollection<T> actual, string errorMessage = "")
		{
			IsNotNull(actual, errorMessage);
			AreEqual(0, actual.Count, errorMessage);
		}

		/// <summary>
		///     Determines whether [is collection not empty] [the specified actual].
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="actual">The actual.</param>
		/// <param name="errorMessage">The error message.</param>
		public void IsCollectionNotEmpty<T>(ICollection<T> actual, string errorMessage = "")
		{
			IsNotNull(actual, errorMessage);
			AreNotEqual(0, actual.Count, errorMessage);
		}

		/// <summary>
		///     Determines whether the specified condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		public void IsTrue(bool condition)
		{
			IsTrue(condition, string.Empty, null);
		}

		/// <summary>
		///     Determines whether the specified condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message.</param>
		public void IsTrue(bool condition, string message)
		{
			IsTrue(condition, message, null);
		}

		/// <summary>
		///     Determines whether the specified condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void IsTrue(bool condition, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.IsTrue(condition, message, parameters));
		}

		/// <summary>
		///     Are the equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreEqual<T>(T expected, T actual)
		{
			AreEqual(expected, actual, string.Empty, null);
		}

		/// <summary>
		///     Are the equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreEqual<T>(T expected, T actual, string message)
		{
			AreEqual(expected, actual, message, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual<T>(T expected, T actual, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreEqual(expected, actual, message, parameters));
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreEqual(object expected, object actual)
		{
			AreEqual(expected, actual, string.Empty, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(object expected, object actual, string message)
		{
			AreEqual(expected, actual, message, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(object expected, object actual, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreEqual(expected, actual, message, parameters));
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreEqual(double expected, double actual, double delta)
		{
			AreEqual(expected, actual, delta, string.Empty, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(double expected, double actual, double delta, string message)
		{
			AreEqual(expected, actual, delta, message, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(double expected, double actual, double delta, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreEqual(expected, actual, delta, message, parameters));
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreEqual(float expected, float actual, float delta)
		{
			AreEqual(expected, actual, delta, string.Empty, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(float expected, float actual, float delta, string message)
		{
			AreEqual(expected, actual, delta, message, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(float expected, float actual, float delta, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreEqual(expected, actual, delta, message, parameters));
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase)
		{
			AreEqual(expected, actual, ignoreCase, string.Empty, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, string message)
		{
			AreEqual(expected, actual, ignoreCase, message, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreEqual(expected, actual, ignoreCase, message, parameters));
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture)
		{
			AreEqual(expected, actual, ignoreCase, culture, string.Empty, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message)
		{
			AreEqual(expected, actual, ignoreCase, culture, message, null);
		}

		/// <summary>
		///     Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message,
			params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreEqual(expected, actual, ignoreCase, culture, message, parameters));
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreNotEqual<T>(T notExpected, T actual)
		{
			AreNotEqual(notExpected, actual, string.Empty, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual<T>(T notExpected, T actual, string message)
		{
			AreNotEqual(notExpected, actual, message, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual<T>(T notExpected, T actual, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, message, parameters));
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreNotEqual(object notExpected, object actual)
		{
			AreNotEqual(notExpected, actual, string.Empty, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(object notExpected, object actual, string message)
		{
			AreNotEqual(notExpected, actual, message, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(object notExpected, object actual, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, message, parameters));
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreNotEqual(double notExpected, double actual, double delta)
		{
			AreNotEqual(notExpected, actual, delta, string.Empty, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(double notExpected, double actual, double delta, string message)
		{
			AreNotEqual(notExpected, actual, delta, message, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(double notExpected, double actual, double delta, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, delta, message, parameters));
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreNotEqual(float notExpected, float actual, float delta)
		{
			AreNotEqual(notExpected, actual, delta, string.Empty, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(float notExpected, float actual, float delta, string message)
		{
			AreNotEqual(notExpected, actual, delta, message, null);
		}

		/// <summary>
		///     Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(float notExpected, float actual, float delta, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, delta, message, parameters));
		}

		/// <summary>
		///     Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase)
		{
			AreNotEqual(notExpected, actual, ignoreCase, string.Empty, null);
		}

		/// <summary>
		///     Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message)
		{
			AreNotEqual(notExpected, actual, ignoreCase, message, null);
		}

		/// <summary>
		///     Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, ignoreCase, message, parameters));
		}

		/// <summary>
		///     Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture)
		{
			AreNotEqual(notExpected, actual, ignoreCase, culture, string.Empty, null);
		}

		/// <summary>
		///     Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message)
		{
			AreNotEqual(notExpected, actual, ignoreCase, culture, message, null);
		}

		/// <summary>
		///     Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message,
			params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, ignoreCase, culture, message, parameters));
		}

		/// <summary>
		///     Checks objects are not the same.
		/// </summary>
		/// <param name="notExpected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		public void AreNotSame(object notExpected, object actual)
		{
			AreNotEqual(notExpected, actual, string.Empty, null);
		}

		/// <summary>
		///     Checks objects are not the same.
		/// </summary>
		/// <param name="notExpected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		public void AreNotSame(object notExpected, object actual, string message)
		{
			AreNotEqual(notExpected, actual, message, null);
		}

		/// <summary>
		///     Checks objects are not the same.
		/// </summary>
		/// <param name="notExpected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void AreNotSame(object notExpected, object actual, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreNotSame(notExpected, actual, message, parameters));
		}

		/// <summary>
		///     Checks objects are the same.
		/// </summary>
		/// <param name="expected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		public void AreSame(object expected, object actual)
		{
			ExecuteSafely(() => Assert.AreSame(expected, actual));
		}

		/// <summary>
		///     Checks objects are the same.
		/// </summary>
		/// <param name="expected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		public void AreSame(object expected, object actual, string message)
		{
			ExecuteSafely(() => Assert.AreSame(expected, actual, message));
		}

		/// <summary>
		///     Checks objects are the same.
		/// </summary>
		/// <param name="expected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void AreSame(object expected, object actual, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.AreSame(expected, actual, message, parameters));
		}

		/// <summary>
		///     Calls assert's Fail method.
		/// </summary>
		public void Fail()
		{
			Fail(string.Empty, null);
		}

		/// <summary>
		///     Calls assert's Fail method.
		/// </summary>
		/// <param name="message">The Message.</param>
		public void Fail(string message)
		{
			Fail(message, null);
		}

		/// <summary>
		///     Calls assert's Fail method.
		/// </summary>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void Fail(string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.Fail(message, parameters));
		}

		/// <summary>
		///     Calls assert's Inconclusive method.
		/// </summary>
		public void Inconclusive()
		{
			Inconclusive(string.Empty, null);
		}

		/// <summary>
		///     Calls assert's Inconclusive method.
		/// </summary>
		/// <param name="message">The Message.</param>
		public void Inconclusive(string message)
		{
			Inconclusive(message, null);
		}

		/// <summary>
		///     Calls assert's Inconclusive method.
		/// </summary>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void Inconclusive(string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.Inconclusive(message, parameters));
		}

		/// <summary>
		///     Checks whether condition is false.
		/// </summary>
		/// <param name="condition">The Condition.</param>
		public void IsFalse(bool condition)
		{
			IsFalse(condition, string.Empty, null);
		}

		/// <summary>
		///     Checks whether condition is false.
		/// </summary>
		/// <param name="condition">The Condition.</param>
		/// <param name="message">The Message.</param>
		public void IsFalse(bool condition, string message)
		{
			IsFalse(condition, message, null);
		}

		/// <summary>
		///     Checks whether condition is false.
		/// </summary>
		/// <param name="condition">The Condition.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsFalse(bool condition, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.IsFalse(condition, message, parameters));
		}

		/// <summary>
		///     Checks whether value is an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="expectedType">The Expected Type.</param>
		public void IsInstanceOfType(object value, Type expectedType)
		{
			IsInstanceOfType(value, expectedType, string.Empty, null);
		}

		/// <summary>
		///     Checks whether value is an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="expectedType">The Expected Type.</param>
		/// <param name="message">The Message.</param>
		public void IsInstanceOfType(object value, Type expectedType, string message)
		{
			IsInstanceOfType(value, expectedType, message, null);
		}

		/// <summary>
		///     Checks whether value is an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="expectedType">The Expected Type.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsInstanceOfType(object value, Type expectedType, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.IsInstanceOfType(value, expectedType, message, parameters));
		}

		/// <summary>
		///     Checks whether value is not an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="wrongType">The Wrong Type.</param>
		public void IsNotInstanceOfType(object value, Type wrongType)
		{
			IsNotInstanceOfType(value, wrongType, string.Empty, null);
		}

		/// <summary>
		///     Checks whether value is not an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="wrongType">The Wrong Type.</param>
		/// <param name="message">The Message.</param>
		public void IsNotInstanceOfType(object value, Type wrongType, string message)
		{
			IsNotInstanceOfType(value, wrongType, message, null);
		}

		/// <summary>
		///     Checks whether value is not an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="wrongType">The Wrong Type.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsNotInstanceOfType(object value, Type wrongType, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.IsNotInstanceOfType(value, wrongType, message, parameters));
		}

		/// <summary>
		///     Checks whether value is not null.
		/// </summary>
		/// <param name="value">The Value.</param>
		public void IsNotNull(object value)
		{
			IsNotNull(value, string.Empty, null);
		}

		/// <summary>
		///     Checks whether value is not null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		public void IsNotNull(object value, string message)
		{
			IsNotNull(value, message, null);
		}

		/// <summary>
		///     Checks whether value is not null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsNotNull(object value, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.IsNotNull(value, message, parameters));
		}

		/// <summary>
		///     Checks whether value is null.
		/// </summary>
		/// <param name="value">The Value.</param>
		public void IsNull(object value)
		{
			IsNull(value, string.Empty, null);
		}

		/// <summary>
		///     Checks whether value is null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		public void IsNull(object value, string message)
		{
			IsNull(value, message, null);
		}

		/// <summary>
		///     Checks whether value is null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsNull(object value, string message, params object[] parameters)
		{
			ExecuteSafely(() => Assert.IsNull(value, message, parameters));
		}

		/// <summary>
		///     Replaces Null Chars.
		/// </summary>
		/// <param name="input">The Text.</param>
		/// <returns>Reformated text.</returns>
		public string ReplaceNullChars(string input)
		{
			return Assert.ReplaceNullChars(input);
		}

		/// <summary>
		///     Checks exceptions.
		/// </summary>
		public void Check()
		{
			Logger.Verify(exceptions);

			if (!HasFails)
			{
				return;
			}

			var builder = new StringBuilder();
			builder.AppendLine("\nThe folowing verifiers failed:");
			exceptions.Select(ex => ex.Message).ForEach(m => builder.AppendLine(m.ReplaceFirst("Assert", "Verify")));

			var first = exceptions.First();
			throw new UnitTestVerifyException(new AggregateException(builder.ToString(), exceptions), first.StackTrace);
		}

		/// <summary>
		///     Execute action safely.
		/// </summary>
		/// <param name="action">The Action.</param>
		private void ExecuteSafely(Action action)
		{
			try
			{
				action();
			}
			catch (UnitTestAssertException e)
			{
				var exception = new UnitTestVerifyException(e, ClenupStackTrace());
				exceptions.Add(exception);
				if (VerifyFailed != null)
				{
					VerifyFailed(exception);
				}
			}
		}

		/// <summary>
		///     Cleans up Stack Trace.
		/// </summary>
		/// <returns>Call stack.</returns>
		private string ClenupStackTrace()
		{
			var stackTrace = new StackTrace(3, true);
			var frames = stackTrace.GetFrames().TakeWhile(x => x.GetILOffset() != -1);
			return GetCallStack(frames);
		}

		/// <summary>
		///     Gets Call Stack.
		/// </summary>
		/// <param name="frames">The Frames.</param>
		/// <returns>Call stack.</returns>
		private string GetCallStack(IEnumerable<StackFrame> frames)
		{
			var text = "at";
			var format = "in {0}:line {1}";
			var flag = true;
			var stringBuilder = new StringBuilder(255);
			foreach (var frame in frames)
			{
				var method = frame.GetMethod();
				if (method != null)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(Environment.NewLine);
					}

					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "   {0} ", text);
					var declaringType = method.DeclaringType;
					if (declaringType != null)
					{
						stringBuilder.Append(declaringType.FullName.Replace('+', '.'));
						stringBuilder.Append(".");
					}

					stringBuilder.Append(method.Name);
					if (method is MethodInfo && method.IsGenericMethod)
					{
						var genericArguments = method.GetGenericArguments();
						stringBuilder.Append("[");
						var j = 0;
						var flag2 = true;
						while (j < genericArguments.Length)
						{
							if (!flag2)
							{
								stringBuilder.Append(",");
							}
							else
							{
								flag2 = false;
							}

							stringBuilder.Append(genericArguments[j].Name);
							j++;
						}

						stringBuilder.Append("]");
					}

					stringBuilder.Append("(");
					var parameters = method.GetParameters();
					var flag3 = true;
					for (var k = 0; k < parameters.Length; k++)
					{
						if (!flag3)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							flag3 = false;
						}

						var str = "<UnknownType>";
						if (parameters[k].ParameterType != null)
						{
							str = parameters[k].ParameterType.Name;
						}

						stringBuilder.Append(str + " " + parameters[k].Name);
					}

					stringBuilder.Append(")");
					if (frame.GetILOffset() != -1)
					{
						string text2 = null;
						try
						{
							text2 = frame.GetFileName();
						}
						catch (SecurityException)
						{
						}

						if (text2 != null)
						{
							stringBuilder.Append(' ');
							stringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, text2, frame.GetFileLineNumber());
						}
					}
				}
			}

			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}
	}
}