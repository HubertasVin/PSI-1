namespace Project.Helpers
{
    // Define a delegate for logging
    public delegate void LogHandler(string message);

    public class Logger
    {
        private static readonly string logFilePath = "log.txt";

        // The delegate instance for logging
        public static LogHandler LogMethod = LogToFile;

        public static void Log(string message)
        {
            // Prefix the message with a timestamp
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}";

            // Invoke the delegate to log the message
            LogMethod(logMessage);
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