﻿namespace SchoolEats.Services.Messaging
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
	}
}
