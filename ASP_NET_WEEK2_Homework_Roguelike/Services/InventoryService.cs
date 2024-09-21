using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using System;
using System.Linq;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class InventoryService
    {
        public void EquipItem(PlayerCharacter player, int itemId)
        {
            var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
                throw new InvalidOperationException("Item not found in inventory.");

            // Logic to equip item based on item type
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

            // Update the player's stats after equipping the item
            player.UpdateStats();
        }

        public void UnequipItem(PlayerCharacter player, Type itemType)
        {
            // Logic to unequip based on item type
            if (itemType == typeof(Helmet))
                player.EquippedHelmet = null;
            else if (itemType == typeof(Armor))
                player.EquippedArmor = null;
            else if (itemType == typeof(Shield))
                player.EquippedShield = null;
            else if (itemType == typeof(Gloves))
                player.EquippedGloves = null;
            else if (itemType == typeof(Trousers))
                player.EquippedTrousers = null;
            else if (itemType == typeof(Boots))
                player.EquippedBoots = null;
            else if (itemType == typeof(Amulet))
                player.EquippedAmulet = null;
            else if (itemType == typeof(SwordOneHanded))
                player.EquippedSwordOneHanded = null;
            else if (itemType == typeof(SwordTwoHanded))
                player.EquippedSwordTwoHanded = null;
            else
                throw new InvalidOperationException("Cannot unequip this type of item.");

            // Update the player's stats after unequipping the item
            player.UpdateStats();
        }

        public void DiscardItem(PlayerCharacter player, int itemId)
        {
            var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
                throw new InvalidOperationException("Item not found in inventory.");

            player.Inventory.Remove(item);

            // Update stats after discarding the item
            player.UpdateStats();
        }
    }
}