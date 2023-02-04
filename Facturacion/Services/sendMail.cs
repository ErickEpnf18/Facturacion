using Facturacion.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Facturacion.Services
{
    public class EmailServices : IEmailSender
    {
        private readonly IConfiguration configuration;

        private SmtpClient Cliente { get; }
        private EmailSenderOptions Options { get; }
        public EmailServices(IConfiguration configuration)
        {
            this.configuration = configuration;
            Cliente = new SmtpClient()
            {
                Host = configuration.GetSection("EmailSenderOptions:Host").Value,
                Port = int.Parse(configuration.GetSection("EmailSenderOptions:Port").Value),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(configuration.GetSection("EmailSenderOptions:Email").Value, configuration.GetSection("EmailSenderOptions:Password").Value),
            };

        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var correo = new MailMessage(from: configuration.GetSection("EmailSenderOptions:Email").Value, to: email, subject: subject, body: htmlMessage);
            correo.IsBodyHtml = true;

            return Cliente.SendMailAsync(correo);
        }
      
    }

}
