using System;
using System.IO;
using System.Text.Json;

namespace ReservationSystem.Logging
{
    using ReservationSystem.Interfaces;
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            var logEntry = new
            {
                Timestamp = DateTime.Now,
                Message = message
            };

            string json = JsonSerializer.Serialize(logEntry);

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(_filePath, true))
                {
                    streamWriter.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log to file: {ex.Message}");
            }
        }
    }
}
