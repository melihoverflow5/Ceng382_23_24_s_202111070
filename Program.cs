using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

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
            DateTime.Now.Date.AddDays(1),
            DateTime.Now.Date.AddDays(2),
            DateTime.Now.Date.AddDays(3),
            DateTime.Now.Date.AddDays(4),
            DateTime.Now.Date.AddDays(5),
            DateTime.Now.Date.AddDays(6),
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
            throw new Exception("No data found in the file");
        }
        
        ReservationHandler reservationHandler = new ReservationHandler(roomData, dateData);
        
        mainLoop(reservationHandler, roomData, dateData);

    }

    public static void mainLoop(ReservationHandler reservationHandler, RoomData roomData, DateTime[] dateData){
        Console.WriteLine("----------Welcome to the Reservation System----------");
        while (true)
        {
            Console.WriteLine("1. Add Reservation");
            Console.WriteLine("2. Delete Reservation");
            Console.WriteLine("3. Display Weekly Schedule");
            Console.WriteLine("4. Exit");
            Console.WriteLine("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the room name: ");
                    string roomName = Console.ReadLine();
                    Console.WriteLine("Enter the date (dd.MM.yyyy): ");
                    string strDate = Console.ReadLine();
                    DateTime date = checkDate(strDate, dateData);
                    Console.WriteLine("Enter the time (HH:mm): ");
                    string strTime = Console.ReadLine();
                    DateTime time = checkTime(strTime);
                    Console.WriteLine("Enter the reserver full name: ");
                    string reserverName = Console.ReadLine();
                    reserverName = checkName(reserverName);
                    // Check null or wrong inputs 
                    
                    Reservation reservation = new Reservation()
                    {
                        time = time,
                        date = date,
                        reserverName = reserverName,
                        room = roomData.Rooms[Array.FindIndex(roomData.Rooms, room => room.roomName == roomName)]
                    };
                    reservationHandler.AddReservation(reservation);
                    Console.WriteLine("Reservation added, You can check the weekly schedule.");
                    Console.WriteLine();
                    break;
                case 2:
                    Console.WriteLine("Enter the room name: ");
                    roomName = Console.ReadLine();
                    roomName = checkRoomName(roomName, roomData);
                    Console.WriteLine("Enter the date (dd.MM.yyyy): ");
                    strDate = Console.ReadLine();
                    date = checkDate(strDate, dateData);
                    Console.WriteLine("Enter the time (HH:mm): ");
                    strTime = Console.ReadLine();
                    time = checkTime(strTime);Console.WriteLine("Enter the reserver full name: ");
                    reserverName = Console.ReadLine();
                    reserverName = checkName(reserverName);

                    reservation = new Reservation()
                    {
                        time = time,
                        date = date,
                        reserverName = reserverName,
                        room = roomData.Rooms[Array.FindIndex(roomData.Rooms, room => room.roomName == roomName)]
                    };
                    reservationHandler.deleteReservation(reservation);
                    Console.WriteLine("Reservation deleted, You can check the weekly schedule.");
                    Console.WriteLine();
                    break;
                case 3:
                    reservationHandler.displayWeeklySchedule();
                    Console.WriteLine();
                    break;
                case 4:
                    return;
            }
        }
    }

    public static string checkName(string name)
    {
        if (name == null || name.Length == 0)
        {
            Console.WriteLine("Name cannot be empty, Enter again:");
            name = Console.ReadLine();
            return checkName(name);
        }

        return name;
    }

    public static string checkRoomName(string roomName, RoomData roomData){
        if (roomName == null || roomName.Length == 0)
        {
            Console.WriteLine("Room name cannot be empty, Enter again:");
            roomName = Console.ReadLine();
            return checkRoomName(roomName, roomData);
        }
        if (Array.FindIndex(roomData.Rooms, room => room.roomName == roomName) == -1)
        {
            Console.WriteLine("Room not found, Enter again:");
            roomName = Console.ReadLine();
            return checkRoomName(roomName, roomData);
        }
        return roomName;
    }

    public static DateTime checkDate(string date, DateTime[] dateData)
    {
        if (date == null || date.Length == 0)
        {
            Console.WriteLine("Date cannot be empty, Enter again:");
            date = Console.ReadLine();
            return checkDate(date, dateData);
        }
        if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
        {
            Console.WriteLine("Invalid date format, Enter again:");
            date = Console.ReadLine();
            return checkDate(date, dateData);
        }
        if (Array.IndexOf(dateData, dateValue) == -1){
            Console.WriteLine("Date not found in the weekly schedule, Enter again:");
            date = Console.ReadLine();
            return checkDate(date, dateData);
        }
        return DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
    }

    public static DateTime checkTime(string time){
        if (time == null || time.Length == 0)
        {
            Console.WriteLine("Time cannot be empty, Enter again:");
            time = Console.ReadLine();
            return checkTime(time);
        }
        if (!DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeValue))
        {
            Console.WriteLine("Invalid time format, Enter again:");
            time = Console.ReadLine();
            return checkTime(time);
        }
        return DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
    }

}


