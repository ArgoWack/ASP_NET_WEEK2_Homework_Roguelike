using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
using System;
using static System.Console;
using ASP_NET_WEEK2_Homework_Roguelike.Controller;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class FindItemEvent : RandomEvent
    {
        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            var item = ItemFactory.GenerateItem<SwordOneHanded>();
            controller.View.ShowEventOutcome($"You have found an item: {item.Name}. Would you like to take it? (y/n)");

            string choice = ReadLine();
            if (choice.ToLower() == "y")
            {
                player.Inventory.Add(item);
                controller.View.ShowEventOutcome($"You have taken the item: {item.Name}.");
            }
            else
            {
                controller.View.ShowEventOutcome($"You have left the item: {item.Name}.");
            }

            // Clear the event after execution
            room.EventStatus = "none";
        }
    }
}
