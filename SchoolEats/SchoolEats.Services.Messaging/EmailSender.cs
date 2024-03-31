namespace SchoolEats.Services.Messaging
{
	using System.Net;
	using System.Net.Mail;
	using static Common.GeneralApplicationConstants;
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SmtpClient(SmtpHost, SmtpPort)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(SmtpMail, SmtpPassword),
			};

			return client.SendMailAsync(
				new MailMessage(from: EmailFrom,
					to: email,
					subject,
					message));
		}
	}
}
