using System;
using System.Runtime.Serialization;

namespace Core.SeleniumUtils.Core.HtmlElements.Exceptions
{
	/// <summary>
	///     HtmlElementsException class.
	/// </summary>
	[Serializable]
	public class HtmlElementsException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="HtmlElementsException" /> class.
		/// </summary>
		public HtmlElementsException()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HtmlElementsException" /> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public HtmlElementsException(string message) : base(message)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HtmlElementsException" /> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public HtmlElementsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HtmlElementsException" /> class.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		protected HtmlElementsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}