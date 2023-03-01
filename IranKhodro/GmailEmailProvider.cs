using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace IranKhodro;

public class GmailEmailProvider : IEmailManager
{
    private readonly IOptions<Settings> _options;

    public GmailEmailProvider(IOptions<Settings> options)
    {
        _options = options;
    }

    public void Send(string address, string message)
    {
        var fromAddress = new MailAddress(_options.Value.SmtpSenderEmailAddress, _options.Value.SmtpSenderDisplayName);
        var toAddress = new MailAddress(address);
        var smtp = new SmtpClient
        {
            Host = _options.Value.SmtpHost,
            Port = _options.Value.SmtpPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, _options.Value.SmtpSenderEmailPassword)
        };
        using var mailMessage = new MailMessage(fromAddress, toAddress)
        {
            Subject = _options.Value.SmtpMessageSubject,
            Body = message
        };
        smtp.Send(mailMessage);

    }
}