using MediatR;

namespace Courses.Application.Abstractions.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>;