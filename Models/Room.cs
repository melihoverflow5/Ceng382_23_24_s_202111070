namespace ReservationSystem.Models
{
    using System.Text.Json.Serialization;
    public record Room
    {
        [JsonPropertyName("roomId")]
        public string Id { get; init; }

        [JsonPropertyName("roomName")]
        public string Name { get; init; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; init; }
    }
}