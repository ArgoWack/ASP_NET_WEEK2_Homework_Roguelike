using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class DialogEvent : RandomEvent
    {
        public override void Execute(PlayerCharacter player, Room room)
        {
            // Generate a random event type
            string eventType = GetRandomEventType();

            switch (eventType)
            {
                case "WiseTraveler":
                    ExecuteWiseTravelerEvent(player);
                    break;
                case "Monk":
                    ExecuteMonkEvent(player);
                    break;
                case "Witch":
                    ExecuteWitchEvent(player);
                    break;
                case "Merchant":
                    ExecuteMerchantEvent(player);
                    break;
                default:
                    WriteLine("You encounter a mysterious stranger who says nothing and disappears.");
                    break;
            }

            // Mark the event as completed
            room.EventStatus = "none";
        }

        private string GetRandomEventType()
        {
            var eventTypes = new[] { "WiseTraveler", "Monk", "Witch", "Merchant" };
            var random = new Random();
            return eventTypes[random.Next(eventTypes.Length)];
        }

        private void ExecuteWiseTravelerEvent(PlayerCharacter player)
        {
            WriteLine("You encounter a wise traveler who offers you advice.");

            // Randomly select a buff to apply
            var random = new Random();
            int buffType = random.Next(4);

            switch (buffType)
            {
                case 0:
                    WriteLine("You have a meaningful conversation and gain wisdom.");
                    WriteLine("You get 200 experience.");
                    player.Experience += 200;
                    break;
                case 1:
                    WriteLine("You are taught how to move swiftly.");
                    WriteLine("Your speed improves by 5.");
                    player.ModifySpeed(5);
                    break;
                case 2:
                    WriteLine("You are taught how to attack better.");
                    WriteLine("Your attack improves by 5.");
                    player.ModifyAttack(5);
                    break;
                case 3:
                    WriteLine("You are taught how to defend better.");
                    WriteLine("Your defense improves by 5.");
                    player.ModifyDefense(5);
                    break;
            }
        }

        private void ExecuteMonkEvent(PlayerCharacter player)
        {
            WriteLine("A monk approaches you and heals your wounds fully.");
            player.Heal(1000); //assuming 1000 should be more than enaugh
        }

        private void ExecuteWitchEvent(PlayerCharacter player)
        {
            WriteLine("You got cursed by the witch.");
            WriteLine("Your health gets drained to half, and your speed, attack, and defense get reduced by 2.");
            player.Health = player.Health / 2;
            player.ModifySpeed(-2);
            player.ModifyAttack(-2);
            player.ModifyDefense(-2);
        }

        private void ExecuteMerchantEvent(PlayerCharacter player)
        {
            WriteLine("A merchant approaches you and shows his stock.");

            string choice;
            do
            {
                WriteLine("\nWrite: \nb. Buy health potion for 40 \ns. Sell an item \nl. Leave");
                choice = ReadLine().ToLower();

                if (choice == "b")
                {
                    player.BuyHealthPotion();
                }
                else if (choice == "s")
                {
                    player.CheckInventory();
                    WriteLine("Enter the ID of the item you want to sell:");
                    if (int.TryParse(ReadLine(), out int itemId))
                    {
                        player.SellItem(itemId);
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
