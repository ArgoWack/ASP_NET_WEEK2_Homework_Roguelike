using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public interface IMapService
    {
        void InitializeStartingRoom(Map map);
        void AddDiscoveredRoom(Map map, Room room);
        Room GenerateRoom(Map map, int currentX, int currentY, string direction);
        void MovePlayer(Map map, ref int playerX, ref int playerY, string direction);
        Room GetDiscoveredRoom(Map map, int x, int y);
    }
}