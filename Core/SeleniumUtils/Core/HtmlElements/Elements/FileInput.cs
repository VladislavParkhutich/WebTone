using System;
using System.IO;
using Core.SeleniumUtils.Core.HtmlElements.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     FileInput class.
	/// </summary>
	public class FileInput : TypifiedElement
	{
		private const string AssemblyScheme = "assembly";

		/// <summary>
		///     Initializes a new instance of the <see cref="FileInput" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public FileInput(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Pointing input field to a file.
		///     Use file URI scheme or absolute path for separate files: file://host/path; eg.
		///     file://localhost/c:/WINDOWS/clock.avi.
		///     Use Spring.Net notation for embedded resources: assembly://AssemblyName/NameSpace/ResourceName; eg.
		///     assembly://HtmlElements-DotNet/Yandex.HtmlElements/TestResource.txt.
		///     For embedded resources the resource will be unpacked to a temp dir.
		/// </summary>
		/// <param name="fileName">The fileName.</param>
		public void SetFileToUpload(string fileName)
		{
			// Proxy can't be used to check the element class, so find real WebElement
			var fileInputElement = GetNotProxiedInputElement();

			// Set local file detector in case of remote driver usage
			if (HtmlElementUtils.IsOnRemoteWebDriver(fileInputElement))
			{
				SetLocalFileDetector((RemoteWebElement) fileInputElement);
			}

			var filePath = GetFilePath(fileName);
			fileInputElement.SendKeys(filePath);
		}

		/// <summary>
		///     Submits this instance.
		/// </summary>
		public void Submit()
		{
			WrappedElement.Submit();
		}

		/// <summary>
		///     Gets the not proxied input element.
		/// </summary>
		/// <returns>The value.</returns>
		private IWebElement GetNotProxiedInputElement()
		{
			// Cannot get something from WebElementProxy class since it's 'internal'
			return WrappedElement.FindElement(By.XPath("."));
		}

		/// <summary>
		///     Sets the local file detector.
		/// </summary>
		/// <param name="element">The element.</param>
		private void SetLocalFileDetector(RemoteWebElement element)
		{
			((RemoteWebDriver) element.WrappedDriver).FileDetector = new LocalFileDetector();
		}

		/// <summary>
		///     Gets the file path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The value.</returns>
		private string GetFilePath(string path)
		{
			Uri fileUri = null;
			try
			{
				fileUri = new Uri(path);
				if (fileUri.Scheme == AssemblyScheme)
				{
					return HtmlElementUtils.ExtractResource(fileUri);
				}
			}
			catch
			{
			}

			return GetPathForSystemFile(path);
		}

		/// <summary>
		///     Gets the path for system file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The value.</returns>
		private string GetPathForSystemFile(string path)
		{
			return Path.GetFullPath(path);
		}
	}
}