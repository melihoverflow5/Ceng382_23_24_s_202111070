namespace ReservationSystem.Models
{
    public record LogRecord(DateTime Timestamp, string ReserverName, string RoomName);
}