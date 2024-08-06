using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    public class Map
    {
        private Dictionary<(int, int), Room> discoveredRooms;
        private List<RoomToDiscover> roomsToDiscover;

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

            // Usuń pokój z listy roomsToDiscover, jeśli został odkryty
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
            //Calculates X,Y coordinates of room
            (int newX, int newY) = GetCoordinatesInDirection(currentX, currentY, direction);

            // Checks if room it on the list to be discoverd
            var roomToDiscover = roomsToDiscover.FirstOrDefault(r => r.X == newX && r.Y == newY);
            Room newRoom;

            if (roomToDiscover != null)
            {
                //If room found on the list
                newRoom = new Room(newX, newY);
                AddDiscoveredRoom(newRoom);

                //Adds exits which are possible excluding the ones that aren't
                foreach (var exitDirection in new[] { "north", "south", "east", "west" })
                {
                    if (!roomToDiscover.BlockedDirections.Contains(exitDirection))
                    {
                        (int exitX, int exitY) = GetCoordinatesInDirection(newX, newY, exitDirection);
                        var adjacentRoom = GetDiscoveredRoom(exitX, exitY);
                        if (adjacentRoom != null)
                        {
                            newRoom.Exits[exitDirection] = adjacentRoom;
                            adjacentRoom.Exits[OppositeDirection(exitDirection)] = newRoom;
                        }
                    }
                }

                //removes room from litst roomsToDiscover since it's discoverd
                roomsToDiscover.Remove(roomToDiscover);
            }
            else
            {
                //if room wasn't on the list to be discoverd then create room without any restrictions
                newRoom = new Room(newX, newY);
                AddDiscoveredRoom(newRoom);
            }

            //connection with the previous room
            Room currentRoom = GetDiscoveredRoom(currentX, currentY);
            if (currentRoom != null)
            {
                currentRoom.Exits[direction] = newRoom;
                newRoom.Exits[OppositeDirection(direction)] = currentRoom;
            }

            return newRoom;
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
    }
}
