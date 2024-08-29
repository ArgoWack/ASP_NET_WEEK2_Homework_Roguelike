using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
using static System.Console;
namespace ASP_NET_WEEK2_Homework_Roguelike.ItemKinds
{
    public static class ItemFactory
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

        // Static property to keep track of the last generated item ID.
        // This ensures each item has a unique ID.
        public static int LastGeneratedItemId { get; set; } = 0;

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

        public static T GenerateItem<T>() where T : Item, new()
        {
            var baseStats = ItemStats.BaseStats[typeof(T)];
            var item = new T();
            var percentage = 0.0;

            // Loop through each property of the item type (e.g., Weight, Attack, Defense).

            foreach (var property in typeof(T).GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    var baseValue = (int)typeof(ItemBaseStats).GetProperty(property.Name)?.GetValue(baseStats);
                    if (baseValue != 0)
                    {
                        var finalValue = GenerateStat(baseValue, out percentage);
                        WriteLine($" {property.Name} = {finalValue}");
                        property.SetValue(item, finalValue);
                    }
                    else
                    {
                        WriteLine($"Base value for {property.Name} is 0");
                    }
                }
            }

            // Set the item name based on its quality and type
            var itemTypeAttribute = typeof(T).GetCustomAttribute<ItemTypeAttribute>();
            if (itemTypeAttribute != null)
            {
                var quality = GetQuality(percentage);
                item.Name = $"{quality} {itemTypeAttribute.Kind}";
            }
            item.ID = ++LastGeneratedItemId;
            return item;
        }

        public static Item GenerateItem(Type itemType, int weight, int defense, int attack, int moneyWorth)
        {
            // Create an instance of the item type.
            var item = (Item)Activator.CreateInstance(itemType);
            item.Weight = weight;
            item.Defense = defense;
            item.Attack = attack;
            item.MoneyWorth = moneyWorth;

            var percentage = CalculatePercentage(attack, defense, weight, itemType);
            var quality = GetQuality(percentage);

            var itemTypeAttribute = itemType.GetCustomAttribute<ItemTypeAttribute>();
            if (itemTypeAttribute != null)
            {
                item.Name = $"{quality} {itemTypeAttribute.Kind}";
            }

            item.ID = ++LastGeneratedItemId;
            return item;
        }

        private static double CalculatePercentage(int attack, int defense, int weight, Type itemType)
        {
            var baseStats = ItemStats.BaseStats[itemType];
            double attackPercentage = baseStats.Attack != 0 ? (double)(attack - baseStats.Attack) / baseStats.Attack : 0;
            double defensePercentage = baseStats.Defense != 0 ? (double)(defense - baseStats.Defense) / baseStats.Defense : 0;
            double weightPercentage = baseStats.Weight != 0 ? (double)(weight - baseStats.Weight) / baseStats.Weight : 0;

            // Simple average of the three percentages
            return (attackPercentage + defensePercentage + weightPercentage) / 3;
        }

        private static int GenerateStat(int baseValue, out double percentage)
        {
            var variation = baseValue * 0.25;
            var offset = (_random.NextDouble() * 2 - 1) * variation;
            var finalValue = baseValue + (int)offset;
            percentage = offset / baseValue;
            return finalValue;
        }
    }
}
