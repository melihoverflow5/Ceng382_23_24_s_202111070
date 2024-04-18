namespace ReservationSystem.Interfaces
{
    using ReservationSystem.Models;

    public interface IReservationService
    {
        void AddReservation(DateTime time, DateTime date, string reserverName, Room room);
        void DeleteReservation(Reservation reservation);
        void DisplayWeeklySchedule();
    }
}