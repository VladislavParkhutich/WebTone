using System;
using System.Configuration;
using System.Globalization;

namespace Core.SeleniumUtils.Core
{
	/// <summary>
	///     App.config settings.
	/// </summary>
	public static class Configurations
	{
		#region WebDriver properties

		/// <summary>
		///     Default timeout to wait for elements.
		/// </summary>
		public static readonly int Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"],
			CultureInfo.InvariantCulture);

		/// <summary>
		///     The maximum amount of time to wait for each command.
		/// </summary>
		public static readonly int WebDriverCommandTimeout =
			Convert.ToInt32(ConfigurationManager.AppSettings["WebDriverCommandTimeout"], CultureInfo.InvariantCulture);

		#endregion WebDriver properties

		#region Email properties

		/// <summary>
		///     Gets the mail map URL.
		/// </summary>
		/// <value>The mail map URL.</value>
		public static string MailMapUrl
		{
			get { return ConfigurationManager.AppSettings["MailImapUrl"]; }
		}

		/// <summary>
		///     Gets a value indicating whether the mail SSL.
		/// </summary>
		/// <value>The mail SSL.</value>
		public static bool MailSsl
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["MailSsl"], CultureInfo.InvariantCulture); }
		}

		/// <summary>
		///     Gets the mail port.
		/// </summary>
		/// <value>The mail port.</value>
		public static int MailPort
		{
			get { return Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"], CultureInfo.InvariantCulture); }
		}

		/// <summary>
		///     Gets the mail login.
		/// </summary>
		/// <value>The mail login.</value>
		public static string MailLogin
		{
			get { return ConfigurationManager.AppSettings["MailLogin"]; }
		}

		/// <summary>
		///     Gets the mail password.
		/// </summary>
		/// <value>The mail password.</value>
		public static string MailPassword
		{
			get { return ConfigurationManager.AppSettings["MailPassword"]; }
		}

		#endregion Email properties
	}
}