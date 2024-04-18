using ReservationSystem.Services;
using ReservationSystem.Repositories;
using ReservationSystem.Interfaces;
using ReservationSystem.Logging;
using ReservationSystem.Models;
using ReservationSystem.Handlers;

class Program
{
    static void Main(string[] args)
    {

        string roomDataPath = Path.Combine("Data", "Data.json");
        string logDataPath = Path.Combine("Data", "LogData.json");
        string reservationDataPath = Path.Combine("Data", "ReservationData.json");
        
        ILogger logger = new FileLogger(logDataPath);

        IRoomHandler roomHandler = new RoomHandler(roomDataPath);

        IReservationRepository reservationRepository = new ReservationRepository(reservationDataPath);
        
        IReservationService reservationService = new ReservationService(reservationRepository, logger, roomHandler);


        reservationService.AddReservation(DateTime.Now, DateTime.Now, "John Doe", roomHandler.GetRooms()[1]);
        reservationService.DisplayWeeklySchedule();

        
    }
}