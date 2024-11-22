using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Items
{
    public abstract class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public int MoneyWorth { get; set; }
        public string Description { get; set; }
        public string ItemType => GetType().Name;
        public int Quantity { get; set; } = 1; // Default for stackable items

    }
}