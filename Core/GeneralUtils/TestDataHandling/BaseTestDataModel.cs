namespace Core.GeneralUtils.TestDataHandling
{
	/// <summary>
	///     All models dedicated to the test data data handling should inherit this class.
	/// </summary>
	public abstract class BaseTestDataModel
	{
		/// <summary>
		///     Gets or sets the test data id.
		/// </summary>
		/// <value>The test data id.</value>
		public string TestDataId { get; set; }

		/// <summary>
		///     Shoulds the serialize test data id.
		/// </summary>
		/// <returns>The flag.</returns>
		public bool ShouldSerializeTestDataId()
		{
			return false;
		}
	}
}