using System;
using System.Collections.Generic;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Services;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.View
{
    public class GameView
    {
        public void DisplayMessage(string message)
        {
            WriteLine(message);
        }

        public void ShowWelcomeMessage()
        {
            DisplayMessage("Welcome to Roguelike game \n");
        }

        public void ShowDescription()
        {
            string description = " \n It's a simple roguelike game with the following hotkeys:" +
                                 "\n Q - Save & Quit" +
                                 "\n E - Opens Inventory" +
                                 "\n A/W/S/D - movement";
            DisplayMessage(description + "\n");
        }
        public void DisplayInventory(PlayerCharacter player)
        {
            WriteLine("\nHere is your inventory:");

            if (!player.Inventory.Any())
            {
                WriteLine("Your inventory is empty.");
                return;
            }

            foreach (var item in player.Inventory)
            {
                if (item is HealthPotion potion)
                {
                    WriteLine($"Item: {potion.Name} | ID: {potion.ID} | Quantity: {potion.Quantity} | Max Stack Size: {potion.MaxStackSize} | Healing: {potion.HealingAmount} | Weight: {potion.Weight * potion.Quantity}| Value: {item.MoneyWorth} coins");
                }
                else
                {
                    WriteLine($"Item: {item.Name} | ID: {item.ID} | Defense: {item.Defense} | Attack: {item.Attack} | Weight: {item.Weight} | Value: {item.MoneyWorth} coins");
                }
            }
        }
        public char PromptForInventoryChoice()
        {
            DisplayMessage(" \n Write:  \ne. Use some item \nd. Discard some item  \nl. Leave inventory");
            char.TryParse(ReadLine(), out char choice);
            return choice;
        }

        public int? PromptForItemId(string action)
        {
            DisplayMessage($" \n Write ID of the item you'd like to {action}:");
            if (int.TryParse(ReadLine(), out int id))
            {
                return id;
            }
            else
            {
                DisplayMessage("Invalid ID.");
                return null;
            }
        }

        public string PromptForCharacterName()
        {
            DisplayMessage("\n Write Character Name");
            return ReadLine();
        }

        public int PromptForSaveFileSelection(string[] saveFiles)
        {
            DisplayMessage("\n Available saved games:");
            for (int i = 0; i < saveFiles.Length; i++)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(saveFiles[i]);
                var characterName = fileName.Replace("_savefile", "");
                DisplayMessage($"{i + 1}. {characterName}");
            }

            DisplayMessage("\n Enter the number of the character you want to load:");
            if (int.TryParse(ReadLine(), out int selectedIndex))
            {
                return selectedIndex;
            }
            else
            {
                ShowError("Invalid input. Returning to main menu.");
                return -1;
            }
        }

        public void ShowError(string message)
        {
            DisplayMessage($"Error: {message}");
        }

        public string PromptForItemPickup()
        {
            DisplayMessage("Would you like to take it? (y/n)");
            return ReadLine().ToLower();
        }

        public string GetMerchantOptions()
        {
            DisplayMessage("Write: \nb - Buy health potion for 40 coins \ns - Sell an item \nl - Leave");
            return ReadLine().ToLower();
        }

        public int? PromptForItemIdToSell()
        {
            DisplayMessage("Enter the ID of the item you want to sell:");
            if (int.TryParse(ReadLine(), out int itemId))
            {
                return itemId;
            }
            return null;
        }

        public string GetMonsterOptions()
        {
            DisplayMessage("\nChoose an action: \nf - Fight \nh - Heal \nl - Leave/Flee");
            return ReadLine().ToLower();
        }

        public ConsoleKeyInfo DisplayMenuAndGetChoice<T>(string menuKind, string prompt, MenuActionService menuActionService)
        {
            var menu = menuActionService.GetMenuActionsByMenuName(menuKind);
            DisplayMessage(prompt);

            foreach (var action in menu)
            {
                if (typeof(T) == typeof(int))
                {
                    DisplayMessage($"{action.Id}. {action.Name}");
                }
                else if (typeof(T) == typeof(char))
                {
                    DisplayMessage($"{(char)action.Id}. {action.Name}");
                }
            }

            return ReadKey(true);
        }

        public void ShowEndGameMessage()
        {
            DisplayMessage("Thank you for playing! The game has ended.");
        }
    }
}