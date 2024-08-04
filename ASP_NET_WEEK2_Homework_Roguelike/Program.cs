﻿using ASP_NET_WEEK2_Homework_Roguelike;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Console;
using static System.Net.Mime.MediaTypeNames;

string description = " \n It's simple roguelike game. With the following hotkeys:" +
    "\n Q - Save&Quit" +
    "\n E - Opens Inventory" +
    "\n A/W/S/D - movement";
//yet to be written


// Przywitanie użytkownika
// Pojawienie się main menu
////a. Nowa gra
////b. Kontynuuj
////c. Opis gry
////d. Wyjdź z gry
//////a Stworzenie nowej postaci z domyślnymi statystykami
//////a1. Ruch A/W/S/D
//////a2. E - sprawdzenie ekwipunku
//////a3. Q - zapis i wyjście
//////b Wczytanie statusu poprzedniej postaci
//////b1. Ruch A/W/S/D
//////b2. E - sprawdzenie ekwipunku
//////b3. Q - zapis i wyjście
/////// a1/b1 Losowanie zdarzeń
/////// 1) znalezienie przedmiotu (losowanie z puli przedmiotów)
/////// 2) wydarzenie (losowanie z puli wydarzeń)
/////// 3) nic się nie dzieje
/////// 4) spotkanie postaci (losowanie z puli postaci)

ConsoleKeyInfo operation;
WriteLine("Welcome to Roguelike game");
PlayerCharacter playerCharacter = new PlayerCharacter();
do
{
    operation = WriteMenuAndParseChar<int>("Main", " What would like to do?");

    switch (operation.KeyChar)
    {
        case '1':
            // Creating New Character
            WriteLine("Write Character Name");
            playerCharacter.Name = ReadLine();
            ReadKey();



            // Save and exit
            playerCharacter.SaveGame("savefile.json");

            break;
        case '2':
            // Continuing the previous game

            try
            {
                PlayerCharacter loadedPlayer = PlayerCharacter.LoadGame("savefile.json");
            }
            catch (FileNotFoundException ex)
            {
                WriteLine(ex.Message);
            }
            break;
        case '3':
            // Displaying description of game and controls
            WriteLine(description);
            break;
        case '4':
            // Quitting the game
            Environment.Exit(0);
            break;
        default:
            WriteLine("Wrong input");
            break;
    }
}
while (operation.KeyChar =='3');

do
{
    operation = WriteMenuAndParseChar<char>("InGameMenu", " \n What would like to do: a/w/s/d/e/q?");

    switch (operation.KeyChar)
    {
        case 'a':

            break;
        case 'w':

            break;
        case 's':

            break;
        case 'd':

            break;
        case 'e':
            char choice;
            do
            {
                // choice = WriteMenuAndParseChar<char>("InventoryMenu", " What would like to do?");

                playerCharacter.CheckInventory();
                WriteLine(" \n Write:  \ne. Use some item \nl. Leave inventory");

                char.TryParse(ReadLine(), out choice);
                if (choice == 'e')
                {
                    WriteLine(" \n Write ID of itme you'd like to use");
                    int id;
                    Int32.TryParse(ReadLine(), out id);
                    playerCharacter.EquipItem(id);
                }
            }
            while (choice != 'l');

            break;
        case 'q':
            // Quitting the game
            Environment.Exit(0);
            break;
        default:
            WriteLine("Wrong input");
            break;
    }
}
while (operation.KeyChar != 'q');

static MenuActionService Initialize(MenuActionService actionService)
{
    actionService.AddNewAction(1, "New game", "Main");
    actionService.AddNewAction(2, "Continue", "Main");
    actionService.AddNewAction(3, "Game description", "Main");
    actionService.AddNewAction(4, "Quit game", "Main");

    actionService.AddNewAction('q', "Save and quit to main menu", "InGameMenu");
    actionService.AddNewAction('e', "Open inventory", "InGameMenu");
    actionService.AddNewAction('a', "Move left", "InGameMenu");
    actionService.AddNewAction('w', "Move straight", "InGameMenu");
    actionService.AddNewAction('s', "Move back", "InGameMenu");
    actionService.AddNewAction('d', "Move right", "InGameMenu");

    actionService.AddNewAction('e', "Use some item", "InventoryMenu");
    actionService.AddNewAction('l', "Leave inventory", "InventoryMenu");

    return actionService;
}

static ConsoleKeyInfo WriteMenuAndParseChar<T>(string menuKind, string WriteLineText)
{
    MenuActionService actionService = new MenuActionService();
    actionService = Initialize(actionService);

    WriteLine(WriteLineText);

    var menu = actionService.GetMenuActionsByMenuName(menuKind);
    for (int i = 0; i < menu.Count; i++)
    {
        if (typeof(T) == typeof(int))
        {
            WriteLine($"{Convert.ToInt32(menu[i].Id)}. {menu[i].Name}");
        }
        else if (typeof(T) == typeof(char))
        {
            WriteLine($"{(char)menu[i].Id}. {menu[i].Name}");
        }
        else
        {
            throw new InvalidOperationException("Unsupported type.");
        }
    }
    return ReadKey(true);
}