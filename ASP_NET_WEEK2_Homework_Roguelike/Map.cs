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
        public Dictionary<(int, int), Room> discoveredRooms;
        public List<RoomToDiscover> roomsToDiscover;

        public Map()
        {
            discoveredRooms = new Dictionary<(int, int), Room>();
            roomsToDiscover = new List<RoomToDiscover>();
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
            discoveredRooms[(room.X, room.Y)] = room;
            room.IsExplored = true;

            //Adds "roomsToDiscover" depending on existing exits in new room
            foreach (var direction in room.Exits.Keys)
            {
                (int newX, int newY) = GetCoordinatesInDirection(room.X, room.Y, direction);
                if (!discoveredRooms.ContainsKey((newX, newY)))
                {
                    RoomToDiscover rtd = new RoomToDiscover(newX, newY, OppositeDirection(direction));
                    roomsToDiscover.Add(rtd);

                    // Blocks direction in room to be discoverd depending on exits in this one
                    rtd.BlockedDirections.Add(OppositeDirection(direction));
                }
            }

            // updates roomsToDiscover 
            foreach (var rtd in roomsToDiscover)
            {
                //checks if new room is nearby the other one
                if ((room.X, room.Y) == GetCoordinatesInDirection(rtd.X, rtd.Y, rtd.EnteringDirection))
                {
                    //adds new blocked direction
                    rtd.BlockedDirections.Add(OppositeDirection(rtd.EnteringDirection));
                }
            }

            // removes roomsToDiscover from list if discoverd
            roomsToDiscover.RemoveAll(r => r.X == room.X && r.Y == room.Y);
        }

        public Room GetDiscoveredRoom(int x, int y)
        {
            //Checks if room it is in collection of discoveredRooms
            if (discoveredRooms.TryGetValue((x, y), out Room room))
            {
                return room;
            }
            return null; //not discoverd room
        }

        public Room GenerateRoom(int currentX, int currentY, string direction)
        {
            // Calculates X,Y coordinates of the new room
            (int newX, int newY) = GetCoordinatesInDirection(currentX, currentY, direction);

            Room newRoom = new Room(newX, newY);

            // Adds random event to a new room
            RandomEvent randomEvent = EventGenerator.GenerateEvent();
            if (randomEvent != null)
            {
                newRoom.EventStatus = randomEvent.GetType().Name;  //EventStatus
            }
            else
            {
                newRoom.EventStatus = "none";
            }

            AddDiscoveredRoom(newRoom);

            // Generate random exits for the new room
            GenerateRandomExits(newRoom);

            // Connection with the previous room
            Room currentRoom = GetDiscoveredRoom(currentX, currentY);
            if (currentRoom != null)
            {
                currentRoom.Exits[direction] = newRoom;
                newRoom.Exits[OppositeDirection(direction)] = currentRoom;
            }

            return newRoom;
        }
        private void GenerateRandomExits(Room room)
        {
            var directions = new[] { "north", "south", "east", "west" };
            var random = new Random();

            // Ensure the room has between 2 and 4 exits
            int numberOfExits = random.Next(2, 5);
            var availableDirections = directions.OrderBy(_ => random.Next()).Take(numberOfExits).ToList();

            foreach (var direction in availableDirections)
            {
                if (!room.Exits.ContainsKey(direction))
                {
                    (int newX, int newY) = GetCoordinatesInDirection(room.X, room.Y, direction);

                    if (!discoveredRooms.ContainsKey((newX, newY)))
                    {
                        room.Exits[direction] = null; // Placeholder for future room
                        RoomToDiscover rtd = new RoomToDiscover(newX, newY, OppositeDirection(direction));
                        roomsToDiscover.Add(rtd);
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

        private (int, int) GetCoordinatesInDirection(int x, int y, string direction)
        {
            switch (direction)
            {
                case "north": return (x, y + 1);
                case "south": return (x, y - 1);
                case "east": return (x + 1, y);
                case "west": return (x - 1, y);
                default: return (x, y);
            }
        }

        private string OppositeDirection(string direction)
        {
            switch (direction)
            {
                case "north": return "south";
                case "south": return "north";
                case "east": return "west";
                case "west": return "east";
                default: return "";
            }
        }

        public void DisplayMap(PlayerCharacter player)
        {
            var minX = discoveredRooms.Keys.Min(k => k.Item1);
            var maxX = discoveredRooms.Keys.Max(k => k.Item1);
            var minY = discoveredRooms.Keys.Min(k => k.Item2);
            var maxY = discoveredRooms.Keys.Max(k => k.Item2);

            WriteLine();
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (discoveredRooms.TryGetValue((x, y), out Room room))
                    {
                        if (player.CurrentX == x && player.CurrentY == y)
                        {
                            Write("P");
                        }
                        else
                        {
                            Write("+");
                        }
                    }
                    else
                    {
                        Write(" ");
                    }

                    // Draw horizontal connection
                    if (x < maxX && discoveredRooms.TryGetValue((x, y), out Room currentRoom) && currentRoom.Exits.ContainsKey("east"))
                    {
                        Write("-");
                    }
                    else
                    {
                        Write(" ");
                    }
                }
                WriteLine();

                // Draw vertical connections
                if (y > minY)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        if (discoveredRooms.TryGetValue((x, y), out Room room) && room.Exits.ContainsKey("south"))
                        {
                            Write("|");
                        }
                        else
                        {
                            Write(" ");
                        }

                        // Space between columns
                        if (x < maxX)
                        {
                            Write(" ");
                        }
                    }
                    WriteLine();
                }
            }
        }
    }
}