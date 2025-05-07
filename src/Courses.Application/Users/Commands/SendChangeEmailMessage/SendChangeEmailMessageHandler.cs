using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Users.Dto;
using Courses.Domain.User;
using MediatR;
using Shared.Results;

namespace Courses.Application.Users.Commands.SendChangeEmailMessage;

internal sealed class SendChangeEmailMessageHandler : IRequestHandler<SendChangeEmailMessageCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<User, UserResponse> _mapper;

    public SendChangeEmailMessageHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, Mapper<User, UserResponse> mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(SendChangeEmailMessageCommand request, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
