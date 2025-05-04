using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.Common.Exceptions;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUserRepository userRepository, IIdentityService identityService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return new NotFoundError("User.NotFound", "User not found.");
        }

        var identityUser = await _identityService.GetByEmailAsync(user.Email)
            ?? throw new InconsistentDataException($"Identity with email '{user.Email}' user not found");

        user.Delete();
        _userRepository.Remove(user);
        await _identityService.DeleteAsync(identityUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
