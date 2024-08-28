using ASP_NET_WEEK2_Homework_Roguelike;
using ASP_NET_WEEK2_Homework_Roguelike.Events;
using ASP_NET_WEEK2_Homework_Roguelike.Items;
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
WriteLine("Welcome to Roguelike game \n");
PlayerCharacter playerCharacter = new PlayerCharacter();
Map map = new Map();
playerCharacter.CurrentMap = map;
bool inGame = false;

do
{
    operation = WriteMenuAndParseChar<int>("Main", "What would you like to do?");

    switch (operation.KeyChar)
    {
        case '1':
            // Creating New Character
            WriteLine("\n Write Character Name");
            playerCharacter.Name = ReadLine();
            ReadKey();
            map.InitializeStartingRoom();
            playerCharacter.CurrentX = 0;
            playerCharacter.CurrentY = 0;

            // Save and continue to game loop
            playerCharacter.SaveGame();
            inGame = true; // Set the flag to true to enter the in-game loop
            break;

        case '2':
            // Continuing the previous game
            var saveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_savefile.json");

            if (saveFiles.Length == 0)
            {
                WriteLine("No saved games found.");
                break;
            }

            WriteLine("\n Available saved games:");
            for (int i = 0; i < saveFiles.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(saveFiles[i]);
                var characterName = fileName.Replace("_savefile", "");
                WriteLine($"{i + 1}. {characterName}");
            }

            WriteLine("Enter the number of the character you want to load:");
            if (int.TryParse(ReadLine(), out int selectedIndex) && selectedIndex > 0 && selectedIndex <= saveFiles.Length)
            {
                var selectedFile = saveFiles[selectedIndex - 1];
                var characterName = Path.GetFileNameWithoutExtension(selectedFile).Replace("_savefile", "");

                try
                {
                    var gameState = PlayerCharacter.LoadGame(characterName);
                    playerCharacter = gameState.PlayerCharacter;
                    map = gameState.Map;

                    // Updating map
                    playerCharacter.CurrentMap = map;
                    inGame = true; // Set the flag to true to enter the in-game loop
                }
                catch (FileNotFoundException ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                WriteLine("Invalid selection. Returning to main menu.");
            }
            break;

        case '3':
            // Displaying description of game and controls
            WriteLine(description +"\n");
            break;

        case '4':
            // Quitting the game
            Environment.Exit(0);
            break;

        default:
            WriteLine("Wrong input");
            break;
    }

    // Enter the in-game menu only if a game is started or loaded
    while (inGame)
    {
        operation = WriteMenuAndParseChar<char>("InGameMenu", "\nWhat would you like to do: a/w/s/d/e/q? \n");

        switch (operation.KeyChar)
        {
            case 'a':
                TryMovePlayer(playerCharacter, map, "west");
                break;
            case 'w':
                TryMovePlayer(playerCharacter, map, "north");
                break;
            case 's':
                TryMovePlayer(playerCharacter, map, "south");
                break;
            case 'd':
                TryMovePlayer(playerCharacter, map, "east");
                break;
            case 'm':
                map.DisplayMap(playerCharacter);
                break;
            case 'p':
                playerCharacter.DisplayCharacterStats();
                break;
            case 'e':
                char choice;
                do
                {
                    playerCharacter.CheckInventory();
                    WriteLine(" \n Write:  \ne. Use some item \nd. Discard some item  \nl. Leave inventory");

                    char.TryParse(ReadLine(), out choice);
                    if (choice == 'e')
                    {
                        WriteLine(" \n Write ID of the item you'd like to use");
                        if (int.TryParse(ReadLine(), out int id))
                        {
                            playerCharacter.EquipItem(id);
                        }
                        else
                        {
                            WriteLine("Invalid ID.");
                        }
                    }
                    if (choice == 'd')
                    {
                        WriteLine(" \n Write ID of the item you'd like to discard");
                        if (int.TryParse(ReadLine(), out int id))
                        {
                            playerCharacter.DiscardItem(id);
                        }
                        else
                        {
                            WriteLine("Invalid ID.");
                        }
                    }
                }
                while (choice != 'l');
                break;
            case 'q':
                // Save and return to main menu
                playerCharacter.SaveGame();
                inGame = false; // Set the flag to false to return to the main menu
                break;
            default:
                WriteLine("\n Wrong input");
                break;
        }
    }
} while (operation.KeyChar != '4'); // Exit only when 4 is chosen

static void TryMovePlayer(PlayerCharacter player, Map map, string direction)
{
    int currentX = player.CurrentX;
    int currentY = player.CurrentY;
    Room currentRoom = map.GetDiscoveredRoom(currentX, currentY);

    if (currentRoom != null && currentRoom.Exits.ContainsKey(direction))
    {
        player.MovePlayer(direction, map);

        Room newRoom = map.GetDiscoveredRoom(player.CurrentX, player.CurrentY);

        // Handle event in the new room
        if (newRoom.EventStatus != "none")
        {
            RandomEvent roomEvent = EventGenerator.GenerateEvent(newRoom.EventStatus);
            roomEvent?.Execute(player, newRoom);
        }
    }
    else
    {
        WriteLine("\n You cannot move in that direction. There is no room.");
    }
}



static MenuActionService Initialize(MenuActionService actionService)
{
    actionService.AddNewAction(1, "New game", "Main");
    actionService.AddNewAction(2, "Continue", "Main");
    actionService.AddNewAction(3, "Game description", "Main");
    actionService.AddNewAction(4, "Quit game", "Main");

    actionService.AddNewAction('q', "Save and quit to main menu", "InGameMenu");
    actionService.AddNewAction('p', "Show player statistics", "InGameMenu");
    actionService.AddNewAction('e', "Open inventory", "InGameMenu");
    actionService.AddNewAction('a', "Move left", "InGameMenu");
    actionService.AddNewAction('w', "Move straight", "InGameMenu");
    actionService.AddNewAction('s', "Move back", "InGameMenu");
    actionService.AddNewAction('d', "Move right", "InGameMenu");
    actionService.AddNewAction('m', "Show map", "InGameMenu");

    actionService.AddNewAction('e', "Use some item", "InventoryMenu");
    actionService.AddNewAction('d', "Discard item", "InventoryMenu");
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