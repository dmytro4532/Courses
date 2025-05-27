using Microsoft.Extensions.Options;

namespace Courses.Infrastructure.Mail;

public class LinkFactory
{
    private readonly LinkSettings _linkSettings;

    public LinkFactory(IOptions<LinkSettings> linkSettings)
    {
        _linkSettings = linkSettings.Value;
    }

    public string CreateEmailConfirmationLink(Guid userId, string token)
    {
        return $"{_linkSettings.BaseUrl}/" +
            $"{_linkSettings.ConfirmEmailEndpoint}?" +
            $"{_linkSettings.UserIdParam}={userId}&" +
            $"{_linkSettings.TokenParam}={token}";
    }
}
