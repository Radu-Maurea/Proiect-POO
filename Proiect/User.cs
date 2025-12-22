namespace Proiect;

public abstract class User
{
    public Guid Id { get; }
    public string Email { get; }
    public string Password { get; }

    public User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
    }
}

