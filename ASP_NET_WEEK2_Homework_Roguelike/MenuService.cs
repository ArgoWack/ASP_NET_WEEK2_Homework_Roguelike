using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    internal class MenuService
    {
        List<MenuAction> MenuAction = new List<MenuAction>();
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
        
        public int AddNewMenu(char itemType)
        {
            int itemTypeId;
            Int32.TryParse(itemType.ToString(), out itemTypeId);
            MenuAction MenuAction = new MenuAction();
            WriteLine("Please enter id for new Item");
            var id = ReadLine();

            int itemId;
            Int32.TryParse(itemType.ToString(), out itemId);
            WriteLine("Please enter id for new Item");
            var name = ReadLine();

            MenuAction.Id = itemId;
            MenuAction.Name = name;

            //MenuAction.AddNewAction(MenuAction);
            return itemId;
        }
        
    }
}
