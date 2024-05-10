using System;
using System.IO;
using System.Text.Json;

namespace ReservationSystem.Logging
{
    using ReservationSystem.Interfaces;
    using ReservationSystem.Models;
    public static class FileLogger
    {
        private static string _filePath;
        private static List<LogRecord> logRecords = new List<LogRecord>();

        public static void Initialize(string filePath){
            _filePath = filePath;
            loadLogsFromJson();
        }

        private static void loadLogsFromJson(){
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    logRecords = JsonSerializer.Deserialize<List<LogRecord>>(json);
                } else{
                    logRecords = new List<LogRecord>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load logs from file: {ex.Message}");
            }
        }

        public static List<LogRecord> DisplayLogsByName(string name) {
            var filteredLogs = logRecords.Where(r => r.ReserverName.Equals(name,StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredLogs;
        } 
        public static List<LogRecord> DisplayLogsByDate(DateTime start, DateTime end){
            var filteredLogs = logRecords.Where(r => r.Timestamp >= start && r.Timestamp <= end).ToList();
            return filteredLogs;
        }

        public static void Log(LogRecord log)
        {
            logRecords.Add(log);
            string json = JsonSerializer.Serialize(logRecords, new JsonSerializerOptions
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
