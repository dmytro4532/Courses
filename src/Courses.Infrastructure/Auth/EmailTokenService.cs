using Courses.Application.Users.Identity;
using Microsoft.AspNetCore.Identity;

namespace Courses.Infrastructure.Auth;

public class EmailTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public EmailTokenService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new InvalidOperationException("User not found.");

        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<string> GenerateChangeEmailTokenAsync(string email, string newEmail)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new InvalidOperationException("User not found.");

        return await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
    }
}
