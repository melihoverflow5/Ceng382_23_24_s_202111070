using System;
using System.IO;
using System.Text.Json;

namespace ReservationSystem.Logging
{
    using ReservationSystem.Interfaces;
    using ReservationSystem.Models;
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private List<LogRecord> _logRecords = new List<LogRecord>();

        public FileLogger(string filePath)
        {
            _filePath = filePath;
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _logRecords = JsonSerializer.Deserialize<List<LogRecord>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load logs from file: {ex.Message}");
            }
        }

        // Adjust this method to accept not just a message but also the reservation name and room name
        public void Log(LogRecord log)
        {
            _logRecords.Add(log);
            string json = JsonSerializer.Serialize(_logRecords, new JsonSerializerOptions
            {
                WriteIndented = true  // Makes the JSON output easier to read
            });

            try
            {
                    File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log to file: {ex.Message}");
            }
        }
    }
}
