using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using System;
using static System.Console;
using ASP_NET_WEEK2_Homework_Roguelike.Model;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class FindItemEvent : RandomEvent
    {
        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            var item = ItemFactory.GenerateItem<SwordOneHanded>();

            controller.HandleEventOutcome($"You have found an item: {item.Name}. Would you like to take it? (y/n)");

            string choice = ReadLine();
            if (choice.ToLower() == "y")
            {
                player.Inventory.Add(item);
                controller.HandleEventOutcome($"You have taken the item: {item.Name}.");
            }
            else
            {
                controller.HandleEventOutcome($"You have left the item: {item.Name}.");
            }

            // Clear the event after execution
            room.EventStatus = "none";
        }
    }
}