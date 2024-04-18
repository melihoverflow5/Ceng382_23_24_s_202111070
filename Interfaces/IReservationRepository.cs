namespace ReservationSystem.Interfaces
{
    using ReservationSystem.Models;

    public interface IReservationRepository
    {
        void AddReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
        List<Reservation> GetAllReservations();
    }
}