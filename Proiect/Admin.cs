namespace Proiect;

public class Admin : User
{
    private Clinica clinica;
    private ILogger _logger;
    public Admin(string email, string password) : base(email, password)
    {
    }

    public void SetClinica(Clinica clinica, ILogger logger)
    {
        this.clinica = clinica;
        _logger = logger;
    }

    public override string Rol()
    {
        return "Admin";
    }

    public bool AdaugaMedic(string email, string nume, string parola, string specialitate, string programLucru, ServiciuMedical serviciu)
    {
        try
        {
            Medic medicNou = new Medic(email, parola);
            medicNou.SetSpecialitate(specialitate);
            medicNou.SetProgram(programLucru);
            medicNou.SetNume(nume);
            medicNou.AdaugaServiciu(serviciu);
            clinica.AdaugaUtilizator(medicNou);
            _logger.LogInfo($"Admin {Email} a adaugat un nou medic {medicNou.Nume}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la adaugarea medicului {ex.Message}");
            return false;
        }
    }

    public bool StergeMedic(string email)
    {
        try
        {
            clinica.StergeUtilizator(email);
            var programare = clinica.ProgramariReadOnly.FirstOrDefault(p => p.Medic.Email == email);
            if (programare != null) clinica.StergeProg(programare.DataOra + " || " + email);
            _logger.LogInfo($"Admin {Email} a sters medicul {email}");
            return true;
                    
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la stergerea medicului: {ex.Message}");
            return false;
        }
    }

    public bool ModificaMedic(string email, string specialitate, string programLucru)
    {
        try
        {
            clinica.ModificaMedic(email, specialitate, programLucru);
            _logger.LogInfo($"Admin {Email} a modificat datele medicului {email}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la modificarea medicului: {ex.Message}");
            return false;
        }
    }

    public bool AdaugaServiciu(string denumire, decimal pret, int durata)
    {
        try
        {
            ServiciuMedical nou = new ServiciuMedical(denumire, pret, durata);
            clinica.AdaugaServiciuMedical(nou);
            _logger.LogInfo($"Admin {Email} a adaugat serviciul {denumire}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la adaugarea serviciului: {ex.Message}");
            return false;
        }
    }

    public bool AsociazaServiciuMedic(string emailMedic, string denumireServiciu)
    {
        try
        {
            clinica.AsociazaServ(emailMedic, denumireServiciu);
            _logger.LogInfo($"Admin {Email} a asociat serviciul {denumireServiciu} medicului {emailMedic}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la asocierea serviciului: {ex.Message}");
            return false;
        }
    }

    public bool StergeProgramare(string programare)
    {
        try
        {
            clinica.StergeProg(programare);
            var ora = programare.Split(new[] { " || " }, StringSplitOptions.None)[0];
            var emailMedic = programare.Split(new[] { " || " }, StringSplitOptions.None)[1];
            _logger.LogInfo($"Admin {Email} a sters programarea de la ora {ora} a medicului {emailMedic}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la stergerea programarii: {ex.Message}");
            return false;
        }
    }

    
    public string VeziProgramari()
    {
        string sb = "";
        clinica.IncarcaDate();
        foreach (var programare in clinica.ProgramariReadOnly)
        {
            if (!programare.Vazut)
                sb += programare.ToString() + Environment.NewLine;
        }
        return sb;
    }

    public string VeziProgramariDiagnosticate()
    {
        string sb = "";
        clinica.IncarcaDate();
        foreach (var programare in clinica.ProgramariReadOnly)
        {
            if (programare.Vazut)
                sb += programare.ToString() + " Diagnostic: " + programare.Diagnostic + Environment.NewLine;
        }
        return sb;
    }

    public int NumarProgramari() => clinica.ProgramariReadOnly.Count;
    public int NumarMedici() => clinica.Medici.Count;
    public int NumarServicii() => clinica.ServiciiReadOnly.Count;
    public int NumarProgramariDiagnosticate() => clinica.ProgramariReadOnly.Count(p => p.Vazut);
    public int NumarProgramariInProgres() => clinica.ProgramariReadOnly.Count(p => !p.Vazut);
    public int NumarConturi() => clinica.UtilizatoriReadOnly.Count;
}