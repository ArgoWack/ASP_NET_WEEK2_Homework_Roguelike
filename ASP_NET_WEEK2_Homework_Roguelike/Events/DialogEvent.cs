using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.View;
using System;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class DialogEvent : RandomEvent
    {
        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            string eventType = GetRandomEventType();
            controller.View.ShowEventEncounter(eventType);

            switch (eventType)
            {
                case "WiseTraveler":
                    controller.View.ShowEventOutcome(ExecuteWiseTravelerEvent(player));
                    break;
                case "Monk":
                    controller.View.ShowEventOutcome(ExecuteMonkEvent(player));
                    break;
                case "Witch":
                    controller.View.ShowEventOutcome(ExecuteWitchEvent(player));
                    break;
                case "Merchant":
                    ExecuteMerchantEvent(player, controller);
                    break;
                default:
                    controller.View.ShowEventOutcome("You encounter a mysterious stranger who says nothing and disappears.");
                    break;
            }

            // Clear the event after execution
            room.EventStatus = "none";
        }

        private string GetRandomEventType()
        {
            var eventTypes = new[] { "WiseTraveler", "Monk", "Witch", "Merchant" };
            var random = new Random();
            return eventTypes[random.Next(eventTypes.Length)];
        }

        private string ExecuteWiseTravelerEvent(PlayerCharacter player)
        {
            var random = new Random();
            int buffType = random.Next(4);

            switch (buffType)
            {
                case 0:
                    player.Experience += 200;
                    return "You have a meaningful conversation and gain wisdom. You get 200 experience.";
                case 1:
                    player.ModifySpeed(5);
                    return "You are taught how to move swiftly. Your speed improves by 5.";
                case 2:
                    player.ModifyAttack(5);
                    return "You are taught how to attack better. Your attack improves by 5.";
                case 3:
                    player.ModifyDefense(5);
                    return "You are taught how to defend better. Your defense improves by 5.";
                default:
                    return "";
            }
        }

        private string ExecuteMonkEvent(PlayerCharacter player)
        {
            player.Heal(1000); // assuming 1000 should be more than enough
            return "A monk approaches you and heals your wounds fully.";
        }

        private string ExecuteWitchEvent(PlayerCharacter player)
        {
            player.Health = player.Health / 2;
            player.ModifySpeed(-2);
            player.ModifyAttack(-2);
            player.ModifyDefense(-2);
            return "You got cursed by the witch. Your health gets drained to half, and your speed, attack, and defense get reduced by 2.";
        }

        private void ExecuteMerchantEvent(PlayerCharacter player, PlayerCharacterController controller)
        {
            string choice;
            do
            {
                controller.ShowInventory();

                WriteLine("\nWrite: \nb. Buy health potion for 40 \ns. Sell an item \nl. Leave");
                choice = ReadLine().ToLower();

                if (choice == "b")
                {
                    controller.BuyHealthPotion();
                }
                else if (choice == "s")
                {
                    WriteLine("Enter the ID of the item you want to sell:");
                    if (int.TryParse(ReadLine(), out int itemId))
                    {
                        controller.SellItem(itemId);
                    }
                    else
                    {
                        WriteLine("Invalid item ID.");
                    }
                }
                else if (choice != "l")
                {
                    WriteLine("Invalid choice. Please choose 'b', 's', or 'l'.");
                }

            } while (choice != "l");

            WriteLine("The merchant nods and moves on.");
        }

    }
}
