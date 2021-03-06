using System;
using Core.GeneralUtils.TestDataHandling;
using Microsoft.Practices.Unity;

namespace Core.GeneralUtils.Container
{
	/// <summary>
	///     UnityBootstrapper class.
	/// </summary>
	public static class UnityBootstrapper
	{
		private static readonly Lazy<UnityContainer> ContainerLazy = new Lazy<UnityContainer>(() =>
		{
			var container = new UnityContainer();
			container.RegisterInstance(new UnityServiceLocator(container));
			container.RegisterType<ITestDataReader, JsonTestDataReader>();

			return container;
		});

		/// <summary>
		///     Gets the container.
		/// </summary>
		/// <value>The container.</value>
		public static UnityContainer Container
		{
			get { return ContainerLazy.Value; }
		}
	}
}