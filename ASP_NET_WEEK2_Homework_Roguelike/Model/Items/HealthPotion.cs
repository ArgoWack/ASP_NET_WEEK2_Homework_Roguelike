using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Items
{
    [ItemType("HealthPotion")]
    public class HealthPotion : Item
    {
        /*
        public int Weight { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public int Quantity { get; set; }
        public int MoneyWorth { get; set; }
        */
        public int HealingAmount { get; set; }
        public int MaxStackSize { get; set; } // Max potions per stack
    }
}
