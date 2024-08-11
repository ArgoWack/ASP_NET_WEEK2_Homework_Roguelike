using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class FindItemEvent : RandomEvent
    {
        public override void Execute(PlayerCharacter player, Room room)
        {
            //Generating random item
            var itemType = GetRandomItemType();

            Item item = itemType switch
            {
                "SwordOneHanded" => ItemFactory.GenerateItem<SwordOneHanded>(),
                "SwordTwoHanded" => ItemFactory.GenerateItem<SwordTwoHanded>(),
                "Armor" => ItemFactory.GenerateItem<Armor>(),
                "Boots" => ItemFactory.GenerateItem<Boots>(),
                "Helmet" => ItemFactory.GenerateItem<Helmet>(),
                "HealthPotion" => ItemFactory.GenerateItem<HealthPotion>(),
                "Shield" => ItemFactory.GenerateItem<Shield>(),
                "Trousers" => ItemFactory.GenerateItem<Trousers>(),
                _ => ItemFactory.GenerateItem<HealthPotion>(),
            };

            WriteLine($"You have found an item: {item.Name}. Would you like to take it? (y/n)");
            string choice = ReadLine();
            if (choice.ToLower() == "y")
            {
                player.Inventory.Add(item);
                WriteLine($"You have taken the item: {item.Name}.");
                room.EventStatus = "none";
            }
            else
            {
                WriteLine($"You have left the item: {item.Name}.");
            }
        }

        private string GetRandomItemType()
        {
            var itemTypes = new[] { "SwordOneHanded", "SwordTwoHanded", "Armor","Boots", "Helmet","HealthPotion", "Shield", "Trousers" };
            var random = new Random();
            return itemTypes[random.Next(itemTypes.Length)];
        }
    }
}
