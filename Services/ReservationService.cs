namespace ReservationSystem.Services
{
    using ReservationSystem.Models;
    using ReservationSystem.Interfaces;
    using System.Linq;
    using System.Text;

    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogger _logger;
        private readonly IRoomHandler _roomHandler;

        public ReservationService(IReservationRepository reservationRepository, ILogger logger, IRoomHandler roomHandler)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
            _roomHandler = roomHandler;
        }

        public void AddReservation(DateTime time, DateTime date, string reserverName, Room room)
        {
            _logger.Log($"Adding reservation for {reserverName} at {time:HH:mm} on {date:yyyy-MM-dd} in room {room.Name}");
            Reservation reservation = new Reservation(time, date, reserverName, room);
            _reservationRepository.AddReservation(reservation);
        }

        public void DeleteReservation(Reservation reservation)
        {
            _logger.Log($"Deleting reservation for {reservation.ReserverName} at {reservation.Time:HH:mm} on {reservation.Date:yyyy-MM-dd} in room {reservation.Room.Name}");
            _reservationRepository.DeleteReservation(reservation);
        }

        public void DisplayWeeklySchedule()
    {
        var now = DateTime.Now.Date;  // Ensures comparison is date-only
        var endOfWeek = now.AddDays(7 - (int)now.DayOfWeek);
        var dates = Enumerable.Range(0, 7).Select(day => now.AddDays(day)).ToList();
        var rooms = _roomHandler.GetRooms();
        var reservations = _reservationRepository.GetAllReservations()
            .Where(r => r.Date.Date >= now && r.Date.Date < endOfWeek)
            .ToList();

        var sb = new StringBuilder();
        // Create header row with dates
        sb.Append("Room    |");
        foreach (var date in dates)
        {
            sb.Append($" {date:dd.MM.yyyy} |");
        }
        sb.AppendLine();

        // Check if rooms are loaded
        if (!rooms.Any())
        {
            sb.AppendLine("No rooms available.");
            Console.WriteLine(sb.ToString());
            return;
        }

        // Create rows for each room
        foreach (var room in rooms)
        {
            sb.Append($"{room.Name,-10} |");  // Ensure room names are printed, adjust -10 as needed for alignment
            foreach (var date in dates)
            {
                var reservationForDay = reservations
                    .FirstOrDefault(r => r.Room.Id == room.Id && r.Date.Date == date);

                if (reservationForDay != null)
                {
                    sb.Append($" {reservationForDay.ReserverName,-10} |");  // Adjust -10 as needed for alignment
                }
                else
                {
                    sb.Append(" Free       |");
                }
            }
            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }
    }
}