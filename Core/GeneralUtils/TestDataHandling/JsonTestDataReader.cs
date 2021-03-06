using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.GeneralUtils.TestDataHandling.Placeholders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.GeneralUtils.TestDataHandling
{
	/// <summary>
	///     JsonTestDataReader class.
	/// </summary>
	public class JsonTestDataReader : BaseTestDataReader
	{
		private const string TestIdJsonPath = "..TestDataId";

		private readonly Assembly specifiedAssembly;

		/// <summary>
		///     Initializes a new instance of the <see cref="JsonTestDataReader" /> class.
		///     Note that for Unit Test project you should set entry assembly
		///     in the first test line using <see cref="AssemblyUtilities.SetEntryAssembly()" />.
		/// </summary>
		public JsonTestDataReader()
		{
			specifiedAssembly = Assembly.GetEntryAssembly();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="JsonTestDataReader" /> class.
		/// </summary>
		/// <param name="specifiedAssembly">The specified assembly.</param>
		public JsonTestDataReader(Assembly specifiedAssembly)
		{
			this.specifiedAssembly = specifiedAssembly;
		}

		/// <summary>
		///     Reads the test data from the <see cref="specifiedAssembly" />.
		/// </summary>
		/// <typeparam name="T">Target Type.</typeparam>
		/// <param name="file">The file.</param>
		/// <param name="testDataIds">The test data ids.</param>
		/// <returns>Object of T type.</returns>
		public override T Read<T>(string file, params string[] testDataIds)
		{
			return Read<T>(specifiedAssembly, file, testDataIds);
		}

		/// <summary>
		///     Reads the test data from specified assembly.
		/// </summary>
		/// <typeparam name="T">Target Type.</typeparam>
		/// <param name="assembly">The assembly.</param>
		/// <param name="file">The file.</param>
		/// <param name="testDataIds">The test data ids.</param>
		/// <returns>The object of T type.</returns>
		public override T Read<T>(Assembly assembly, string file, params string[] testDataIds)
		{
			var specificDataList = new List<string>
			{
				"MatterRequest",
				"TrustAccountViewModel",
				"InvoicePaymentModel",
				"TrustAccountTransactionViewModel",
				"PreBillGenerateCriteriaModel",
				"TrustAccountViewModel"
			};
			var defaultType = typeof(T);

			using (
				var stream = LoadResourceStream(assembly,
					string.Format(CultureInfo.InvariantCulture, "{0}.{1}.json", TestDataFolderName, file)))
			{
				using (var reader = new StreamReader(stream))
				{
					var jsonText = reader.ReadToEnd();
					var processedJson = new PlaceholderReplacersContainer().ReplaceTokens(
						jsonText,
						defaultType.IsGenericType
							? specificDataList.Contains(defaultType.GetGenericArguments()[0].Name)
							: specificDataList.Contains(defaultType.Name));
					JToken jsonTokens;
					using (var textReader = new StringReader(processedJson))
					{
						using (var jsonReader = new JsonTextReader(textReader))
						{
							jsonReader.DateParseHandling = DateParseHandling.None;
							jsonTokens = JToken.Load(jsonReader);
						}
					}

					var tokens =
						testDataIds.SelectMany(tdi => jsonTokens.SelectTokens(TestIdJsonPath).Where(token => tdi == token.ToString()));

					if (defaultType.IsGenericType && defaultType.GetGenericArguments().Length == 1)
					{
						var underlineType = defaultType.GetGenericArguments()[0];
						var selectedTokens = tokens.Select(t => t.Parent.Parent.ToObject(underlineType));

						var listType = typeof(List<>);
						var constructedListType = listType.MakeGenericType(underlineType);
						var newList = Activator.CreateInstance(constructedListType) as IList;
						foreach (var token in selectedTokens)
						{
							if (newList != null)
							{
								newList.Add(token);
							}
						}

						return (T) newList;
					}

					var singleNode = tokens.Select(t => t.Parent.Parent.ToObject(defaultType)).Single();
					return (T) singleNode;
				}
			}
		}
	}
}