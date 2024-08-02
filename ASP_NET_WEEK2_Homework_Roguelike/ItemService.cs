using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    internal class ItemService
    {
        public List<Item> Items { get; set; }

        public ItemService() 
        {
            //Item = new List<Item>();
        }

        public ConsoleKeyInfo AddNewItemView(MenuActionService actionService)
        {
            WriteLine("Please Select item type");
            var AddNewItemMenu = actionService.GetMenuActionsByMenuName("AddNewItemMenu");
            for (int i = 0; i < AddNewItemMenu.Count; i++)
            {
                WriteLine($"{AddNewItemMenu[i].Id}. {AddNewItemMenu[i].Name}");
            }

            var operation = ReadKey();
            return operation;
        }
        /*
        public int AddNewItem(char itemType)
        {
            int itemTypeId;
            Int32.TryParse(itemType.ToString(), out itemTypeId);
            Item item = new Item();
            item.TypeId = itemTypeId;
            WriteLine("Please enter id for new Item");
            var id = ReadLine();

            int itemId;
            Int32.TryParse(itemType.ToString(), out itemId);
            WriteLine("Please enter id for new Item");
            var name = ReadLine();

            item.Id = itemId;
            item.Name = name;

            Items.Add(item);
            return itemId;
        }
        */
    }
}
