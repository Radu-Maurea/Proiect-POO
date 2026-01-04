namespace Proiect;

public class Admin : User
{
    private Clinica clinica;
    public Admin(string email, string password) : base(email, password)
    {
        
    }

    public void SetClinica(Clinica clinica)
    {
        this.clinica = clinica;
    }
    public override string Rol()
    {
        return "Admin";
    }

    public bool AdaugaMedic(string email, string parola,string specialitate,string programLucru)
    {
            Medic medicNou = new Medic(email,parola);
            medicNou.SetSpecialitate(specialitate);
            medicNou.SetProgram(programLucru);
            clinica.AdaugaUtilizator(medicNou);
            return true;
    }

    public bool StergeMedic(string email)
    {
        if (clinica.ExistaEmail(email))
        {
            var user = clinica.utilizatori.FirstOrDefault(u => u.Email.Trim().ToLower() == email && u is Medic);

            if (user != null)
            {
                clinica.utilizatori.Remove(user); 
                clinica.SalvareInFisier();       
                return true;
            }
        }
        return false;
    }

    public bool ModificaMedic(string email, string specialitate, string programLucru)
    {
        var medic = clinica.utilizatori.OfType<Medic>().FirstOrDefault(m => m.Email.Trim().ToLower() == email.Trim().ToLower());
        if (medic != null)
        {
            medic.SetSpecialitate(specialitate);
            medic.SetProgram(programLucru);
            
            clinica.SalvareInFisier();
            return true;
        }

        return false;
    }
    
    
}