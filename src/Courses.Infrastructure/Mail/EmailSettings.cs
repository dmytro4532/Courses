namespace Courses.Infrastructure.Mail;

public class EmailSettings
{
    public const string SectionName = nameof(EmailSettings);

    public string Host { get; set; }

    public int Port { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool EnableSsl { get; set; }
}
