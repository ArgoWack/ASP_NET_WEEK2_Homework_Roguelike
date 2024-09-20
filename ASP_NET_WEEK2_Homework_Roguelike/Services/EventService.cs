using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using System;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class EventService
    {
        private readonly PlayerCharacterController _playerController;

        public EventService(PlayerCharacterController playerController)
        {
            _playerController = playerController;
        }

        public void HandleEventOutcome(string outcome)
        {
            Console.WriteLine(outcome);
        }

        // realted to FindItemEvent
        public string PromptForItemPickup()
        {
            Console.WriteLine("Would you like to take the item? (y/n)");
            string choice = Console.ReadLine();
            return choice;
        }

        // realted to MonsterEvent
        public string GetMonsterOptions()
        {
            Console.WriteLine("\nChoose an action: \nf - Fight \nh - Heal \nl - Leave/Flee");
            string choice = Console.ReadLine();
            return choice;
        }

        // realted to DialogEvent
        public string GetMerchantOptions()
        {
            Console.WriteLine("Write: \nb - Buy health potion for 40 coins \ns - Sell an item \nl - Leave");
            string choice = Console.ReadLine();
            return choice;
        }

        //// realted to merchant interaction
        public int? PromptForItemIdToSell()
        {
            Console.WriteLine("Enter the ID of the item you want to sell:");
            if (int.TryParse(Console.ReadLine(), out int itemId))
            {
                return itemId;
            }
            return null;
        }

        public void BuyHealthPotion(PlayerCharacter player)
        {
            try
            {
                player.BuyHealthPotion();
                HandleEventOutcome("You bought a health potion for 40 coins.");
            }
            catch (InvalidOperationException ex)
            {
                HandleEventOutcome(ex.Message);
            }
        }

        public void SellItem(PlayerCharacter player, int itemId)
        {
            try
            {
                player.SellItem(itemId);
                HandleEventOutcome($"You sold item with ID {itemId}.");
            }
            catch (InvalidOperationException ex)
            {
                HandleEventOutcome(ex.Message);
            }
        }

        public void HealPlayer(PlayerCharacter player, int amount)
        {
            player.Heal(amount);
            HandleEventOutcome($"You have been healed by {amount} points.");
        }

        public void ModifyPlayerStats(PlayerCharacter player, string stat, int amount)
        {
            switch (stat)
            {
                case "experience":
                    player.Experience += amount;
                    HandleEventOutcome($"You gained {amount} experience.");
                    break;
                case "speed":
                    player.ModifySpeed(amount);
                    HandleEventOutcome($"Your speed has been modified by {amount}.");
                    break;
                case "attack":
                    player.ModifyAttack(amount);
                    HandleEventOutcome($"Your attack has been modified by {amount}.");
                    break;
                case "defense":
                    player.ModifyDefense(amount);
                    HandleEventOutcome($"Your defense has been modified by {amount}.");
                    break;
                default:
                    HandleEventOutcome("Invalid stat modification.");
                    break;
            }
        }
    }
}