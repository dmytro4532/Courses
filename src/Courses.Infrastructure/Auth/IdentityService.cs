using Courses.Application.Abstractions.Services;
using Courses.Application.Users.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Infrastructure.Auth;

public class IdentityService :  IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task CreateAsync(
        ApplicationUser identityUser,
        string password)
    {
        ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        var result = await _userManager.CreateAsync(identityUser, password);

        HandleIdentityResult(result, "Failed to create identity user.");
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser identityUser)
    {
        ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));

        return await _userManager.GetRolesAsync(identityUser);
    }


    public async Task<Result> LoginAsync(
        ApplicationUser identityUser,
        string password)
    {
        ArgumentNullException.ThrowIfNull(identityUser);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var result = await _signInManager.PasswordSignInAsync(
            identityUser,
            password,
            isPersistent: true,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return new Error("User.WrongEmailOrPassword", "Wrong email or password.");
        }

        return Result.Success();
    }

    public async Task<Result> ConfirmEmailAsync(
        ApplicationUser identityUser,
        string token)
    {
        var result = await _userManager.ConfirmEmailAsync(
            identityUser, token);

        if (!result.Succeeded)
        {
            return new Error("User.ConfirmEmailFail", "Failed to confirm email");
        }

        return Result.Success();
    }

    public async Task<Result> ChangeEmailAsync(
        ApplicationUser identityUser,
        string newEmail,
        string token)
    {
        var identityResult = await _userManager.ChangeEmailAsync(
            user: identityUser,
            newEmail,
            token);

        if (!identityResult.Succeeded)
        {
            return new Error("User.ChangeEmailFail", "Failed to change email");
        }

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(
        ApplicationUser identityUser,
        string newPassword,
        string token)
    {
        var identityResult = await _userManager.ResetPasswordAsync(
            identityUser,
            token,
            newPassword);

        if (!identityResult.Succeeded)
        {
            return new Error("User.PasswordResetFail", "Failed to reset password");
        }

        return Result.Success();
    }

    public async Task AddToRoleAsync(
        ApplicationUser identityUser,
        string roleName)
    {
        ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        var result = await _userManager.AddToRoleAsync(identityUser, roleName);

        HandleIdentityResult(result, "Failed to add identity user to role.");
    }

    public async Task RemoveFromRoleAsync(
        ApplicationUser identityUser,
        string roleName)
    {
        ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        var result = await _userManager.RemoveFromRoleAsync(identityUser, roleName);

        HandleIdentityResult(result, "Failed to remove identity user from role.");
    }

    public async Task DeleteAsync(ApplicationUser identityUser)
    {
        ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));

        var result = await _userManager.DeleteAsync(identityUser);

        HandleIdentityResult(result, "Failed to delete identity user.");
    }

    private void HandleIdentityResult(IdentityResult result, string errorMessage)
    {
        if (result.Succeeded)
        {
            return;
        }

        var errors = result.Errors.Select(x => x.Description);
        var errorsString = string.Join(Environment.NewLine, errors);

        throw new InvalidOperationException($"{errorMessage}\r\nErrors: {errorsString}");
    }
}
