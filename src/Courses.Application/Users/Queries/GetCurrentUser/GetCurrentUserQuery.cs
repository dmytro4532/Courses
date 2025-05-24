using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Users.Dto;
using Shared.Results;

namespace Courses.Application.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IQuery<Result<UserResponse>>;
