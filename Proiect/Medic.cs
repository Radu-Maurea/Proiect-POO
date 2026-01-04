namespace Proiect
{
    public class Specialitate
    {
        public string Nume { get; set; }

        public Specialitate(string nume)
        {
            Nume = nume;
        }

        public override string ToString()
        {
            return Nume;
        }
    }

    public class Medic : User
    {
        public Specialitate Specialitate { get; private set; }
        public string ProgramLucru { get; private set; }

        public Medic(string email, string password) : base(email,password) { }
        private List<Pacient> Pacienti = new List<Pacient>();
        public override string Rol()
        {
            return "Medic";
        }
        
        public void AfisarezPacienti()
        {
            foreach (var pacient in Pacienti)
            {
                Console.WriteLine(pacient.ToString());
            }
        }
        public void SetSpecialitate(Specialitate specialitate)
        {
            Specialitate = specialitate;
        }

        public void SetProgram(string program)
        {
            ProgramLucru = program;
        }

        public override string ToString()
        {
            return $"{Email} | {Specialitate} | {ProgramLucru}";
        }


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