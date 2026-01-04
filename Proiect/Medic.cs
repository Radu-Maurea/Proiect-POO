namespace Proiect
{
    public class Medic : User
    {
        
        public Medic(string email, string password): base(email, password) { }
        
        public override string Rol()
        {
            return "Medic";
        }
    }
}