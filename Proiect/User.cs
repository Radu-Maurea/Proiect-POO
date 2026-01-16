using System.Text.Json.Serialization;

namespace Proiect;

[JsonDerivedType(typeof(Admin), typeDiscriminator: "Admin")]
[JsonDerivedType(typeof(Medic), typeDiscriminator: "Medic")]
[JsonDerivedType(typeof(Pacient), typeDiscriminator: "Pacient")]

public abstract class User
{
    [JsonInclude]
    public Guid Id { get; protected set; }
    [JsonInclude]
    public string Email { get; protected set; }
    [JsonInclude]
    public string Password { get; protected set; }

    protected User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
    }
    public abstract string Rol();
    public override string ToString()
    {
        return $"Email: {Email}, Password: {Password}";
    }
}