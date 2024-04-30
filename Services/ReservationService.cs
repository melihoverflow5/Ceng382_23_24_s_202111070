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
            Reservation reservation = new Reservation(time, date, reserverName, room);
            
            if(!_roomHandler.GetRooms().Contains(room))
            {
                LogRecord logRecordDNE = new LogRecord
                (
                    DateTime.Now,
                    reserverName,
                    room.Name, 
                    $"Room {room.Name} does not exist. Reservation could not be created."    
                );

                _logger.Log(logRecordDNE);
                return;
            }
            if(_reservationRepository.GetAllReservations().Any(r => r.Room == room && r.Date.Date == date.Date))
            {
                LogRecord logRecordAR = new LogRecord
                (
                    DateTime.Now,
                    reserverName,
                    room.Name, 
                    $"Room {room.Name} is already reserved at {time:HH:mm} on {date:yyyy-MM-dd}. Reservation could not created."    
                );
                _logger.Log(logRecordAR);
                return;
            }
            _reservationRepository.AddReservation(reservation);
            LogRecord logRecordSuccess = new LogRecord
            (
                DateTime.Now,
                reserverName,
                room.Name, 
                $"Reservation for {reserverName} at {time:HH:mm} on {date:yyyy-MM-dd} in room {room.Name} was successfully created."    
            );
            _logger.Log(logRecordSuccess);
        }

        public void DeleteReservation(Reservation reservation)
        {
            LogRecord logRecord = new LogRecord
            (
                DateTime.Now,
                reservation.ReserverName,
                reservation.Room.Name, 
                $"Deleting reservation for {reservation.ReserverName} at {reservation.Time:HH:mm} on {reservation.Date:yyyy-MM-dd} in room {reservation.Room.Name}"    
            );
            _logger.Log(logRecord);
            _reservationRepository.DeleteReservation(reservation);
        }

        public void DisplayWeeklySchedule()
        {
            var now = DateTime.Now.Date;  
            var endOfWeek = now.AddDays(7 - (int)now.DayOfWeek);
            var dates = Enumerable.Range(0, 7).Select(day => now.AddDays(day)).ToList();
            var rooms = _roomHandler.GetRooms();
            var reservations = _reservationRepository.GetAllReservations()
                .Where(r => r.Date.Date >= now && r.Date.Date < endOfWeek)
                .ToList();

            var sb = new StringBuilder();
            sb.Append("Room       |");

            foreach (var date in dates)
            {
                sb.Append($" {date:dd.MM.yyyy} |");
            }
            sb.AppendLine();

            if (!rooms.Any())
            {
                sb.AppendLine("No rooms available.");
                Console.WriteLine(sb.ToString());
                return;
            }

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