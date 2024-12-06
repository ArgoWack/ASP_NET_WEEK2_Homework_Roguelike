namespace ASP_NET_WEEK3_Homework_Roguelike.Model.Items
{
    public class ItemBaseStats
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public int MoneyWorth { get; set; }
        public string Description { get; set; }
        public int MaxStackSize { get; set; } // Optional, for stackable items
        public int HealingAmount { get; set; } // Optional, for healing items
    }
}