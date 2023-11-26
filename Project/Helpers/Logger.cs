namespace Project.Helpers
{
    public delegate void LogHandler(string message);

    public class Logger
    {
        private static readonly string logFilePath = "log.txt";

        public static LogHandler? LogEvent;

        public static void LogException(Exception e)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {e}";

            // Log to file
            LogToFile(logMessage);

            // Invoke the delegate if it's subscribed
            LogEvent?.Invoke(logMessage);
        }

        private static void LogToFile(string message)
        {
            if (!File.Exists(logFilePath))
                File.Create(logFilePath).Dispose();

            using StreamWriter sw = File.AppendText(logFilePath);
            sw.WriteLine(message);
        }
    }
}
