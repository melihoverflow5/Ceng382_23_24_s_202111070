namespace ReservationSystem.Repositories
{
    using ReservationSystem.Models;
    using ReservationSystem.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;

    public class ReservationRepository : IReservationRepository
    {
         private string _filePath;
        private List<Reservation> _reservations;

        public ReservationRepository(string filePath)
        {
            _filePath = filePath;
            _reservations = LoadReservationsFromJson();
        }

        private List<Reservation> LoadReservationsFromJson()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    return JsonSerializer.Deserialize<List<Reservation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Reservation>();
                }
                return new List<Reservation>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading from the file: {ex.Message}");
                return new List<Reservation>();
            }
        }

        private void SaveReservationsToJson()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_reservations, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }

        public void AddReservation(Reservation reservation)
        {
            _reservations.Add(reservation);
            SaveReservationsToJson();
        }

        public void DeleteReservation(Reservation reservation)
        {
            var existingReservation = _reservations.FirstOrDefault(r => r.Equals(reservation));
            if (existingReservation != null)
            {
                _reservations.Remove(existingReservation);
                SaveReservationsToJson();
            }
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservations;
        }
    }
}