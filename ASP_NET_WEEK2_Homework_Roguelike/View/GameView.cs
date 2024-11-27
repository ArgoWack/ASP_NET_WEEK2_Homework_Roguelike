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
            ConsoleHelper.PrintColored(message, ConsoleColor.Blue, true);
        }
        public void ShowWelcomeMessage()
        {
            //DisplayMessage("Welcome to Roguelike game \n");
            ConsoleHelper.PrintColored("Welcome to Roguelike game \n", ConsoleColor.DarkCyan, true);

        }
        public void ShowDescription()
        {
            string description = " \n It's a simple roguelike game with the following hotkeys:" +
                                 "\n Q - Save & Quit" +
                                 "\n E - Opens Inventory" +
                                 "\n A/W/S/D - movement";
            ConsoleHelper.PrintColored(description + "\n", ConsoleColor.Cyan, true);
        }
        public char PromptForInventoryChoice()
        {
            ConsoleHelper.PrintColored(" \n Write:  \ne. Use some item \nd. Discard some item  \nl. Leave inventory", ConsoleColor.DarkYellow, true);
            char.TryParse(ReadLine(), out char choice);
            return choice;
        }
        public int? PromptForItemId(string action)
        {
            ConsoleHelper.PrintColored($" \n Write ID of the item you'd like to {action}:", ConsoleColor.Yellow, true);
            if (int.TryParse(ReadLine(), out int id))
            {
                return id;
            }
            else
            {
                ConsoleHelper.PrintColored("Invalid ID.", ConsoleColor.DarkRed, true);
                return null;
            }
        }
        public string PromptForCharacterName()
        {
            ConsoleHelper.PrintColored("\n Write Character Name", ConsoleColor.Yellow, true);
            return ReadLine();
        }
        public int PromptForSaveFileSelection(string[] saveFiles)
        {
            ConsoleHelper.PrintColored("\n Available saved games:", ConsoleColor.Yellow, true);
            for (int i = 0; i < saveFiles.Length; i++)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(saveFiles[i]);
                var characterName = fileName.Replace("_savefile", "");
                ConsoleHelper.PrintColored($"{i + 1}. {characterName}", ConsoleColor.DarkYellow, true);
            }
            ConsoleHelper.PrintColored("\n Enter the number of the character you want to load:", ConsoleColor.Yellow, true);
            if (int.TryParse(ReadLine(), out int selectedIndex))
            {
                return selectedIndex;
            }
            else
            {
                ConsoleHelper.PrintColored("Invalid input. Returning to main menu.", ConsoleColor.DarkRed, true);
                return -1;
            }
        }
        public void ShowError(string message)
        {
            ConsoleHelper.PrintColored($"Error: {message}", ConsoleColor.DarkRed, true);
        }
        public string PromptForItemPickup()
        {
            ConsoleHelper.PrintColored("Would you like to take it? (y/n)", ConsoleColor.Yellow, true);
            return ReadLine().ToLower();
        }
        public string GetMerchantOptions()
        {
            ConsoleHelper.PrintColored("Write: \nb - Buy health potion for 40 coins \ns - Sell an item \nl - Leave", ConsoleColor.DarkYellow, true);
            return ReadLine().ToLower();
        }
        public int? PromptForItemIdToSell()
        {
            ConsoleHelper.PrintColored("Enter the ID of the item you want to sell:", ConsoleColor.Yellow, true);
            if (int.TryParse(ReadLine(), out int itemId))
            {
                return itemId;
            }
            return null;
        }
        public string GetMonsterOptions()
        {
            ConsoleHelper.PrintColored("\nChoose an action: \nf - Fight \nh - Heal \nl - Leave/Flee", ConsoleColor.DarkYellow, true);
            return ReadLine().ToLower();
        }
        public ConsoleKeyInfo DisplayMenuAndGetChoice<T>(string menuKind, string prompt, MenuActionService menuActionService)
        {
            var menu = menuActionService.GetMenuActionsByMenuName(menuKind);
            ConsoleHelper.PrintColored(prompt, ConsoleColor.Yellow, true);
            foreach (var action in menu)
            {
                if (typeof(T) == typeof(int))
                {
                    ConsoleHelper.PrintColored($"{action.Id}. {action.Name}", ConsoleColor.DarkYellow, true);
                }
                else if (typeof(T) == typeof(char))
                {
                    ConsoleHelper.PrintColored($"{(char)action.Id}. {action.Name}", ConsoleColor.DarkYellow, true);
                }
            }

            return ReadKey(true);
        }
        public void ShowEndGameMessage()
        {
            ConsoleHelper.PrintColored("Thank you for playing! The game has ended.", ConsoleColor.Cyan, true);
        }
        public void ShowMessage(string message)
        {
            DisplayMessage(message);

        }
    }
}