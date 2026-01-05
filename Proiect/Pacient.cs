namespace Proiect;

public class Pacient : User
{
    private Clinica clinica;
    public Pacient(string email, string password) : base(email, password){}

    public void SetClinica(Clinica clinica)
    {
        this.clinica = clinica;
    }
    public override string Rol()
    {
        return "Pacient";
    }

    public string CautareDoctorNume(string nume)
    {
        nume = nume.Trim().ToLower();
        string sb = "";
    
        Console.WriteLine($"DEBUG: Cautare pornita. Medici in lista: {clinica.Medici.Count}");

        foreach (var medic in clinica.Medici)
        {
            Console.WriteLine($"DEBUG: Verific medicul: '{medic.Nume}'");

            if (medic.Nume != null && medic.Nume.ToLower().Contains(nume))
            {
                sb += medic.ToString() + Environment.NewLine + "-----------------------" + Environment.NewLine;
            }
        }

        if (string.IsNullOrEmpty(sb))
        {
            return "Nu s-a gasit niciun medic cu acest nume!";
        }
        return sb;
    }
    
    public string CautareDoctorSpecializare(string specializare)
    {
        string sb = "";
        foreach (var medic in clinica.Medici)
        {
            if (medic.Specialitate == specializare)
            {
                sb += medic.ToString() + Environment.NewLine + "-----------------------" + Environment.NewLine;
            }
        }

        if (string.IsNullOrEmpty(sb))
        {
            return $"Nu s-au găsit medici pentru specializarea: {specializare}";
        }
        return sb;
    }
}