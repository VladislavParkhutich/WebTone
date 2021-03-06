using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.GeneralUtils.MSTest
{
	/// <summary>
	///     MSTestContext class.
	/// </summary>
	public static class MSTestContext
	{
		[ThreadStatic] private static TestContext instance;

		/// <summary>
		///     Gets or sets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static TestContext Instance
		{
			get { return instance; }

			set { instance = value; }
		}
	}
}