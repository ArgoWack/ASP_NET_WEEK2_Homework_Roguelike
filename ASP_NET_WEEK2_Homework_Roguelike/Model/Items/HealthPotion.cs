namespace ASP_NET_WEEK3_Homework_Roguelike.Model.Items
{
    [ItemType("HealthPotion")]
    public class HealthPotion : Item
    {
        public int HealingAmount { get; set; }
        public int MaxStackSize { get; set; } // Max potions per stack
    }
}