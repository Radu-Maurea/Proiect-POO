namespace Proiect;

public class Programare
{
    public Medic Medic { get; set; }
    public Pacient Pacient { get; set; }
    public ServiciuMedical Serviciu { get; set; }
    public string DataOra { get; set; }
    public string Diagnostic { get; set; }
    public bool Vazut { get; set; }
    public Programare(Medic medic, Pacient pacient, ServiciuMedical serviciu, string dataOra)
    {
        Medic = medic;
        Pacient = pacient;
        Serviciu = serviciu;
        DataOra = dataOra;
        Vazut = false;
    }
    
    public void SetDiagnostic(string diagnostic) => Diagnostic = diagnostic;
    
    public override string ToString()
    {
        return $"{DataOra} | {Medic.Email} | {Pacient.Email} | {Serviciu.Denumire}";
    }
}