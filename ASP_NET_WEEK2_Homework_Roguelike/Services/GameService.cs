using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Services;
using System;
using System.IO;
using static System.Console;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Events;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class GameService
    {
        private PlayerCharacter _playerCharacter;
        private Map _map;
        private PlayerCharacterController _playerController;
        private EventService _eventService;
        private CharacterInteractionService _characterInteractionService;
        private EventController _eventController;
        private bool _inGame;

        public void StartGame()
        {
            // Initialize main components
            _playerCharacter = new PlayerCharacter();
            _map = new Map();
            _playerCharacter.CurrentMap = _map;
            _playerController = new PlayerCharacterController(_playerCharacter, _map);

            // Initialize services and controllers
            _eventService = new EventService(_playerController);
            _characterInteractionService = new CharacterInteractionService(_eventService);
            _eventController = new EventController(_playerController, _eventService);
            EventGenerator.Initialize(_eventService, _characterInteractionService);

            // Display initial game welcome message
            WriteLine("Welcome to Roguelike game \n");
        }

        public void RunInGameLoop()
        {
            string description = " \n It's a simple roguelike game with the following hotkeys:" +
                "\n Q - Save & Quit" +
                "\n E - Opens Inventory" +
                "\n A/W/S/D - movement";

            ConsoleKeyInfo operation;

            do
            {
                // Calling WriteMenuAndParseChar method here
                operation = WriteMenuAndParseChar<int>("Main", "What would you like to do?");

                switch (operation.KeyChar)
                {
                    case '1':
                        StartNewGame();
                        break;
                    case '2':
                        ContinueGame();
                        break;
                    case '3':
                        ShowDescription(description);
                        break;
                    case '4':
                        QuitGame();
                        break;
                    default:
                        WriteLine("\nWrong input\n");
                        break;
                }

                while (_inGame)
                {
                    operation = WriteMenuAndParseChar<char>("InGameMenu", "\nWhat would you like to do: a/w/s/d/e/q? \n");

                    switch (operation.KeyChar)
                    {
                        case 'a':
                            MovePlayer("west");
                            break;
                        case 'w':
                            MovePlayer("north");
                            break;
                        case 's':
                            MovePlayer("south");
                            break;
                        case 'd':
                            MovePlayer("east");
                            break;
                        case 'm':
                            _playerController.ShowMap();
                            break;
                        case 'p':
                            _playerController.ShowCharacterStats();
                            break;
                        case 'e':
                            HandleInventory();
                            break;
                        case 'q':
                            _playerCharacter.SaveGame();
                            _inGame = false;
                            break;
                        default:
                            WriteLine("\n Wrong input");
                            break;
                    }
                }
            } while (operation.KeyChar != '4');
        }

        private void StartNewGame()
        {
            WriteLine("\n Write Character Name");
            _playerCharacter.Name = ReadLine();
            ReadKey();
            _map.InitializeStartingRoom();
            _playerCharacter.CurrentX = 0;
            _playerCharacter.CurrentY = 0;

            _playerCharacter.SaveGame();
            _inGame = true;
        }

        private void ContinueGame()
        {
            var saveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_savefile.json");

            if (saveFiles.Length == 0)
            {
                WriteLine("No saved games found.");
                return;
            }

            WriteLine("\n Available saved games:");
            for (int i = 0; i < saveFiles.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(saveFiles[i]);
                var characterName = fileName.Replace("_savefile", "");
                WriteLine($"{i + 1}. {characterName}");
            }

            WriteLine("\n Enter the number of the character you want to load:");
            if (int.TryParse(ReadLine(), out int selectedIndex) && selectedIndex > 0 && selectedIndex <= saveFiles.Length)
            {
                var selectedFile = saveFiles[selectedIndex - 1];
                var characterName = Path.GetFileNameWithoutExtension(selectedFile).Replace("_savefile", "");

                try
                {
                    var gameState = PlayerCharacter.LoadGame(characterName);
                    _playerCharacter = gameState.PlayerCharacter;
                    _map = gameState.Map;

                    _playerCharacter.CurrentMap = _map;
                    _playerController = new PlayerCharacterController(_playerCharacter, _map);
                    _inGame = true;
                }
                catch (FileNotFoundException ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                WriteLine("\nInvalid selection. Returning to main menu.\n");
            }
        }

        private void ShowDescription(string description)
        {
            WriteLine(description + "\n");
        }

        private void QuitGame()
        {
            Environment.Exit(0);
        }

        private void MovePlayer(string direction)
        {
            _playerController.MovePlayer(direction);

            var currentRoom = _map.GetDiscoveredRoom(_playerCharacter.CurrentX, _playerCharacter.CurrentY);
            if (currentRoom != null)
            {
                _eventController.ExecuteEvent(currentRoom);
            }
        }

        private void HandleInventory()
        {
            char choice;
            do
            {
                _playerController.ShowInventory();
                WriteLine(" \n Write:  \ne. Use some item \nd. Discard some item  \nl. Leave inventory");

                char.TryParse(ReadLine(), out choice);
                if (choice == 'e')
                {
                    WriteLine(" \n Write ID of the item you'd like to use");
                    if (int.TryParse(ReadLine(), out int id))
                    {
                        _playerController.EquipItem(id);
                    }
                    else
                    {
                        WriteLine("\nInvalid ID.\n");
                    }
                }
                if (choice == 'd')
                {
                    WriteLine(" \n Write ID of the item you'd like to discard");
                    if (int.TryParse(ReadLine(), out int id))
                    {
                        _playerController.DiscardItem(id);
                    }
                    else
                    {
                        WriteLine("\nInvalid ID.\n");
                    }
                }
            } while (choice != 'l');
        }

        private static MenuActionService Initialize(MenuActionService actionService)
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

        // Re-added WriteMenuAndParseChar method to ensure it is available in GameService
        private static ConsoleKeyInfo WriteMenuAndParseChar<T>(string menuKind, string WriteLineText)
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
    }
}