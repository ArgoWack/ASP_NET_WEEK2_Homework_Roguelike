﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
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
        public static Item GenerateRandomItem()
        {
            if (!itemTypes.Any())
            {
                return null;
            }

            var randomType = itemTypes[_random.Next(itemTypes.Count)];
            return GenerateItem(randomType);
        }

        // Generate a specific item type
        public static Item GenerateItem(Type itemType)
        {
            if (!typeof(Item).IsAssignableFrom(itemType) || itemType.IsAbstract || itemType.GetConstructor(Type.EmptyTypes) == null)
            {
                Console.WriteLine($"Invalid item type: {itemType}"); // Debugging log
                return null;
            }

            if (!ItemStats.BaseStats.TryGetValue(itemType, out var baseStats))
            {
                Console.WriteLine($"Base stats not found for item type: {itemType}"); // Debugging log
                return null;
            }

            try
            {
                // Special handling for HealthPotion
                if (itemType == typeof(HealthPotion))
                {
                    var potion = new HealthPotion
                    {
                        StackSize = 10,
                        HealingAmount = 25,
                        Quantity = 1,
                        Weight = 1,
                        Name = "HealthPotion",
                        ID = ++LastGeneratedItemId,
                        MoneyWorth = 40
                    };
                    return potion;
                }

                // General item creation logic for other items
                var item = (Item)Activator.CreateInstance(itemType);

                // Assign random stats and calculate the quality percentage
                double percentage = 0.0;
                AssignRandomStats(item, baseStats, ref percentage);

                // Assign a name based on quality
                AssignItemName(item, itemType, percentage);

                item.ID = ++LastGeneratedItemId;
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while generating item: {ex.Message}");
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