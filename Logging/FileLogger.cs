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
            loadLogsFromJson();
        }

        private void loadLogsFromJson(){
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _logRecords = JsonSerializer.Deserialize<List<LogRecord>>(json);
                } else{
                    _logRecords = new List<LogRecord>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load logs from file: {ex.Message}");
            }
        }

        public void Log(LogRecord log)
        {
            _logRecords.Add(log);
            string json = JsonSerializer.Serialize(_logRecords, new JsonSerializerOptions
            {
                WriteIndented = true 
            });

            try
            {
                    File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log to file: {ex.Message}");
            }
            loadLogsFromJson();
        }
    }
}
