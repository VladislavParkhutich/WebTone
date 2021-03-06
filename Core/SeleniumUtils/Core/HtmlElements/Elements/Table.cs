using System;
using System.Collections.Generic;
using System.Linq;
using Core.SeleniumUtils.Core.HtmlElements.Exceptions;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     Table class.
	/// </summary>
	public class Table : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Table" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Table(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Gets the <see cref="System.Collections.Generic.List&lt;OpenQA.Selenium.IWebElement&gt;" /> at the specified index.
		/// </summary>
		/// <value>The value.</value>
		/// <param name="index">The index.</param>
		/// <returns>The result.</returns>
		public List<IWebElement> this[int index]
		{
			get { return GetRows()[index]; }
		}

		/// <summary>
		///     Gets the headings.
		/// </summary>
		/// <returns>The value.</returns>
		public List<IWebElement> GetHeadings()
		{
			return WrappedElement.FindElements(By.XPath(".//th")).ToList();
		}

		/// <summary>
		///     Gets the headings as string.
		/// </summary>
		/// <returns>The value.</returns>
		public List<string> GetHeadingsAsString()
		{
			return GetHeadings().ConvertAll(GetText);
		}

		/// <summary>
		///     Gets the rows.
		/// </summary>
		/// <returns>The value.</returns>
		public List<List<IWebElement>> GetRows()
		{
			var rows = new List<List<IWebElement>>();
			var rowElements = WrappedElement.FindElements(By.XPath(".//tr")).ToList();
			foreach (var rowElement in rowElements)
			{
				rows.Add(rowElement.FindElements(By.XPath(".//td")).ToList());
			}

			return rows;
		}

		/// <summary>
		///     Gets the rows as string.
		/// </summary>
		/// <returns>The value.</returns>
		public List<List<string>> GetRowsAsString()
		{
			return GetRows().ConvertAll(row => row.ConvertAll(element => GetText(element)));
		}

		/// <summary>
		///     Gets the columns.
		/// </summary>
		/// <returns>The value.</returns>
		public List<List<IWebElement>> GetColumns()
		{
			var columns = new List<List<IWebElement>>();
			var rows = GetRows();

			if (rows.Count <= 0)
			{
				return columns;
			}

			var columnsNumber = rows[0].Count;
			for (var i = 0; i < columnsNumber; i++)
			{
				var column = new List<IWebElement>();
				foreach (var row in rows)
				{
					column.Add(row[i]);
				}

				columns.Add(column);
			}

			return columns;
		}

		/// <summary>
		///     Gets the columns as string.
		/// </summary>
		/// <returns>The value.</returns>
		public List<List<string>> GetColumnsAsString()
		{
			return GetColumns().ConvertAll(row => row.ConvertAll(element => GetText(element)));
		}

		/// <summary>
		///     Gets the rows mapped to headings.
		/// </summary>
		/// <returns>The value.</returns>
		public List<IDictionary<string, IWebElement>> GetRowsMappedToHeadings()
		{
			return GetRowsMappedToHeadings(GetHeadingsAsString());
		}

		/// <summary>
		///     Gets the rows mapped to headings.
		/// </summary>
		/// <param name="headings">The headings.</param>
		/// <returns>The value.</returns>
		public List<IDictionary<string, IWebElement>> GetRowsMappedToHeadings(List<string> headings)
		{
			var rowsMappedToHeadings = new List<IDictionary<string, IWebElement>>();
			var rows = GetRows();

			if (rows.Count <= 0)
			{
				return rowsMappedToHeadings;
			}

			foreach (var row in rows)
			{
				if (row.Count != headings.Count)
				{
					throw new HtmlElementsException("Headings count is not equal to number of cells in row");
				}

				IDictionary<string, IWebElement> rowToHeadingsMap = new Dictionary<string, IWebElement>();
				var cellNumber = 0;
				foreach (var heading in headings)
				{
					rowToHeadingsMap[heading] = row[cellNumber];
					cellNumber++;
				}

				rowsMappedToHeadings.Add(rowToHeadingsMap);
			}

			return rowsMappedToHeadings;
		}

		/// <summary>
		///     Gets the rows as string mapped to headings.
		/// </summary>
		/// <returns>The value.</returns>
		public List<IDictionary<string, string>> GetRowsAsStringMappedToHeadings()
		{
			return GetRowsAsStringMappedToHeadings(GetHeadingsAsString());
		}

		/// <summary>
		///     Gets the rows as string mapped to headings.
		/// </summary>
		/// <param name="headings">The headings.</param>
		/// <returns>The value.</returns>
		public List<IDictionary<string, string>> GetRowsAsStringMappedToHeadings(List<string> headings)
		{
			Func<IDictionary<string, IWebElement>, IDictionary<string, string>> dictionaryConvert =
				delegate(IDictionary<string, IWebElement> from)
				{
					IDictionary<string, string> result = new Dictionary<string, string>();
					foreach (var pair in from)
					{
						result[pair.Key] = GetText(pair.Value);
					}

					return result;
				};
			return GetRowsMappedToHeadings(headings).ConvertAll(row => dictionaryConvert(row));
		}

		/// <summary>
		///     Gets the text.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The value.</returns>
		private string GetText(IWebElement element)
		{
			return element.Text;
		}
	}
}