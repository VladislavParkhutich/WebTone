using System;

namespace Core.GeneralUtils.Verification
{
	/// <summary>
	///     Verify context class.
	/// </summary>
	public static class VerifyContext
	{
		[ThreadStatic] private static Verify current;

		/// <summary>
		///     Gets or sets the current.
		/// </summary>
		/// <value>The current.</value>
		public static Verify Current
		{
			get { return current; }

			set { current = value; }
		}
	}
}