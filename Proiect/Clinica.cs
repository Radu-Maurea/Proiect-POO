using System.Collections.Generic;
using System.Linq;

namespace Proiect
{
    public class Clinica
    {
        private List<User> utilizatori = new List<User>();
        public IReadOnlyList<User> UtilizatoriReadOnly => utilizatori.AsReadOnly();
        public void AdaugaUtilizator(User user)
        {
            utilizatori.Add(user);
        }

        public bool ExistaEmail(string email)
        {
            email = email.Trim().ToLower();

            foreach (var user in utilizatori)
            {
                if (user.Email.Trim().ToLower() == email)
                    return true;
            }
            return false;
        }

    }
}