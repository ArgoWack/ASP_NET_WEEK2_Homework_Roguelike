using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Items
{
    public static class ItemStats
    {
        public static readonly Dictionary<Type, ItemBaseStats> BaseStats = new Dictionary<Type, ItemBaseStats>
        {
            { typeof(Helmet), new ItemBaseStats { Weight=5, Defense = 10,Attack=0,MoneyWorth=4 } },
            { typeof(Armor), new ItemBaseStats { Weight=50, Defense = 40,Attack=0,MoneyWorth=12 } },
            { typeof(SwordOneHanded), new ItemBaseStats { Weight=10, Defense = 10,Attack=30,MoneyWorth=15 } },
            { typeof(SwordTwoHanded), new ItemBaseStats { Weight=15, Defense = 40,Attack=50,MoneyWorth=25 } },
            { typeof(Shield), new ItemBaseStats { Weight=15, Defense = 30,Attack=0,MoneyWorth=8 } },
            { typeof(Gloves), new ItemBaseStats { Weight=4, Defense = 7,Attack=0,MoneyWorth=3 } },
            { typeof(Trousers), new ItemBaseStats { Weight=8, Defense = 12,Attack=0,MoneyWorth=5 } },
            { typeof(Boots), new ItemBaseStats { Weight=5, Defense = 5,Attack=0,MoneyWorth=4 } },
            { typeof(Amulet), new ItemBaseStats { Weight=0, Defense = 1,Attack=4,MoneyWorth=10 } }
        };
    }

    public class ItemBaseStats
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public int MoneyWorth { get; set; }
        public string Description { get; set; }
    }
}
