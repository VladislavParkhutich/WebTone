﻿using System;
using Core.SeleniumUtils.Core.Objects;

namespace Core.SeleniumUtils.Core.ErrorHandling
{
	/// <summary>
	///     Base class for ui handling.
	/// </summary>
	internal abstract class BaseUIErrorHandler : UIInfrastructureObject, IUIErrorHandler
	{
		private readonly TimeSpan checkTimeout;

		private readonly string errorMessage;
		private readonly TimeSpan fullTimeout;

		/// <summary>
		///     Initializes a new instance of the <see cref="BaseUIErrorHandler" /> class.
		/// </summary>
		/// <param name="fullTimeout">The full timeout.</param>
		/// <param name="checkTimeout">The check timeout.</param>
		/// <param name="errorMessage">The error message.</param>
		protected BaseUIErrorHandler(TimeSpan fullTimeout, TimeSpan checkTimeout, string errorMessage)
		{
			this.fullTimeout = fullTimeout;
			this.checkTimeout = checkTimeout;
			this.errorMessage = errorMessage;
		}

		/// <summary>
		///     Gets a value indicating whether error exists.
		/// </summary>
		/// <value>
		///     <c>true</c> if error exists; otherwise, <c>false</c>.
		/// </value>
		public abstract bool ErrorExists { get; }

		/// <summary>
		///     Handles the error. .
		///     if <see cref="ErrorExists" /> is <c>false</c> then skip, otherwise call <see cref="TryHandleError()" /> every
		///     <see cref="checkTimeout" /> during <see cref="fullTimeout" />.
		///     If <see cref="ErrorExists" /> is <c>false</c> <see cref="TryHandleError()" /> then return.
		///     If <see cref="ErrorExists" /> is still <c>true</c> after the <see cref="fullTimeout" /> is passed then throw
		///     <see cref="TimeoutException" /> with <see cref="errorMessage" />
		/// </summary>
		public void HandleError()
		{
			if (!ErrorExists)
			{
				return;
			}

			Waiter.SpinWaitEnsureSatisfied(
				()
					=>
				{
					TryHandleError();
					return !ErrorExists;
				},
				fullTimeout,
				checkTimeout,
				errorMessage);
		}

		/// <summary>
		///     Should be overriden to specify steps to handle error.
		/// </summary>
		protected abstract void TryHandleError();
	}
}