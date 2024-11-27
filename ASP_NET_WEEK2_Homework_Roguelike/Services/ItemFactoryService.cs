using System.Reflection;
using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;
using ASP_NET_WEEK3_Homework_Roguelike.View;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public static class ItemFactoryService
    {
        public enum ItemQuality
        {
            Shitty,
            Damaged,
            Cracked,
            Typical,
            Fine,
            Great,
            Legendary
        }
        private static readonly Random _random = new Random();
        public static int LastGeneratedItemId { get; set; } = 0;

        // List of all item types available for random generation
        private static readonly List<Type> itemTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(Item).IsAssignableFrom(t) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
            .ToList();

        // Generates a random item from the available list of item types
        public static Item GenerateRandomItem(PlayerCharacterView view)
        {
            if (!itemTypes.Any())
            {
                view.ShowError("No available item types for generation.");
                return null;
            }
            var randomType = itemTypes[_random.Next(itemTypes.Count)];
            return GenerateItem(randomType, view);
        }

        // Generates a specific item type
        public static Item GenerateItem(Type itemType, PlayerCharacterView view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            if (!typeof(Item).IsAssignableFrom(itemType) || itemType.IsAbstract || itemType.GetConstructor(Type.EmptyTypes) == null)
            {
                view.ShowError($"Invalid item type: {itemType}");
                return null;
            }
            if (!ItemStats.BaseStats.TryGetValue(itemType, out var baseStats))
            {
                view.ShowError($"Base stats not found for item type: {itemType}");
                return null;
            }
            try
            {
                // Handles stackable or healing-specific items (e.g., HealthPotion)
                if (itemType == typeof(HealthPotion))
                {
                    var potion = new HealthPotion
                    {
                        Name = baseStats.Name,
                        Weight = baseStats.Weight,
                        MoneyWorth = baseStats.MoneyWorth,
                        MaxStackSize = baseStats.MaxStackSize,
                        HealingAmount = baseStats.HealingAmount,
                        Quantity = 1,
                        ID = ++LastGeneratedItemId
                    };
                    view.ShowItemGenerated($"Generated HealthPotion with ID: {potion.ID}");
                    return potion;
                }

                // General item creation logic for other items
                var item = (Item)Activator.CreateInstance(itemType);
                item.ID = ++LastGeneratedItemId;

                // Assigns stats and calculate quality
                double percentage = 0.0;
                AssignRandomStats(item, baseStats, ref percentage);
                AssignItemName(item, itemType, percentage);
                return item;
            }
            catch (Exception ex)
            {
                view.ShowError($"Exception occurred while generating item: {ex.Message}");
                return null;
            }
        }
        private static void AssignRandomStats(Item item, ItemBaseStats baseStats, ref double percentage)
        {
            foreach (var property in typeof(ItemBaseStats).GetProperties())
            {
                var itemProperty = item.GetType().GetProperty(property.Name);
                if (itemProperty != null && itemProperty.PropertyType == typeof(int) && itemProperty.CanWrite)
                {
                    var baseValue = (int?)property.GetValue(baseStats) ?? 0;
                    if (baseValue != 0)
                    {
                        var finalValue = GenerateRandomStat(baseValue, out double calculatedPercentage);
                        itemProperty.SetValue(item, finalValue);
                        percentage = calculatedPercentage;
                    }
                }
            }
        }
        private static int GenerateRandomStat(int baseValue, out double percentage)
        {
            var variation = baseValue * 0.25;
            var offset = (_random.NextDouble() * 2 - 1) * variation;
            percentage = offset / baseValue;
            return baseValue + (int)offset;
        }
        private static void AssignItemName(Item item, Type itemType, double percentage)
        {
            var quality = GetQuality(percentage);
            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();
            if (itemTypeAttribute != null)
            {
                item.Name = $"{quality} {itemTypeAttribute.Kind}";
            }
        }
        public static ItemQuality GetQuality(double percentage)
        {
            if (percentage <= -0.20) return ItemQuality.Shitty;
            if (percentage <= -0.10) return ItemQuality.Damaged;
            if (percentage <= -0.05) return ItemQuality.Cracked;
            if (percentage <= 0.05) return ItemQuality.Typical;
            if (percentage <= 0.10) return ItemQuality.Fine;
            if (percentage <= 0.20) return ItemQuality.Great;
            return ItemQuality.Legendary;
        }
    }
}