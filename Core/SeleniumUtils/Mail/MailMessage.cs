using System.Collections.Generic;
using System.Linq;
using MimeKit;

namespace Core.SeleniumUtils.Mail
{
	/// <summary>
	///     MailMessage class.
	/// </summary>
	public class MailMessage
	{
		private readonly MimeMessage mimeMessage;

		/// <summary>
		///     Initializes a new instance of the <see cref="MailMessage" /> class.
		/// </summary>
		/// <param name="mimeMessage">The MIME message.</param>
		public MailMessage(MimeMessage mimeMessage)
		{
			this.mimeMessage = mimeMessage;
		}

		/// <summary>
		///     Gets the attachments.
		/// </summary>
		/// <value>The attachments.</value>
		public IEnumerable<MimeEntity> Attachments
		{
			get { return mimeMessage.Attachments; }
		}

		/// <summary>
		///     Gets the subject.
		/// </summary>
		/// <value>The subject.</value>
		public string Subject
		{
			get { return mimeMessage.Subject; }
		}

		/// <summary>
		///     Gets the body as text.
		/// </summary>
		/// <value>The body as text.</value>
		public string BodyAsText
		{
			get
			{
				var firstOrDefault = mimeMessage.BodyParts.OfType<TextPart>().FirstOrDefault();
				if (firstOrDefault != null)
				{
					return firstOrDefault.Text;
				}

				return null;
			}
		}

		///// <summary>
		///// Retrieves the attachments as byte arrays.
		///// </summary>
		///// <returns>The result.</returns>

		//public IList<byte[]> RetrieveAttachmentsAsByteArrays()
		//{
		//	var attachmentsList = new List<byte[]>();

		//	if (!this.mimeMessage.Attachments.Any())
		//	{
		//		return attachmentsList;
		//	}

		//	using (var ms = new MemoryStream())
		//	{
		//		this.mimeMessage.Attachments.ToList().ForEach(a =>
		//			{
		//				a.ContentObject.Open().CopyTo(ms);
		//				attachmentsList.Add(ms.ToArray());
		//			});
		//	}

		//	return attachmentsList;
		//}
	}
}