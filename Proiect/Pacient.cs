using System.Text.Json.Serialization;
namespace Proiect;

public class Pacient : User
{
    private Clinica clinica;
    private ILogger _logger;
    public Pacient(string email, string password) : base(email, password){}
    [JsonInclude] public string Nume { get; private set; }
    
    public void SetClinica(Clinica clinica) => this.clinica = clinica;
    
    public void SetLogger(ILogger logger) => this._logger = logger;

    public void SetNume(string nume) => Nume = nume;
    
    public override string Rol()
    {
        return "Pacient";
    }
    
    public string CautareDoctorNume(string nume)
    {
        nume = nume.Trim().ToLower();
        string sb = "";
    
        foreach (var medic in clinica.Medici)
        {
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

    public bool CreereProgramare(Medic medic, ServiciuMedical serviciu, string ora)
    {
        try
        {
            Programare programareNoua = new Programare(medic,this,serviciu,ora);
            clinica.AdaugaProgramare(programareNoua);
            _logger?.LogInfo($"[PROGRAMARE] Pacientul {Email} a creat o programare la {medic.Nume} pe data/ora {ora}.");
            return true;
        }
        catch(Exception ex) 
        { 
            _logger?.LogError($"[EROARE PROGRAMARE] Eșec la crearea programării pentru {Email}: {ex.Message}");
            throw;
        }
    }
    

    public string VeziProgramari(string email)
    {
        clinica.IncarcaDate();
        string sb = "";
        foreach (var programare in clinica.ProgramariReadOnly)
        {
            if (programare.Pacient.Email == email && programare.Vazut == false)
                sb += programare.ToString() + Environment.NewLine;
        }
        return sb;
    }

    public bool StergeProgramare(string programare)
    {
        try 
        {
            clinica.StergeProg(programare);
            var ora = programare.Split(new[] { " || " }, StringSplitOptions.None)[0];
            var emailMedic = programare.Split(new[] { " || " }, StringSplitOptions.None)[1];
            _logger?.LogInfo($"[ANULARE] Pacientul {Email} a anulat programarea de la {ora} cu medicul {emailMedic}.");
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError($"[EROARE ANULARE] Pacientul {Email} nu a putut anula programarea: {ex.Message}");
            return false;
        }
    }
}