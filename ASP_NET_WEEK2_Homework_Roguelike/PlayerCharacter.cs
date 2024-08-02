using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    public class PlayerCharacter
    {
        public string Name { get; set; }
        public Helmet EquippedHelmet { get; set; }
        public Armor EquippedArmor { get; set; }


        public PlayerCharacter()
        {

        }

        public void EquipItem(Item item)
        {
            switch (item)
            {
                case Helmet helmet:
                    EquippedHelmet = helmet;
                    break;
                case Armor armor:
                    EquippedArmor = armor;
                    break;

                default:
                    throw new InvalidOperationException("Item type not supported");
            }
        }

        public void UnequipItem(Type itemType)
        {
            if (itemType == typeof(Helmet))
                EquippedHelmet = null;
            else if (itemType == typeof(Armor))
                EquippedArmor = null;

        }

    }
}
