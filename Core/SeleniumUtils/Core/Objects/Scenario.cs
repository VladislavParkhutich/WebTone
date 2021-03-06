using System;

namespace Core.SeleniumUtils.Core.Objects
{
	/// <summary>
	///     Scenario class.
	/// </summary>
	public abstract class Scenario : UIInfrastructureObject
	{
		#region Methods

		/// <summary>
		///     Waits for condition.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <returns>The result.</returns>
		protected virtual bool WaitForCondition(bool condition)
		{
			var isTrue = new Func<bool>(() => condition);

			return Waiter.SpinWait(isTrue, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(100));
		}

		/// <summary>
		///     Waits for condition.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <returns>The result.</returns>
		protected virtual bool WaitForCondition(Func<bool> condition)
		{
			return Waiter.SpinWait(condition, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(100));
		}

		#endregion
	}
}