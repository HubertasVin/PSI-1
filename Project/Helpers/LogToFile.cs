namespace Project.Helpers;

public class LogToFile
{
    public static void LogException(Exception e)
    {
        string path = "log.txt";
        if (!File.Exists(path))
            File.Create(path).Dispose();
        using StreamWriter sw = File.AppendText(path);
        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + e);
    }
}