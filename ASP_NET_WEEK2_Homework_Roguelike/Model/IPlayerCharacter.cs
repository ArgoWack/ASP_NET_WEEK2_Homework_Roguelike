using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK3_Homework_Roguelike.Model
{
    public interface IPlayerCharacter
    {
        string Name { get; set; }
        int Health { get; set; }
        int Money { get; set; }
        int Level { get; set; }
        int Experience { get; set; }
        int CurrentX { get; set; }
        int CurrentY { get; set; }
        List<Item> Inventory { get; set; }

        void EquipItem(int itemId);
        void UnequipItem(ItemType itemType);
        void DiscardItem(int itemId);
        void Heal(int amount);
        void GetExperience(int amount);
        void ReceiveHealthPotion(int quantity = 1);
        void HealByPotion();
    }
}