using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

public class RoomData
{
    [JsonPropertyName("Room")]
    public Room[] Rooms{get; set;}
}

public class Room
{
    [JsonPropertyName("roomId")]
    public string roomId {get; set;}

    [JsonPropertyName("roomName")]
    public string roomName {get; set;}

    [JsonPropertyName("capacity")]
    public int capacity{get; set;}
}

public class Reservation 
{
    public DateTime time {get; set;}
    public DateTime date {get; set;}
    public string? reserverName {get; set;}
    public Room room {get; set;}
}

public class ReservationHandler
{
    private Reservation[][] reservations;
    private RoomData roomData;
    private DateTime[] dateData;

    public ReservationHandler(RoomData rooms, DateTime[] dates)
    {
        roomData = rooms;
        dateData = dates;
        reservations = new Reservation[roomData.Rooms.Length][];
        for (int i = 0; i < roomData.Rooms.Length; i++)
        {
            reservations[i] = new Reservation[dates.Length];
        }   
    }

    public void AddReservation(Reservation reservation)
    {
        int roomIndex = Convert.ToInt32(reservation.room.roomId ) - 1;
        int dateIndex = Array.IndexOf(dateData, reservation.date);
        reservations[roomIndex][dateIndex] = reservation;
    }

    public void deleteReservation(Reservation reservation)
    {
        int roomIndex = Convert.ToInt32(reservation.room.roomId ) - 1;
        int dateIndex = Array.IndexOf(dateData, reservation.date);
        if (reservations[roomIndex][dateIndex] == null)
        {
            Console.WriteLine("No reservation found for the given date and room");
            return;
        }
        reservations[roomIndex][dateIndex] = null;
    }

    public void displayWeeklySchedule()
    {
        Console.WriteLine("Weekly Schedule");
        Console.Write("Room".PadRight(5) + " | " + "Capacity".PadRight(8) + " | ");
        for (int i = 0; i < dateData.Length; i++)
        {
            Console.Write($"{dateData[i].ToString("dd.MM.yyyy").PadRight(10)} | ");
        }
        Console.WriteLine();
        Console.WriteLine("-".PadRight(dateData.Length * 13 + 18, '-'));
        
        for (int j = 0; j < roomData.Rooms.Length; j++)
        {
            Console.Write($"{roomData.Rooms[j].roomName.PadRight(5)} | {roomData.Rooms[j].capacity.ToString().PadRight(8)} | ");
            for (int k = 0; k < reservations[j].Length; k++)
            {
                string displayText = "";
                if (reservations[j][k] != null && reservations[j][k].reserverName != null)
                {
                    displayText = reservations[j][k].reserverName;
                    string[] surname = displayText.Split(" ");
                    displayText = surname[surname.Length - 1].Length > 10 ? surname[surname.Length - 1].Substring(0,8) + ".." : surname[1];
                }
                else
                {
                    displayText = "Free";
                }
                Console.Write($"{displayText.PadRight(10)} | ");
            }
            Console.WriteLine();
        }
    }
}
class Program
{
    static void Main(String[]args)
    {
        DateTime[] dateData = new DateTime[]
        {
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2),
            DateTime.Now.AddDays(3),
            DateTime.Now.AddDays(4),
            DateTime.Now.AddDays(5),
            DateTime.Now.AddDays(6),
        };
        CultureInfo culture = new CultureInfo("en-US");

        //File path defined as relative path to the project
        //If the file is in the bin/Debug/net8.0, you can use the file name directly
        string filePath = "../../../Data.json";

        string jsonString = "";

        try{
        jsonString = File.ReadAllText(filePath);
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
            Console.WriteLine("No data found in the file");
            return;
        }
        
        ReservationHandler reservationHandler = new ReservationHandler(roomData, dateData);

        Reservation reservation = new Reservation();
        reservation.date = dateData[4];
        reservation.room = roomData.Rooms[3];
        reservation.time = DateTime.Parse("12:00", culture);
        reservation.reserverName = "John Weasley";
        
        reservationHandler.AddReservation(reservation);

        reservationHandler.displayWeeklySchedule();

        reservationHandler.deleteReservation(reservation);

        reservationHandler.displayWeeklySchedule();
    }
}


