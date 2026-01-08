namespace Proiect
{
    public class Medic : User
    {
        public string Specialitate { get; set; }
        public Clinica clinica;
        private ILogger _logger; 
        public string ProgramLucru { get; set; }
        public string Nume { get; set; }
        public List<ServiciuMedical> ServiciiOferite { get; set; } = new List<ServiciuMedical>();
        public Medic(string email, string password) : base(email, password) { }

        public override string Rol() => "Medic";

        public override string ToString()
        {
            return $"Dr. {Nume} | Specializare: {Specialitate} | Program: {ProgramLucru} | Contact: {Email}";
        }

        public void SetSpecialitate(string s) => Specialitate = s;
        public void SetProgram(string p) => ProgramLucru = p;
        public void SetNume(string n) => Nume = n;
        public void SetClinica(Clinica clinica) => this.clinica = clinica;
        
        // Metoda noua pentru a injecta logger-ul
        public void SetLogger(ILogger logger) => this._logger = logger;

        public void GiveDiagnostic(string selectieCombo, string diag)
        {
            try
            {
                string[] parti = selectieCombo.Split(new[] { " - " }, StringSplitOptions.None);
                if (parti.Length == 2)
                {
                    string emailPacient = parti[0]; 
                    string ora = parti[1];

                    foreach (var programare in clinica.programari)
                    {
                        if (programare.Pacient.Email.Equals(emailPacient) && 
                            programare.DataOra.Equals(ora) && 
                            programare.Medic.Email.Equals(this.Email))
                        {
                            programare.SetDiagnostic(diag);
                            programare.Vazut = true;
                            _logger?.LogInfo($"[DIAGNOSTIC] Medicul {Nume} ({Email}) a diagnosticat pacientul {emailPacient} pentru programarea din {ora}.");
                            break; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"[EROARE DIAGNOSTIC] Eșec la salvarea diagnosticului de către medicul {Email}: {ex.Message}");
            }
        }
    }
    
    // Clasa ServiciuMedical ramane neschimbata
    public class ServiciuMedical
    {
        public string Denumire { get; set; }
        public decimal Pret { get; set; }
        public int DurataMinute { get; set; }

        public ServiciuMedical(string denumire, decimal pret, int durataMinute)
        {
            Denumire = denumire;
            Pret = pret;
            DurataMinute = durataMinute;
        }

        public override string ToString()
        {
            return $"{Denumire} | {Pret} lei | {DurataMinute} min";
        }
    }
}