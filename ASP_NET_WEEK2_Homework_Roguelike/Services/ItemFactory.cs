using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using System;
using System.Reflection;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public static class ItemFactoryService
    {
        // Enumeration for item quality
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

        // Static property to keep track of the last generated item ID.
        public static int LastGeneratedItemId { get; set; } = 0;

        // Public method to generate an item of a specific type
        public static T GenerateItem<T>() where T : Item, new()
        {
            var baseStats = ItemStats.BaseStats[typeof(T)];
            var item = new T();
            double percentage = 0.0;

            // Assign random stats to the item based on its base stats
            AssignRandomStats(item, baseStats, ref percentage);

            // Set the item name based on its quality and type
            AssignItemName(item, typeof(T), percentage);
            item.ID = ++LastGeneratedItemId;
            return item;
        }

        // Public method to generate an item with specific stats
        public static Item GenerateItem(Type itemType, int weight, int defense, int attack, int moneyWorth)
        {
            var item = (Item)Activator.CreateInstance(itemType);
            item.Weight = weight;
            item.Defense = defense;
            item.Attack = attack;
            item.MoneyWorth = moneyWorth;

            var percentage = CalculateQualityPercentage(attack, defense, weight, itemType);
            AssignItemName(item, itemType, percentage);

            item.ID = ++LastGeneratedItemId;
            return item;
        }

        // Method to assign random stats to the item
        private static void AssignRandomStats(Item item, ItemBaseStats baseStats, ref double percentage)
        {
            foreach (var property in item.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    var baseValue = (int?)property.GetValue(baseStats) ?? 0;
                    if (baseValue != 0)
                    {
                        var finalValue = GenerateRandomStat(baseValue, out double calculatedPercentage);
                        property.SetValue(item, finalValue);
                        percentage = calculatedPercentage; // Adjust if multiple stats affect quality calculation
                    }
                }
            }
        }

        // Method to generate a random stat value based on a base value
        private static int GenerateRandomStat(int baseValue, out double percentage)
        {
            var variation = baseValue * 0.25;
            var offset = (_random.NextDouble() * 2 - 1) * variation;
            percentage = offset / baseValue;
            return baseValue + (int)offset;
        }

        // Method to calculate item quality based on stats
        private static double CalculateQualityPercentage(int attack, int defense, int weight, Type itemType)
        {
            var baseStats = ItemStats.BaseStats[itemType];
            double attackPercentage = baseStats.Attack != 0 ? (double)(attack - baseStats.Attack) / baseStats.Attack : 0;
            double defensePercentage = baseStats.Defense != 0 ? (double)(defense - baseStats.Defense) / baseStats.Defense : 0;
            double weightPercentage = baseStats.Weight != 0 ? (double)(weight - baseStats.Weight) / baseStats.Weight : 0;

            return (attackPercentage + defensePercentage + weightPercentage) / 3;
        }

        // Method to assign the item name based on quality and type
        private static void AssignItemName(Item item, Type itemType, double percentage)
        {
            var quality = GetQuality(percentage);
            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();
            if (itemTypeAttribute != null)
            {
                item.Name = $"{quality} {itemTypeAttribute.Kind}";
            }
        }

        // Method to determine item quality based on calculated percentage
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