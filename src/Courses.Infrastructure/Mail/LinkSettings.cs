namespace Courses.Infrastructure.Mail;

public class LinkSettings
{
    public const string SectionName = nameof(LinkSettings);

    public string BaseUrl { get; set; }

    public string ConfirmEmailEndpoint { get; set; }

    public string UserIdParam { get; set; }

    public string EmailParam { get; set; }

    public string TokenParam { get; set; }
}
