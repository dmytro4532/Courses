using Courses.Domain.Common;
using Courses.Domain.Users.DomainEvents;

namespace Courses.Domain.Users;

public class User : AggregateRoot
{
    public Username Username { get; private set; }

    public Email Email { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool Deleted { get; private set; }

    private User() { }

    private User(Guid id, Username username, Email email)
        : base(id)
    {
        Username = username;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserEmailChangedDomainEvent(Guid.NewGuid(), Id, Email));
    }

    public void ConfirmEmail()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(Username username)
    {
        Username = username;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        Deleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public static User Create(Guid id, Username username, Email email)
    {
        var user = new User(id, username, email);

        user.AddDomainEvent(new UserCreatedDomainEvent(Guid.NewGuid(), user.Id, user.Email));

        return user;
    }
}
