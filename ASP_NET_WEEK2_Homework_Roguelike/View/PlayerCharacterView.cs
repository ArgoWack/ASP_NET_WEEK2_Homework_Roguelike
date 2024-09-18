using ASP_NET_WEEK2_Homework_Roguelike;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
using System;
using System.Collections.Generic;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.View
{
    public class PlayerCharacterView
    {
        public void DisplayCharacterStats(PlayerCharacter player)
        {
            var equippedItems = new Dictionary<string, Item>
            {
                { "Amulet", player.EquippedAmulet },
                { "Armor", player.EquippedArmor },
                { "Boots", player.EquippedBoots },
                { "Gloves", player.EquippedGloves },
                { "Helmet", player.EquippedHelmet },
                { "Shield", player.EquippedShield },
                { "SwordOneHanded", player.EquippedSwordOneHanded },
                { "SwordTwoHanded", player.EquippedSwordTwoHanded },
                { "Trousers", player.EquippedTrousers }
            };

            WriteLine($@"
            Character name: {player.Name}
            Attack: {player.Attack}
            Defense: {player.Defense}
            Speed: {player.Speed}
            Weight: {player.Weight}
            Money: {player.Money}
            Health: {player.Health}
            Level: {player.Level}
            Experience: {player.Experience}");

            foreach (var equippedItem in equippedItems)
            {
                if (equippedItem.Value != null)
                {
                    WriteLine($@"
                    Equipped {equippedItem.Key}: {equippedItem.Value.Name ?? "None"} 
                    ID: {equippedItem.Value.ID}, Defense: {equippedItem.Value.Defense}, Attack: {equippedItem.Value.Attack}, Weight: {equippedItem.Value.Weight}, Money worth: {equippedItem.Value.MoneyWorth}, Description: {equippedItem.Value.Description}");
                }
                else
                {
                    WriteLine($@"
                    Equipped {equippedItem.Key}: None");
                }
            }
        }
        public void DisplayInventory(PlayerCharacter player)
        {
            WriteLine(" \n Here is your inventory: ");
            foreach (Item item in player.Inventory)
            {
                WriteLine($"This is: {item.Name} ID: {item.ID} Defense: {item.Defense} Attack: {item.Attack} Weight: {item.Weight} Money worth: {item.MoneyWorth} Description: {item.Description}");
            }
        }
        public void ShowEquipItemSuccess(string itemName)
        {
            WriteLine($"You have equipped {itemName}.");
        }
        public void ShowDiscardItemSuccess(string itemName)
        {
            WriteLine($"Item '{itemName}' has been discarded.");
        }
        public void ShowPlayerMovement(string direction, int currentX, int currentY)
        {
            WriteLine($"Moved {direction}. Current position: ({currentX}, {currentY})");
        }
        public void ShowEventEncounter(string eventType)
        {
            WriteLine($"You encounter a {eventType}!");
        }
        public void ShowEventOutcome(string outcome)
        {
            WriteLine(outcome);
        }
        public void ShowError(string message)
        {
            WriteLine($"Error: {message}");
        }
        public void ShowBuyHealthPotionSuccess()
        {
            WriteLine("You bought a health potion for 40 coins.");
        }
        public void ShowSellItemSuccess(string itemName, int price)
        {
            WriteLine($"You sold {itemName} for {price} coins.");
        }
    }
}