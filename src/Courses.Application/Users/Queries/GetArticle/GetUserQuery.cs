using Courses.Application.Abstractions.Messaging;
using Courses.Application.Users.Dto;
using Shared.Results;

namespace Courses.Application.Users.Queries.GetArticle;

public record GetUserQuery(Guid UserId) : IQuery<Result<UserResponse>>;
