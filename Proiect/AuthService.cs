namespace Proiect;

//Serviciu care retine cine e logat
public class AuthService
{
    public User CurrentUser { get; set; }
    public bool IsLoggedIn => CurrentUser != null;
    public void Logout() => CurrentUser = null;
}