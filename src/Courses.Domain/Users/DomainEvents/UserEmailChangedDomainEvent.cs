﻿using Courses.Domain.Common.Events;

namespace Courses.Domain.Users.DomainEvents;

public record UserEmailChangedDomainEvent(Guid Id, Guid UserId, string Email) : DomainEvent(Id);
