﻿using ASP_NET_WEEK2_Homework_Roguelike;
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
WriteLine("Welcome to Roguelike game");
PlayerCharacter playerCharacter = new PlayerCharacter();
Map map = new Map();
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
            map.InitializeStartingRoom();
            playerCharacter.CurrentX = 0;
            playerCharacter.CurrentY = 0;

            // Save and exit
            playerCharacter.SaveGame("savefile.json",map);

            break;
        case '2':
            // Continuing the previous game

            try
            {
                var gameState = PlayerCharacter.LoadGame("savefile.json");
                playerCharacter = gameState.PlayerCharacter;
                map = gameState.Map;
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
            WriteLine(" \n Character name: " + playerCharacter.Name
                + "\n Attack: " + playerCharacter.Attack
                + "\n Defense: " + playerCharacter.Defense
                + "\n Speed: " + playerCharacter.Speed
                + "\n Weight: " + playerCharacter.Weight
                + "\n Money: " + playerCharacter.Money
                + "\n Health: " + playerCharacter.Health
                + "\n Level: " + playerCharacter.Level
                + "\n Experience: " + playerCharacter.Experience
            + "\n Equipped Amulet: " + playerCharacter.EquippedAmulet + " ID: " + playerCharacter.EquippedAmulet.ID + " Defense: " + playerCharacter.EquippedAmulet.Defense + " Attack: " + playerCharacter.EquippedAmulet.Attack + " Weight: " + playerCharacter.EquippedAmulet.Weight + " Money worth: " + playerCharacter.EquippedAmulet.MoneyWorth + " Description: " + playerCharacter.EquippedAmulet.Description
            + "\n Equipped Armor: " + playerCharacter.EquippedArmor + " ID: " + playerCharacter.EquippedArmor.ID + " Defense: " + playerCharacter.EquippedArmor.Defense + " Attack: " + playerCharacter.EquippedArmor.Attack + " Weight: " + playerCharacter.EquippedArmor.Weight + " Money worth: " + playerCharacter.EquippedArmor.MoneyWorth + " Description: " + playerCharacter.EquippedArmor.Description
            + "\n Equipped Boots: " + playerCharacter.EquippedBoots + " ID: " + playerCharacter.EquippedBoots.ID + " Defense: " + playerCharacter.EquippedBoots.Defense + " Attack: " + playerCharacter.EquippedBoots.Attack + " Weight: " + playerCharacter.EquippedBoots.Weight + " Money worth: " + playerCharacter.EquippedBoots.MoneyWorth + " Description: " + playerCharacter.EquippedBoots.Description
            + "\n Equipped Gloves: " + playerCharacter.EquippedGloves + " ID: " + playerCharacter.EquippedGloves.ID + " Defense: " + playerCharacter.EquippedGloves.Defense + " Attack: " + playerCharacter.EquippedGloves.Attack + " Weight: " + playerCharacter.EquippedGloves.Weight + " Money worth: " + playerCharacter.EquippedGloves.MoneyWorth + " Description: " + playerCharacter.EquippedGloves.Description
            + "\n Equipped Helmet: " + playerCharacter.EquippedHelmet + " ID: " + playerCharacter.EquippedHelmet.ID + " Defense: " + playerCharacter.EquippedHelmet.Defense + " Attack: " + playerCharacter.EquippedHelmet.Attack + " Weight: " + playerCharacter.EquippedHelmet.Weight + " Money worth: " + playerCharacter.EquippedHelmet.MoneyWorth + " Description: " + playerCharacter.EquippedHelmet.Description
            + "\n Equipped Shield: " + playerCharacter.EquippedShield + " ID: " + playerCharacter.EquippedShield.ID + " Defense: " + playerCharacter.EquippedShield.Defense + " Attack: " + playerCharacter.EquippedShield.Attack + " Weight: " + playerCharacter.EquippedShield.Weight + " Money worth: " + playerCharacter.EquippedShield.MoneyWorth + " Description: " + playerCharacter.EquippedShield.Description
            + "\n Equipped Equipped SwordOneHanded: " + playerCharacter.EquippedSwordOneHanded + " ID: " + playerCharacter.EquippedSwordOneHanded.ID + " Defense: " + playerCharacter.EquippedSwordOneHanded.Defense + " Attack: " + playerCharacter.EquippedSwordOneHanded.Attack + " Weight: " + playerCharacter.EquippedSwordOneHanded.Weight + " Money worth: " + playerCharacter.EquippedSwordOneHanded.MoneyWorth + " Description: " + playerCharacter.EquippedSwordOneHanded.Description
            + "\n Equipped Equipped SwordTwoHanded: " + playerCharacter.EquippedSwordTwoHanded + " ID: " + playerCharacter.EquippedSwordTwoHanded.ID + " Defense: " + playerCharacter.EquippedSwordTwoHanded.Defense + " Attack: " + playerCharacter.EquippedSwordTwoHanded.Attack + " Weight: " + playerCharacter.EquippedSwordTwoHanded.Weight + " Money worth: " + playerCharacter.EquippedSwordTwoHanded.MoneyWorth + " Description: " + playerCharacter.EquippedSwordTwoHanded.Description
            + "\n Equipped Equipped Trousers: " + playerCharacter.EquippedTrousers + " ID: " + playerCharacter.EquippedTrousers.ID + " Defense: " + playerCharacter.EquippedTrousers.Defense + " Attack: " + playerCharacter.EquippedTrousers.Attack + " Weight: " + playerCharacter.EquippedTrousers.Weight + " Money worth: " + playerCharacter.EquippedTrousers.MoneyWorth + " Description: " + playerCharacter.EquippedTrousers.Description);
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
            playerCharacter.SaveGame("savefile.json", map);
            Environment.Exit(0);
            break;
        default:
            WriteLine("Wrong input");
            break;
    }
}
while (operation.KeyChar != 'q');

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
            newRoom.EventStatus = "none";
        }
    }
    else
    {
        WriteLine("You cannot move in that direction. There is no room.");
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