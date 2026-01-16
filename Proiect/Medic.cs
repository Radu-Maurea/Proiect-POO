using System.Text.Json.Serialization;
namespace Proiect
{
    public class Medic : User
    {
        [JsonInclude]
        public string Specialitate { get; private set; }
        public Clinica clinica;
        private ILogger _logger; 
        [JsonInclude]
        public string ProgramLucru { get; private set; }
        [JsonInclude]
        public string Nume { get; private set; }
        [JsonInclude]
        [JsonPropertyName("ServiciiOferite")]
        private List<ServiciuMedical> ServiciiOferite { get; set; } = new List<ServiciuMedical>();
        public IReadOnlyList<ServiciuMedical> ServiciiOferiteReadOnly => ServiciiOferite.AsReadOnly();
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
        public void SetLogger(ILogger logger) => this._logger = logger;

        public void GiveDiagnostic(string selectieCombo, string diagnostic)
        {
            if (clinica == null)
                throw new InvalidOperationException("Medicul nu este asociat unei clinici.");

            string[] parti = selectieCombo.Split(new[] { " - " }, StringSplitOptions.None);
            if (parti.Length != 2)
                return;

            string emailPacient = parti[0];
            string ora = parti[1];

            clinica.DiagnosticareProgramare(
                this.Email,
                emailPacient,
                ora,
                diagnostic
            );
        }

        public void AdaugaServiciu(ServiciuMedical serviciuMedical) => ServiciiOferite.Add(serviciuMedical);
    }
    
    public class ServiciuMedical
    {
        public string Denumire { get; init; }
        public decimal Pret { get; init; }
        public int DurataMinute { get; init; }

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