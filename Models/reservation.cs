namespace ReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public int RoomId { get; set; } 
        public string UserId { get; set; } 
    }
}
