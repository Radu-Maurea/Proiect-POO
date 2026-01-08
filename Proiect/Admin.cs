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
            medicNou.ServiciiOferite.Add(serviciu);
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
            if (clinica.ExistaEmail(email))
            {
                var user = clinica.utilizatori.FirstOrDefault(u => u.Email.Trim().ToLower() == email.Trim().ToLower() && u is Medic);

                if (user != null)
                {
                    clinica.utilizatori.Remove(user);
                    clinica.SalvareInFisier();
                    _logger.LogInfo($"Admin {Email} a sters medicul {email}");
                    return true;
                }
            }
            return false;
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
            var medic = clinica.utilizatori.OfType<Medic>().FirstOrDefault(m => m.Email.Trim().ToLower() == email.Trim().ToLower());
            if (medic != null)
            {
                medic.SetSpecialitate(specialitate);
                medic.SetProgram(programLucru);
                clinica.SalvareInFisier();
                _logger.LogInfo($"Admin {Email} a modificat datele medicului {email}");
                return true;
            }
            return false;
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
            clinica.servicii.Add(nou);
            clinica.SalvareServiciiInFisier();
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
            var medic = clinica.Medici.FirstOrDefault(m => m.Email == emailMedic);
            var serviciu = clinica.servicii.FirstOrDefault(s => s.Denumire == denumireServiciu);

            if (medic != null && serviciu != null)
            {
                if (!medic.ServiciiOferite.Any(s => s.Denumire == denumireServiciu))
                {
                    medic.ServiciiOferite.Add(serviciu);
                    clinica.SalvareInFisier();
                    _logger.LogInfo($"Admin {Email} a asociat serviciul {denumireServiciu} medicului {emailMedic}");
                    return true;
                }
            }
            return false;
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
            clinica.IncarcaProgramariDinFisier();
            string[] parti = programare.Split(new[] { " || " }, StringSplitOptions.None);
            if (parti.Length == 2)
            {
                string ora = parti[0];
                string emailMedic = parti[1];
                var deSters = clinica.programari.FirstOrDefault(p =>
                    p.DataOra.Equals(ora) &&
                    p.Medic.Email.Equals(emailMedic) &&
                    p.Vazut == false);

                if (deSters != null)
                {
                    clinica.programari.Remove(deSters);
                    clinica.SalvareProgramariInFisier();
                    _logger.LogInfo($"Admin {Email} a sters programarea de la ora {ora} a medicului {emailMedic}");
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Eroare la stergerea programarii: {ex.Message}");
            return false;
        }
    }

    // Metodele de vizualizare raman neschimbate (fara logare) conform solicitarii tale
    public string VeziProgramari()
    {
        string sb = "";
        clinica.IncarcaProgramariDinFisier();
        foreach (var programare in clinica.programari)
        {
            if (!programare.Vazut)
                sb += programare.ToString() + Environment.NewLine;
        }
        return sb;
    }

    public string VeziProgramariDiagnosticate()
    {
        string sb = "";
        clinica.IncarcaProgramariDinFisier();
        foreach (var programare in clinica.programari)
        {
            if (programare.Vazut)
                sb += programare.ToString() + " Diagnostic: " + programare.Diagnostic + Environment.NewLine;
        }
        return sb;
    }

    public int NumarProgramari() => clinica.programari.Count;
    public int NumarMedici() => clinica.Medici.Count;
    public int NumarServicii() => clinica.servicii.Count;
    public int NumarProgramariDiagnosticate() => clinica.programari.Count(p => p.Vazut);
    public int NumarProgramariInProgres() => clinica.programari.Count(p => !p.Vazut);
    public int NumarConturi() => clinica.utilizatori.Count;
}