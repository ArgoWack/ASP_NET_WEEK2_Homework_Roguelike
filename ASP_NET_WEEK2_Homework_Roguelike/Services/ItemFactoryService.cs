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

        // List of item types (from ItemType enum) for generation
        private static readonly List<ItemType> itemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList();

        // Generates a random item from the list of item types
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
        public static Item GenerateItem(ItemType itemType, PlayerCharacterView view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (!ItemStats.BaseStats.TryGetValue(itemType, out var baseStats))
            {
                view.ShowError($"Base stats not found for item type: {itemType}");
                return null;
            }

            try
            {
                if (itemType == ItemType.HealthPotion)
                {
                    var potion = new HealthPotion
                    {
                        Name = baseStats.Name,
                        Weight = baseStats.Weight,
                        MoneyWorth = baseStats.MoneyWorth,
                        MaxStackSize = baseStats.MaxStackSize,
                        HealingAmount = baseStats.HealingAmount,
                        Type = itemType,
                        Quantity = 1,
                        ID = ++LastGeneratedItemId
                    };
                    view.ShowItemGenerated($"Generated HealthPotion with ID: {potion.ID}");
                    return potion;
                }

                // General item creation logic
                var item = new Item
                {
                    Type = itemType,
                    ID = ++LastGeneratedItemId
                };

                // Assign random stats and calculate quality
                double percentage = 0.0;
                AssignRandomStats(item, baseStats, ref percentage);
                AssignItemName(item, percentage);
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
        private static void AssignItemName(Item item, double percentage)
        {
            var quality = GetQuality(percentage);
            item.Name = $"{quality} {item.Type}";
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