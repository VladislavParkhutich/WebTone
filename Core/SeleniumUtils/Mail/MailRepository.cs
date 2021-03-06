using System;
using System.Net;
using System.Threading;
using Core.SeleniumUtils.Core;
using MailKit.Net.Imap;

namespace Core.SeleniumUtils.Mail
{
	/// <summary>
	///     MailRepository class.
	/// </summary>
	public class MailRepository
	{
		/// <summary>
		///     Logins to server.
		/// </summary>
		/// <returns>The result.</returns>
		private EmailClient LoginToServer()
		{
			var credentials = new NetworkCredential(Configurations.MailLogin, Configurations.MailPassword);
			var uri = new Uri(Configurations.MailMapUrl);
			var emailClient = new EmailClient {Imap = new ImapClient(), Cancel = new CancellationTokenSource()};
			emailClient.Imap.Connect(uri, emailClient.Cancel.Token);
			emailClient.Imap.AuthenticationMechanisms.Remove("XOAUTH");
			emailClient.Imap.Authenticate(credentials, emailClient.Cancel.Token);
			return emailClient;
		}

		///// <summary>
		///// Gets the messages by from and subject.
		///// </summary>
		///// <param name="from">The From.</param>
		///// <param name="subject">The subject.</param>
		///// <returns>The result.</returns>
		//public IList<MailMessage> GetMessagesByFromAndSubject(string from, string subject)
		//{
		//	UniqueId[] searchIds = null;
		//	EmailClient client = null;
		//	IFolder inbox = null;
		//	var waitForMessage = new Func<bool>(() =>
		//		{
		//			client = this.LoginToServer();
		//			inbox = client.Imap.Inbox;
		//			inbox.Open(FolderAccess.ReadOnly, client.Cancel.Token);
		//			searchIds = inbox.Search(SearchQuery.And(SearchQuery.FromContains(from), SearchQuery.SubjectContains(subject)), client.Cancel.Token);
		//			var found = searchIds.Length > 0;
		//			if (!found)
		//			{
		//				client.Imap.Disconnect(true, client.Cancel.Token);
		//			}

		//			return found;
		//		});

		//	Waiter.SpinWaitEnsureSatisfied(waitForMessage, TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(5), "The message from " + from + " with subject " + subject + " not found after 3 minutes");

		//	var messages = searchIds.Select(sId => new MailMessage(inbox.GetMessage((UniqueId)sId, client.Cancel.Token)));
		//	return messages.ToList();
		//}

		/// <summary>
		///     EmailClient class.
		/// </summary>
		private class EmailClient
		{
			/// <summary>
			///     Gets or sets the imap.
			/// </summary>
			/// <value>The imap.</value>
			public ImapClient Imap { get; set; }

			/// <summary>
			///     Gets or sets the cancel.
			/// </summary>
			/// <value>The cancel.</value>
			public CancellationTokenSource Cancel { get; set; }
		}
	}
}