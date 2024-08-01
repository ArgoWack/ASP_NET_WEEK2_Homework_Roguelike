using ASP_NET_WEEK2_Homework_Roguelike;
using System.Runtime.CompilerServices;
using static System.Console;
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

MenuActionService actionService = new MenuActionService();
actionService = Initialize(actionService);

WriteLine("Welcome to Roguelike game");
WriteLine("What would like to do?");

var mainMenu = actionService.GetMenuActionsByMenuName("Main");
for (int i = 0; i < mainMenu.Count; i++)
{
    WriteLine($"{mainMenu[i].Id}. {mainMenu[i].Name}");
}

var operation = ReadKey();
switch(operation.KeyChar)
{
    case '1':
        break;
    case '2':
        break;
    case '3':
        break;
    case '4':
        break;
    default:
        WriteLine("Wrong input");
        break;
}

  static MenuActionService Initialize(MenuActionService actionService)
{
    actionService.AddNewAction(1, "New game", "Main");
    actionService.AddNewAction(2, "Continue", "Main");
    actionService.AddNewAction(3, "Game description", "Main");
    actionService.AddNewAction(4, "Quit game", "Main");
    return actionService;
}