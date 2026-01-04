namespace Proiect;

public class Admin : User
{
    public Admin(string email, string password) : base(email, password)
    {
    }

    public override string Rol()
    {
        return "Admin";
    }
}