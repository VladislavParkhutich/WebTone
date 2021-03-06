using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.GeneralUtils.Verification
{
	/// <summary>
	///     Unit Test Verify Exception class.
	/// </summary>
	[Serializable]
	public class UnitTestVerifyException : UnitTestAssertException
	{
		private readonly Exception originalException;

		private readonly string originalStackTrace;

		/// <summary>
		///     Initializes a new instance of the <see cref="UnitTestVerifyException" /> class.
		/// </summary>
		public UnitTestVerifyException()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="UnitTestVerifyException" /> class.
		/// </summary>
		/// <param name="originalStackTrace">The original stack trace.</param>
		public UnitTestVerifyException(string originalStackTrace)
		{
			this.originalStackTrace = originalStackTrace;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="UnitTestVerifyException" /> class.
		/// </summary>
		/// <param name="originalStackTrace">The original stack trace.</param>
		/// <param name="originalException">The original exception.</param>
		public UnitTestVerifyException(string originalStackTrace, Exception originalException)
		{
			this.originalStackTrace = originalStackTrace;
			this.originalException = originalException;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="UnitTestVerifyException" /> class.
		/// </summary>
		/// <param name="originalException">The original exception.</param>
		/// <param name="originalStackTrace">The original stack trace.</param>
		public UnitTestVerifyException(Exception originalException, string originalStackTrace)
		{
			this.originalException = originalException;
			this.originalStackTrace = originalStackTrace;
		}

		/// <summary>
		///     Gets a collection of key/value pairs that provide additional user-defined
		///     information about the exception.
		/// </summary>
		/// <returns>
		///     An object that implements the <see cref="T:System.Collections.IDictionary" />
		///     interface and contains a collection of user-defined key/value pairs. The default
		///     is an empty collection.
		/// </returns>
		/// <value>The Dictionary.</value>
		public override IDictionary Data
		{
			get { return originalException.Data; }
		}

		/// <summary>
		///     Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public override string Message
		{
			get { return originalException.Message; }
		}

		/// <summary>
		///     Gets or sets the name of the application or the object that causes the error.
		/// </summary>
		/// <returns>The name of the application or the object that causes the error.</returns>
		/// <exception cref="T:System.ArgumentException">The object must be a runtime <see cref="N:System.Reflection" /> object.</exception>
		/// <value>The source.</value>
		public override string Source
		{
			get { return originalException.Source; }
		}

		/// <summary>
		///     Provides COM objects with version-independent access to the <see cref="P:System.Exception.StackTrace" />
		///     property.
		/// </summary>
		/// <returns>
		///     A string that describes the contents of the call stack, with the most
		///     recent method call appearing first.
		/// </returns>
		/// <value>Stack Trace.</value>
		public override string StackTrace
		{
			get { return originalStackTrace; }
		}
	}
}