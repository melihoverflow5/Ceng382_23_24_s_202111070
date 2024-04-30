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

        string roomDataPath = "Data/Data.json";
        string logDataPath = "Data/LogData.json";
        string reservationDataPath = "Data/ReservationData.json";
        
        ILogger logger = new FileLogger(logDataPath);

        IRoomHandler roomHandler = new RoomHandler(roomDataPath);

        IReservationRepository reservationRepository = new ReservationRepository(reservationDataPath);
        
        IReservationService reservationService = new ReservationService(reservationRepository, logger, roomHandler);

        reservationService.AddReservation(DateTime.Now, DateTime.Now, "John Wick", roomHandler.GetRooms()[2]);
        reservationService.AddReservation(DateTime.Now.AddDays(4), DateTime.Now.AddDays(4), "John Snow", roomHandler.GetRooms()[5]);
        reservationService.AddReservation(DateTime.Now.AddDays(2), DateTime.Now.AddDays(2), "Tom Cruise", roomHandler.GetRooms()[1]);
        reservationService.DeleteReservation(reservationRepository.GetAllReservations()[0]);
        reservationService.DisplayWeeklySchedule();

        
    }
}