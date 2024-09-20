using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using System;
using System.Linq;
using System.Reflection;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class InventoryService
    {
        private readonly CharacterStatsService _statsService;

        public InventoryService()
        {
            _statsService = new CharacterStatsService();
        }

        // Equip an item by ID
        public void EquipItem(PlayerCharacter player, int itemId)
        {
            var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
            {
                throw new InvalidOperationException("Item not found in inventory.");
            }

            var itemType = item.GetType();
            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();

            if (itemTypeAttribute != null)
            {
                HandleTwoHandedWeaponEquipping(player, item);

                // Unequip the current item of the same type before equipping a new one
                UnequipItem(player, itemType);

                var property = player.GetType().GetProperty($"Equipped{itemTypeAttribute.Kind}");
                if (property != null)
                {
                    property.SetValue(player, item);
                    UpdatePlayerStats(player);
                }
                else
                {
                    throw new InvalidOperationException($"No equipped property found for {itemTypeAttribute.Kind}");
                }
            }
            else
            {
                throw new InvalidOperationException("Item type not supported.");
            }
        }

        // Handle specific logic for equipping two-handed weapons
        private void HandleTwoHandedWeaponEquipping(PlayerCharacter player, Item item)
        {
            if (item is SwordTwoHanded)
            {
                UnequipItem(player, typeof(SwordOneHanded));
                UnequipItem(player, typeof(Shield));
            }
            else if (item is SwordOneHanded || item is Shield)
            {
                UnequipItem(player, typeof(SwordTwoHanded));
            }
        }

        // Unequip an item of a specific type
        public void UnequipItem(PlayerCharacter player, Type itemType)
        {
            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();
            if (itemTypeAttribute != null)
            {
                var property = player.GetType().GetProperty($"Equipped{itemTypeAttribute.Kind}");
                if (property != null)
                {
                    property.SetValue(player, null);
                    UpdatePlayerStats(player);
                }
                else
                {
                    throw new InvalidOperationException($"No equipped property found for {itemTypeAttribute.Kind}");
                }
            }
        }

        // Discard an item from the inventory by ID
        public void DiscardItem(PlayerCharacter player, int itemId)
        {
            var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);

            if (item == null)
            {
                throw new InvalidOperationException("Item not found in inventory.");
            }

            player.Inventory.Remove(item);

            UnequipItemIfEquipped(player, item);

            UpdatePlayerStats(player);
        }

        // Check if the item being discarded is currently equipped and unequip it
        private void UnequipItemIfEquipped(PlayerCharacter player, Item item)
        {
            if (player.EquippedAmulet?.ID == item.ID) player.EquippedAmulet = null;
            if (player.EquippedArmor?.ID == item.ID) player.EquippedArmor = null;
            if (player.EquippedBoots?.ID == item.ID) player.EquippedBoots = null;
            if (player.EquippedGloves?.ID == item.ID) player.EquippedGloves = null;
            if (player.EquippedHelmet?.ID == item.ID) player.EquippedHelmet = null;
            if (player.EquippedShield?.ID == item.ID) player.EquippedShield = null;
            if (player.EquippedSwordOneHanded?.ID == item.ID) player.EquippedSwordOneHanded = null;
            if (player.EquippedSwordTwoHanded?.ID == item.ID) player.EquippedSwordTwoHanded = null;
            if (player.EquippedTrousers?.ID == item.ID) player.EquippedTrousers = null;
        }

        // Update the player's stats after equipping or discarding items
        private void UpdatePlayerStats(PlayerCharacter player)
        {
            // Use CharacterStatsService to calculate and directly update player's stats
            player.SetBaseAttack(_statsService.CalculateAttack(player));
            player.SetBaseDefense(_statsService.CalculateDefense(player));
            player.SetWeight(_statsService.CalculateWeight(player));
        }
    }
}