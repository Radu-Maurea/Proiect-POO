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

    public bool AdaugaMedic(string email,string nume ,string parola,string specialitate,string programLucru)
    {
            Medic medicNou = new Medic(email,parola);
            medicNou.SetSpecialitate(specialitate);
            medicNou.SetProgram(programLucru);
            medicNou.SetNume(nume);
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
   
    public bool AdaugaServiciu(string denumire, decimal pret, int durata)
    {
        try
        {
            ServiciuMedical nou = new ServiciuMedical(denumire, pret, durata);
            clinica.servicii.Add(nou);
            clinica.SalvareServiciiInFisier();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool AsociazaServiciuMedic(string emailMedic, string denumireServiciu)
    {
        var medic = clinica.Medici.FirstOrDefault(m => m.Email == emailMedic);
        var serviciu = clinica.servicii.FirstOrDefault(s => s.Denumire == denumireServiciu);

        if (medic != null && serviciu != null)
        {
            if (!medic.ServiciiOferite.Any(s => s.Denumire == denumireServiciu))
            {
                medic.ServiciiOferite.Add(serviciu);
                clinica.SalvareInFisier(); 
                return true;
            }
        }
        return false;
    }
    
}