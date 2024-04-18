namespace ReservationSystem.Interfaces
{
    using ReservationSystem.Models;
    using System.Collections.Generic;

    public interface IRoomHandler
    {
        List<Room> GetRooms();
        void SaveRooms(List<Room> rooms);
    }
}