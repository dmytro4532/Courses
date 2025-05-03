using Courses.Domain.Common;
using Courses.Domain.User.DomainEvents;

namespace Courses.Domain.User;

public class User : AggregateRoot
{
    public Username Username { get; private set; }

    public Email Email { get; private set; }

    public bool EmailConfirmed { get; private set; }

    public string PasswordHash { get; private set; }

    public Role Role { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool Deleted { get; private set; }

    private User() { }

    private User(Guid id, Username username, Email email, string passwordHash, Role role)
        : base(id)
    {
        Username = username;
        Email = email;
        EmailConfirmed = false;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
        EmailConfirmed = false;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserEmailChangedDomainEvent(Guid.NewGuid(), Id, Email));
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
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

    public static User Create(Guid id, Username username, Email email, string passwordHash, Role role)
    {
        var user = new User(id, username, email, passwordHash, role);

        user.AddDomainEvent(new UserCreatedDomainEvent(Guid.NewGuid(), user.Id, user.Email));

        return user;
    }
}
