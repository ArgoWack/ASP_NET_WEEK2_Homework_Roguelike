using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.View
{
    public interface IPlayerCharacterView
    {
        void ShowMap(Map map, PlayerCharacter player);
        void ShowCharacterStats(PlayerCharacter player);
        void DisplayInventory(PlayerCharacter player);
        void ShowEquipItemSuccess(string itemName);
        void ShowDiscardItemSuccess(string itemName);
        void ShowPlayerMovement(string direction, int currentX, int currentY);
        void ShowEventEncounter(string eventType);
        void ShowError(string message);
        void RelayMessage(string message);
        void ShowItemGenerated(string message);
    }
}