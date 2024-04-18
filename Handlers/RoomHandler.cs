namespace ReservationSystem.Handlers
{
    using ReservationSystem.Interfaces;
    using ReservationSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    public class RoomHandler : IRoomHandler
    {
        private string _filePath;
        private List<Room> _rooms;
        public RoomHandler(string filePath)
        {
            _filePath = filePath;
            _rooms = LoadRoomsFromJson();
            Console.WriteLine($"Loaded {_rooms.Count} rooms.");
        }

        private List<Room> LoadRoomsFromJson()
        {
            string jsonString = "";

            try{
            jsonString = File.ReadAllText(_filePath);
            } catch (Exception e)
            {
                Console.WriteLine("Error reading file: " + e.Message + "Please check the file path");
            }

            var roomData = new RoomData();

            if(!string.IsNullOrEmpty(jsonString))
            {
                roomData = JsonSerializer.Deserialize<RoomData>(
                                        jsonString, 
                                        new JsonSerializerOptions()
                {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString | 
                        JsonNumberHandling.WriteAsString
                });
            }

            if (roomData.Rooms == null)
            {
                throw new Exception("No data found in the file");
            }

            return roomData.Rooms.ToList();
        }

        public List<Room> GetRooms()
        {
            return _rooms;
        }

        public void SaveRooms(List<Room> rooms)
        {
            try
            {
                var roomsData = new RoomData { Rooms = rooms.ToArray() };
                string json = JsonSerializer.Serialize(roomsData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
    }
}