namespace Proiect;

public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message);
    void LogWarning(string message);
}

public class FileLogger : ILogger
{
    private const string FilePath = @"F:\info\an_2\Proiect POO\Proiect\Proiect\logs.txt";

    public void LogInfo(string message)=>Write("INFO",message);
    public void LogError(string message)=>Write("ERROR",message);
    public void LogWarning(string message)=>Write("WARNING",message);

    private void Write(string level, string message)
    {
        string lg = $"{DateTime.Now:yy-MM-dd HH:mm:ss} [{level}] {message}";
        File.AppendAllText(FilePath, lg + Environment.NewLine);

    }
}