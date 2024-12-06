using ASP_NET_WEEK3_Homework_Roguelike.Model;
using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;
using System.Reflection;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public class InventoryService
    {
        public void EquipItem(PlayerCharacter player, int itemId)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Player cannot be null.");

            // finds the item in the player's inventory
            var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);
            if (item == null)
                throw new InvalidOperationException($"Item with ID {itemId} not found in inventory.");

            // used reflection to ensure EquippedItems is initialized
            var equippedItemsProperty = typeof(PlayerCharacter)
                .GetProperty("EquippedItems", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (equippedItemsProperty == null)
                throw new InvalidOperationException("EquippedItems property not found in PlayerCharacter.");

            var equippedItems = equippedItemsProperty.GetValue(player) as Dictionary<ItemType, Item>;
            if (equippedItems == null)
            {
                equippedItems = new Dictionary<ItemType, Item>();
                equippedItemsProperty.SetValue(player, equippedItems);
            }

            // checks if the item type is already equipped
            if (equippedItems.TryGetValue(item.Type, out var currentlyEquippedItem) && currentlyEquippedItem != null)
            {
                // returns the currently equipped item to the inventory
                player.Inventory.Add(currentlyEquippedItem);
            }

            // equips the new item
            equippedItems[item.Type] = item;
            player.Inventory.Remove(item);

            player.UpdateStats();
        }
        public void UnequipItem(PlayerCharacter player, ItemType itemType)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            if (!player.EquippedItems.ContainsKey(itemType))
                throw new InvalidOperationException("No item of this type is equipped.");

            var item = player.EquippedItems[itemType];
            player.EquippedItems.Remove(itemType);
            player.Inventory.Add(item);

            // after unequipping
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
                // ensures unequipping the item if it's equipped
                if (player.EquippedItems.ContainsKey(item.Type) && player.EquippedItems[item.Type].ID == itemId)
                {
                    UnequipItem(player, item.Type);
                }

                player.Inventory.Remove(item);
            }

            // after discarding
            player.UpdateStats();
        }
    }
}