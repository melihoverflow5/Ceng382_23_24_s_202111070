namespace ReservationSystem.Interfaces
{
    using ReservationSystem.Models;
    public interface ILogger
    {
        void Log(LogRecord log);
    }
}