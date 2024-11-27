using ASP_NET_WEEK3_Homework_Roguelike.Model;
using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public class InventoryService
    {
        public void EquipItem(PlayerCharacter player, int itemId)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
                throw new InvalidOperationException("Item not found in inventory.");
            switch (item)
            {
                case Helmet helmet:
                    player.EquippedHelmet = helmet;
                    break;
                case Armor armor:
                    player.EquippedArmor = armor;
                    break;
                case Shield shield:
                    player.EquippedShield = shield;
                    break;
                case Gloves gloves:
                    player.EquippedGloves = gloves;
                    break;
                case Trousers trousers:
                    player.EquippedTrousers = trousers;
                    break;
                case Boots boots:
                    player.EquippedBoots = boots;
                    break;
                case Amulet amulet:
                    player.EquippedAmulet = amulet;
                    break;
                case SwordOneHanded swordOneHanded:
                    player.EquippedSwordOneHanded = swordOneHanded;
                    break;
                case SwordTwoHanded swordTwoHanded:
                    player.EquippedSwordTwoHanded = swordTwoHanded;
                    break;
                default:
                    throw new InvalidOperationException("Cannot equip this type of item.");
            }
            // ensures player stats are updated after equipping an item
            player.UpdateStats();
        }
        public void UnequipItem(PlayerCharacter player, Type itemType)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (itemType == null)
                throw new ArgumentNullException(nameof(itemType));
            if (itemType == typeof(Helmet)) player.EquippedHelmet = null;
            else if (itemType == typeof(Armor)) player.EquippedArmor = null;
            else if (itemType == typeof(Shield)) player.EquippedShield = null;
            else if (itemType == typeof(Gloves)) player.EquippedGloves = null;
            else if (itemType == typeof(Trousers)) player.EquippedTrousers = null;
            else if (itemType == typeof(Boots)) player.EquippedBoots = null;
            else if (itemType == typeof(Amulet)) player.EquippedAmulet = null;
            else if (itemType == typeof(SwordOneHanded)) player.EquippedSwordOneHanded = null;
            else if (itemType == typeof(SwordTwoHanded)) player.EquippedSwordTwoHanded = null;
            else
                throw new InvalidOperationException("Cannot unequip this type of item.");
            // ensures player stats are updated after unequipping an item
            player.UpdateStats();
        }
        public void DiscardItem(PlayerCharacter player, int itemId)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
                throw new InvalidOperationException("Item not found in inventory.");
            if (item is HealthPotion potion)
            {
                potion.Quantity--;
                if (potion.Quantity == 0)
                    player.Inventory.Remove(potion);
            }
            else
            {
                player.UnequipItem(item.GetType());
                player.Inventory.Remove(item);
            }
            // ensures player stats are updated after discarding an item
            player.UpdateStats();
        }
    }
}