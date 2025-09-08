using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Infrastructure.Services
{
	public class SmtpEmailSender : IEmailSender
	{
		private readonly IConfiguration _config;

		public SmtpEmailSender(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendEmailAsync(string to, string subject, string body)
		{
			var host = _config["EmailSettings:SmtpServer"];
			var port = int.Parse(_config["EmailSettings:Port"]);
			var username = _config["EmailSettings:UserName"];
			var password = _config["EmailSettings:Password"];
			var from = _config["EmailSettings:From"];

			using var client = new SmtpClient(host)
			{
				Port = port,
				Credentials = new NetworkCredential(from, password),
				EnableSsl = true
			};

			using var message = new MailMessage(from, to)
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			};

			await client.SendMailAsync(message);
		}
	}
}
