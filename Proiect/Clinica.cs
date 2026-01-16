using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Proiect;

public class Clinica
{
    private static string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    
    private const string FileName = @"F:\info\an_2\Proiect POO\Proiect\Proiect\utilizatori.json";
    private const string ServiciiFileName = @"F:\info\an_2\Proiect POO\Proiect\Proiect\servicii.json";
    private const string ProgramariFileName = @"F:\info\an_2\Proiect POO\Proiect\Proiect\programari.json";

    private readonly ILogger _logger;

    private List<User> utilizatori = new List<User>();
    private List<ServiciuMedical> servicii = new List<ServiciuMedical>();
    private List<Programare> programari = new List<Programare>();
    public IReadOnlyList<User> UtilizatoriReadOnly => utilizatori.AsReadOnly();
    public IReadOnlyList<ServiciuMedical> ServiciiReadOnly => servicii.AsReadOnly();
    public IReadOnlyList<Programare> ProgramariReadOnly => programari.AsReadOnly();

    public List<Medic> Medici => utilizatori.Where(u => u is Medic).Cast<Medic>().ToList();
    public List<Pacient> Pacienti => utilizatori.Where(u => u is Pacient).Cast<Pacient>().ToList();


    public Clinica(ILogger logger)
    {
        _logger = logger;
        IncarcaDate();
    }

    public void SalvareInFisier()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(utilizatori, options);
            File.WriteAllText(FileName, jsonString);
            _logger.LogInfo("Utilizatori salvati in fisier");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            _logger.LogError("Eroare la salvare utilizatori: " + ex.Message);
        }

        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(servicii, options);
            File.WriteAllText(ServiciiFileName, jsonString);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Eroare la salvarea serviciilor: " + ex.Message);
        }

        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(programari, options);
            File.WriteAllText(ProgramariFileName, json);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Eroare salvare programare: " + ex.Message);
        }
    }

    public void IncarcaDate()
    {
        if (File.Exists(ProgramariFileName))
        {
            try
            {
                string jsonString = File.ReadAllText(ProgramariFileName);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                programari.Clear(); 
                programari = JsonSerializer.Deserialize<List<Programare>>(jsonString, options) ?? new List<Programare>();
            }
            catch (Exception ex) { MessageBox.Show("Eroare la încărcare programari: " + ex.Message); }
        }

        if (File.Exists(FileName))
        {
            try
            {
                string jsonString = File.ReadAllText(FileName);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                utilizatori.Clear(); 
                utilizatori = JsonSerializer.Deserialize<List<User>>(jsonString, options) ?? new List<User>();
            }
            catch (Exception ex) { MessageBox.Show("Eroare la încărcare utilizatori: " + ex.Message); }
        }

        if (File.Exists(ServiciiFileName))
        {
            try
            {
                string jsonString = File.ReadAllText(ServiciiFileName);
                servicii.Clear(); 
                servicii = JsonSerializer.Deserialize<List<ServiciuMedical>>(jsonString) ?? new List<ServiciuMedical>();
            }
            catch { servicii = new List<ServiciuMedical>(); }
        }
    }

    public bool ExistaEmail(string email)
    {
        email = email.Trim().ToLower();
        return utilizatori.Any(user => user.Email.Trim().ToLower() == email);
    }

    public bool VerificareLogin(string email, string password)
    {
        _logger.LogInfo($"Login incercat: {email}");
        email = email.Trim().ToLower();
        foreach (var user in utilizatori)
        {
            if (user.Email.Trim().ToLower() == email && user.Password == password)
            {
                _logger.LogInfo("Login reusit");
                return true;
            }
        }
        _logger.LogInfo("Login incorect");
        return false;
    }

    public User GetUserLogin(string email, string password)
    {
        email = email.Trim().ToLower();
        return utilizatori.FirstOrDefault(user => user.Email.Trim().ToLower() == email && user.Password == password);
    }

    public void AdaugaUtilizator(User user)
    {
        utilizatori.Add(user);
        SalvareInFisier();
    }

    public void StergeUtilizator(string email)
    {
        var user = utilizatori.FirstOrDefault(u => u.Email.Trim().ToLower() == email.Trim().ToLower());
        if (user != null)
        {
            utilizatori.Remove(user);
            SalvareInFisier();
        }
    }

    public void ModificaMedic(string email, string specialitate, string programLucru)
    {
        Medic medic = Medici.FirstOrDefault(m => m.Email.Trim().ToLower() == email.Trim().ToLower());
        if (medic != null)
        {
            medic.SetSpecialitate(specialitate);
            medic.SetProgram(programLucru);
            SalvareInFisier();
        }
    }

    public void AdaugaServiciuMedical(ServiciuMedical serviciuMedical)
    {
        servicii.Add(serviciuMedical);
        SalvareInFisier();
    }

    public void AsociazaServ(string emailMedic, string denumireServiciu)
    {
        var medic = Medici.FirstOrDefault(m => m.Email == emailMedic);
        var serviciu = servicii.FirstOrDefault(s => s.Denumire == denumireServiciu);
        if (medic != null && serviciu != null)
        {
            if (!medic.ServiciiOferiteReadOnly.Any(s => s.Denumire == denumireServiciu))
            {
                medic.AdaugaServiciu(serviciu);
                SalvareInFisier();
            }
        }
    }

    public void StergeProg(string progrmare)
    {
        IncarcaDate();
        string[] parti = progrmare.Split(new[] { " || " }, StringSplitOptions.None);
        if (parti.Length == 2)
        {
            string ora = parti[0];
            string emailMedic = parti[1];
            var deSters = programari.FirstOrDefault(p =>
                p.DataOra.Equals(ora) &&
                p.Medic.Email.Equals(emailMedic) &&
                p.Vazut == false);
            if (deSters != null)
            {
                programari.Remove(deSters);
                SalvareInFisier();
            }
        }
    }

    public void AdaugaProgramare(Programare p)
    {
        if (!EsteIntervalDisponibil(p.Medic.Email, p.Pacient.Email, p.DataOra))
        {
            throw new InvalidOperationException("Medicul sau pacientul nu este disponibil la ora indicata.");
        }
        _logger.LogInfo($"Programare creata pentru {p.Pacient.Email}");
        programari.Add(p);
        SalvareInFisier();
    }

    public void DiagnosticareProgramare(string emailMedic, string emailPacient, string dataOra, string diagnostic)
    {
        var programare = programari.FirstOrDefault(p =>
            p.Medic.Email.Equals(emailMedic, StringComparison.OrdinalIgnoreCase) &&
            p.Pacient.Email.Equals(emailPacient, StringComparison.OrdinalIgnoreCase) &&
            p.DataOra.Equals(dataOra));

        if (programare == null) throw new InvalidOperationException("Programarea nu a fost găsită.");
        if (programare.Vazut) throw new InvalidOperationException("Programarea a fost deja diagnosticată.");

        programare.SetDiagnostic(diagnostic);
        programare.Vazut = true;
        _logger?.LogInfo($"[DIAGNOSTIC] Medicul {emailMedic} a diagnosticat pacientul {emailPacient} pentru {dataOra}");
        SalvareInFisier();
    }

    public bool EsteIntervalDisponibil(string emailMedic, string emailPacient, string dataOra)
    {
        //Verificare daca medicul este ocupat la ora indicata
        bool medicOcupat = programari.Any(p => p.Medic.Email == emailMedic && p.DataOra == dataOra && !p.Vazut);
        
        //Verificare daca pacientul este ocupat la ora indicata
        bool pacientOcupat = programari.Any(p => p.Pacient.Email == emailPacient && p.DataOra == dataOra && !p.Vazut);
        
        return !medicOcupat && !pacientOcupat;
    }
}