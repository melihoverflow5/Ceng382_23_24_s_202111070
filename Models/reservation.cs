public class Reservation
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public int RoomId { get; set; }  // Foreign key for Room
}
