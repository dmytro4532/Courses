using Courses.Application.Abstractions.Services;
using Courses.Infrastructure.Auth;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Courses.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly EmailTokenService _emailTokenService;
    private readonly LinkFactory _linkFactory;

    public EmailService(IOptions<EmailSettings> emailSettings, EmailTokenService emailTokenService, LinkFactory linkFactory)
    {
        _emailSettings = emailSettings.Value;
        _emailTokenService = emailTokenService;
        _linkFactory = linkFactory;
    }

    public async Task SendConfirmaitionEmailAsync(Guid userId, string email)
    {
        var token = await _emailTokenService.GenerateEmailConfirmationTokenAsync(email);
        var escapedToken = Uri.EscapeDataString(token);
        var link = _linkFactory.CreateEmailConfirmationLink(userId, escapedToken);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Courses", _emailSettings.UserName));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Confirm your email";
        message.Body = new TextPart("plain")
        {
            Text = $"Welcome to the Courses platform! Please confirm your email by clicking the link {link}."
        };

        await SendEmailAsync(message);
    }

    private async Task SendEmailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        await client.ConnectAsync(
            _emailSettings.Host,
            _emailSettings.Port,
            _emailSettings.EnableSsl);

        await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
