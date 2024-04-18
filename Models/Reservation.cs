namespace ReservationSystem.Models
{
    public record Reservation(DateTime Time, DateTime Date, string ReserverName, Room Room);
}