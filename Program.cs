using ReservationSystem.Services;
using ReservationSystem.Repositories;
using ReservationSystem.Interfaces;
using ReservationSystem.Logging;
using ReservationSystem.Models;
using ReservationSystem.Handlers;
/*
SOLID principles like SRP and DI principles are quite important in software development because they help to make the code more maintainable, scalable and testable.
The Single Responsibility Principle (SRP) states that a class should have only one reason to change. This means that a class should have only one job or responsibility.
Last week we have everything in one class, which is not a good practice. We have to separate the responsibilities of the class into different classes. I obeyed this principle by 
creating different classes for different responsibilities like RoomHandler, ReservationService, ReservationRepository, FileLogger, etc.
Dependency Injection (DI) is a technique in which an object receives other objects that it depends on. These other objects are called dependencies. In the last week, we have used 
just a ReservationHandler class to handle all the responsibilities. But now I have used DI to inject the dependencies of the ReservationService class. I have injected the ReservationRepository,
Logger, and RoomHandler dependencies into the ReservationService class. This makes the code more maintainable and testable.

*/
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