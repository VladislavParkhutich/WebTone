using Core.GeneralUtils.Container;

namespace Core.SeleniumUtils.Core.Objects
{
	/// <summary>
	///     The base object, contains references to general items, which can be used by views, scenarios, tests, other helpers.
	/// </summary>
	public class UIInfrastructureObject : InfrastructureObject
	{
		#region Properties

		/// <summary>
		///     Gets the browser.
		/// </summary>
		/// <value>The browser.</value>
		protected Browser Browser
		{
			get { return Resolve<Browser>(); }
		}

		#endregion
	}
}