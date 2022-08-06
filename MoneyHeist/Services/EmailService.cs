using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MoneyHeist.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _from;
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _from = configuration["EmailConfiguration:From"];
            _smtpServer = configuration["EmailConfiguration:SmtpServer"];
            _port = Int32.Parse(configuration["EmailConfiguration:Port"]);
            _username = configuration["EmailConfiguration:Username"];
            _password = configuration["EmailConfiguration:Password"];
        }

        public void Send(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(body);

            using var smtp = new SmtpClient();
            smtp.Connect(_smtpServer, _port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_username, _password);
            smtp.Send(email);
            smtp.Disconnect(true);

        }


    }
}
