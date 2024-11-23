
namespace ASP_NET_WEEK2_Homework_Roguelike.Model
{
    public class Map
    {
        // stores discovered rooms with their coordinates as keys
        public Dictionary<(int, int), Room> DiscoveredRooms { get; set; }
        // stores rooms that are yet to be discovered
        public List<RoomToDiscover> RoomsToDiscover { get; set; }
        public Map()
        {
            DiscoveredRooms = new Dictionary<(int, int), Room>();
            RoomsToDiscover = new List<RoomToDiscover>();
        }
    }
}