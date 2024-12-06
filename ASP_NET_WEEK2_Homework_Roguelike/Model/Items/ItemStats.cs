namespace ASP_NET_WEEK3_Homework_Roguelike.Model.Items
{
    public static class ItemStats
    {
        public static readonly Dictionary<ItemType, ItemBaseStats> BaseStats = new Dictionary<ItemType, ItemBaseStats>
        {
            { ItemType.Helmet, new ItemBaseStats { Weight = 5, Defense = 10, Attack = 0, MoneyWorth = 4 } },
            { ItemType.Armor, new ItemBaseStats { Weight = 50, Defense = 40, Attack = 0, MoneyWorth = 12 } },
            { ItemType.SwordOneHanded, new ItemBaseStats { Weight = 10, Defense = 10, Attack = 30, MoneyWorth = 15 } },
            { ItemType.SwordTwoHanded, new ItemBaseStats { Weight = 15, Defense = 40, Attack = 50, MoneyWorth = 25 } },
            { ItemType.Shield, new ItemBaseStats { Weight = 15, Defense = 30, Attack = 0, MoneyWorth = 8 } },
            { ItemType.Gloves, new ItemBaseStats { Weight = 4, Defense = 7, Attack = 0, MoneyWorth = 3 } },
            { ItemType.Trousers, new ItemBaseStats { Weight = 8, Defense = 12, Attack = 0, MoneyWorth = 5 } },
            { ItemType.Boots, new ItemBaseStats { Weight = 5, Defense = 5, Attack = 0, MoneyWorth = 4 } },
            { ItemType.Amulet, new ItemBaseStats { Weight = 0, Defense = 1, Attack = 4, MoneyWorth = 10 } },
            { ItemType.HealthPotion, new ItemBaseStats { Name = "HealthPotion", Attack = 0, Defense = 0, MoneyWorth = 40, Weight = 1, MaxStackSize = 10, HealingAmount = 40 } }
        };
    }
}