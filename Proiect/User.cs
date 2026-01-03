using System.Text.Json.Serialization;

namespace Proiect;

[JsonDerivedType(typeof(Admin), typeDiscriminator: "Admin")]
[JsonDerivedType(typeof(Medic), typeDiscriminator: "Medic")]
[JsonDerivedType(typeof(Pacient), typeDiscriminator: "Pacient")]
public abstract class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    protected User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
    }
}