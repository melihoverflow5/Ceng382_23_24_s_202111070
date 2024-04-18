namespace ReservationSystem.Models
{
    using System.Text.Json.Serialization;
    public record RoomData{
        [JsonPropertyName("Room")]
        public Room[] Rooms {get; set;}
    }
}