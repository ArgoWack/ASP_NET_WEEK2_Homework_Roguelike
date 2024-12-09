using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Controller
{
    public interface IPlayerCharacterController
    {
        PlayerCharacter PlayerCharacter { get; }
        void ShowCharacterStats();
        void ShowMap();
        void MovePlayer(string direction);
        void EquipItem(int itemId);
        void DiscardItem(int itemId);
        void ShowInventory();
    }
}