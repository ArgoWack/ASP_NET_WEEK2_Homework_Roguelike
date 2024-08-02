﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ASP_NET_WEEK2_Homework_Roguelike.Items;

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

            foreach (var property in typeof(T).GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    var baseValue = (int)typeof(ItemBaseStats).GetProperty(property.Name)?.GetValue(baseStats);
                    var finalValue = GenerateStat(baseValue, out percentage);
                    property.SetValue(item, finalValue);
                }
            }

            var itemTypeAttribute = typeof(T).GetCustomAttribute<ItemTypeAttribute>();
            if (itemTypeAttribute != null)
            {
                var quality = GetQuality(percentage);
                item.Name = $"{quality} {itemTypeAttribute.Kind}";
            }

            return item;
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