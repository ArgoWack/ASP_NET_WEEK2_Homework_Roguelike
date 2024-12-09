using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;
using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public interface IInventoryService
    {
        void EquipItem(PlayerCharacter player, int itemId);
        void UnequipItem(PlayerCharacter player, ItemType itemType);
        void DiscardItem(PlayerCharacter player, int itemId);
    }
}