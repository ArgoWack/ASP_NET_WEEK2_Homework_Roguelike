﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike.Items
{
    public abstract class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public int MoneyWorth { get; set; }
        public string Description { get; set; }
    }
}