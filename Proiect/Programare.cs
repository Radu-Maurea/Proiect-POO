using System.Text.Json.Serialization;
namespace Proiect;

public class Programare
{
    [JsonInclude]
    public Medic Medic { get; private set; }
    [JsonInclude]
    public Pacient Pacient { get; private set; }
    [JsonInclude]
    public ServiciuMedical Serviciu { get; private set; }
    [JsonInclude]
    public string DataOra { get; private set; }
    [JsonInclude]
    public string Diagnostic { get; private set; }
    [JsonInclude]
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