using ASP_NET_WEEK2_Homework_Roguelike;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Console;
using static System.Net.Mime.MediaTypeNames;

string description = "It's simple roguelike game. With the following hotkeys:" +
    "Q - Save&Quit" +
    "E - Opens Inventory" +
    "A/W/S/D - movement";
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

var operation = ReadKey();
do
{
        MenuActionService actionService = new MenuActionService();
    actionService = Initialize(actionService);

    WriteLine("Welcome to Roguelike game");
    WriteLine("What would like to do?");

    var mainMenu = actionService.GetMenuActionsByMenuName("Main");
    for (int i = 0; i < mainMenu.Count; i++)
    {
        WriteLine($"{mainMenu[i].Id}. {mainMenu[i].Name}");
    }

         operation = ReadKey();

    PlayerCharacter playerCharacter = new PlayerCharacter();


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


static MenuActionService Initialize(MenuActionService actionService)
{
    actionService.AddNewAction(1, "New game", "Main");
    actionService.AddNewAction(2, "Continue", "Main");
    actionService.AddNewAction(3, "Game description", "Main");
    actionService.AddNewAction(4, "Quit game", "Main");

    //actionService.AddNewAction(q, "Save and quit to main menu", "InGameMenu");
    //actionService.AddNewAction(e, "Open inventory", "InGameMenu");
    //actionService.AddNewAction(a, "Move left", "InGameMenu");
    //actionService.AddNewAction(w, "Move straight", "InGameMenu");
    //actionService.AddNewAction(s, "Move back", "InGameMenu");
    //actionService.AddNewAction(d, "Move right", "InGameMenu");

    return actionService;
}