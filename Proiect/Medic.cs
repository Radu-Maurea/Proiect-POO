namespace Proiect
{
    public class Medic : User
    {
        
        public string Specialitate { get; set; }
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
    }
    
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