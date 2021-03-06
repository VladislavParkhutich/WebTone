using Core.GeneralUtils.MSTest;
using Core.GeneralUtils.TestDataHandling;
using Core.GeneralUtils.Verification;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.GeneralUtils.Container
{
	/// <summary>
	///     Infrastructure Object class.
	/// </summary>
	public abstract class InfrastructureObject
	{
		/// <summary>
		///     Gets or sets the test context.
		/// </summary>
		/// <value>The test context.</value>
		public TestContext TestContext
		{
			get { return MSTestContext.Instance; }

			set { MSTestContext.Instance = value; }
		}

		/// <summary>
		///     Gets the verify.
		/// </summary>
		/// <value>The verify.</value>
		public virtual Verify Verify
		{
			get { return VerifyContext.Current; }
		}

		/// <summary>
		///     Gets Test data reader.
		/// </summary>
		/// <value>The test data reader.</value>
		public ITestDataReader TestDataReader
		{
			get { return new JsonTestDataReader(); }
		}

		/// <summary>
		///     Gets Files data reader.
		/// </summary>
		/// <value>The files data reader.</value>
		public FilesTestDataReader FilesDataReader
		{
			get { return new FilesTestDataReader(); }
		}

		/// <summary>
		///     Gets Data source.
		/// </summary>
		/// <value>The data source.</value>
		protected DataSources DataSource
		{
			get { return Resolve<DataSources>(); }
		}

		/// <summary>
		///     Resolves passed type.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <returns>The object of T type.</returns>
		protected T Resolve<T>() where T : class
		{
			return UnityBootstrapper.Container.Resolve<T>();
		}
	}
}