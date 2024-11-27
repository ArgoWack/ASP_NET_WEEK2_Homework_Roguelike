
namespace ASP_NET_WEEK3_Homework_Roguelike.Model.Items
{
    [ItemType("HealthPotion")]
    public class HealthPotion : Item
    {
        
        public int Weight { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public int Quantity { get; set; }
        public int MoneyWorth { get; set; }
        
        public int HealingAmount { get; set; }
        public int MaxStackSize { get; set; } // Max potions per stack
    }
}