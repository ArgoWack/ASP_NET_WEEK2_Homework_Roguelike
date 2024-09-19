using ASP_NET_WEEK2_Homework_Roguelike.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    public class Map
    {
        public Dictionary<(int, int), Room> DiscoveredRooms { get; set; }
        public List<RoomToDiscover> RoomsToDiscover { get; set; }

        public Map()
        {
            DiscoveredRooms = new Dictionary<(int, int), Room>();
            RoomsToDiscover = new List<RoomToDiscover>();
        }

        public void InitializeStartingRoom()
        {
            Room startingRoom = new Room(0, 0);

            startingRoom.Exits["north"] = null;
            startingRoom.Exits["south"] = null;
            startingRoom.Exits["east"] = null;
            startingRoom.Exits["west"] = null;

            AddDiscoveredRoom(startingRoom);
        }

        public void AddDiscoveredRoom(Room room)
        {
            DiscoveredRooms[(room.X, room.Y)] = room;
            room.IsExplored = true;

            // Add logic to handle rooms to discover and updating discovered exits
            foreach (var direction in room.Exits.Keys)
            {
                (int newX, int newY) = GetCoordinatesInDirection(room.X, room.Y, direction);
                if (!DiscoveredRooms.ContainsKey((newX, newY)))
                {
                    RoomToDiscover rtd = new RoomToDiscover(newX, newY, OppositeDirection(direction));
                    RoomsToDiscover.Add(rtd);
                    rtd.BlockedDirections.Add(OppositeDirection(direction));
                }
            }

            RoomsToDiscover.RemoveAll(r => r.X == room.X && r.Y == room.Y);
        }

        // Retrieves a discovered room based on coordinates
        public Room GetDiscoveredRoom(int x, int y)
        {
            return DiscoveredRooms.TryGetValue((x, y), out Room room) ? room : null;
        }

        // Generates a new room based on current position and direction
        public Room GenerateRoom(int currentX, int currentY, string direction)
        {
            (int newX, int newY) = GetCoordinatesInDirection(currentX, currentY, direction);
            Room newRoom = new Room(newX, newY);

            // Add a random event or no event to the new room
            RandomEvent randomEvent = EventGenerator.GenerateEvent();
            newRoom.EventStatus = randomEvent != null ? randomEvent.GetType().Name : "none";

            AddDiscoveredRoom(newRoom);

            // Generate random exits for the new room
            GenerateRandomExits(newRoom);

            // Connect the new room with the previous one based on direction
            Room currentRoom = GetDiscoveredRoom(currentX, currentY);
            if (currentRoom != null)
            {
                currentRoom.Exits[direction] = newRoom;
                newRoom.Exits[OppositeDirection(direction)] = currentRoom;
            }

            return newRoom;
        }

        // Generates random exits for a room
        private void GenerateRandomExits(Room room)
        {
            var directions = new[] { "north", "south", "east", "west" };
            var random = new Random();

            int numberOfExits = random.Next(2, 5);
            var availableDirections = directions.OrderBy(_ => random.Next()).Take(numberOfExits).ToList();

            foreach (var direction in availableDirections)
            {
                if (!room.Exits.ContainsKey(direction))
                {
                    (int newX, int newY) = GetCoordinatesInDirection(room.X, room.Y, direction);
                    if (!DiscoveredRooms.ContainsKey((newX, newY)))
                    {
                        room.Exits[direction] = null;
                        RoomToDiscover rtd = new RoomToDiscover(newX, newY, OppositeDirection(direction));
                        RoomsToDiscover.Add(rtd);
                    }
                }
            }
        }

        public Room MovePlayer(ref int playerX, ref int playerY, string direction)
        {
            //Calculates new coordinates
            (int newX, int newY) = GetCoordinatesInDirection(playerX, playerY, direction);

            //Checks following room
            Room targetRoom = GetDiscoveredRoom(newX, newY);
            if (targetRoom == null)
            {
                //creates room if not existent
                targetRoom = GenerateRoom(playerX, playerY, direction);
            }

            //current position
            playerX = newX;
            playerY = newY;

            return targetRoom;
        }

        // Helper method to calculate coordinates in a given direction
        private (int, int) GetCoordinatesInDirection(int x, int y, string direction)
        {
            return direction switch
            {
                "north" => (x, y + 1),
                "south" => (x, y - 1),
                "east" => (x + 1, y),
                "west" => (x - 1, y),
                _ => (x, y)
            };
        }

        // Helper method to get the opposite direction
        private string OppositeDirection(string direction)
        {
            return direction switch
            {
                "north" => "south",
                "south" => "north",
                "east" => "west",
                "west" => "east",
                _ => ""
            };
        }
    }
}