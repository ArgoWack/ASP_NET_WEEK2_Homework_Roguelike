﻿using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.View;
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
        private MapService _mapService;
        private bool _inGame;
        private MenuActionService _menuActionService;
        private GameView _gameView;

        public GameService()
        {
            _menuActionService = new MenuActionService();
            InitializeMenuActions(_menuActionService);
            _gameView = new GameView();
        }

        public void StartGame()
        {
            _playerCharacter = new PlayerCharacter();
            _map = new Map();
            _mapService = new MapService(new EventService(_playerController, _gameView));
            _playerCharacter.CurrentMap = _map;
            _playerController = new PlayerCharacterController(_playerCharacter, _map, _mapService);

            _eventService = new EventService(_playerController, _gameView);
            _characterInteractionService = new CharacterInteractionService(_eventService);
            _eventController = new EventController(_playerController, _eventService);
            EventGenerator.Initialize(_eventService, _characterInteractionService);

            _gameView.ShowWelcomeMessage();
            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            ConsoleKeyInfo operation;
            do
            {
                operation = _gameView.DisplayMenuAndGetChoice<int>("Main", "What would you like to do?", _menuActionService);

                switch (operation.KeyChar)
                {
                    case '1':
                        StartNewGame();
                        break;
                    case '2':
                        ContinueGame();
                        break;
                    case '3':
                        _gameView.ShowDescription();
                        break;
                    case '4':
                        QuitGame();
                        break;
                    default:
                        _gameView.ShowError("Wrong input");
                        break;
                }

                if (_inGame)
                {
                    RunInGameLoop();
                }
            } while (operation.KeyChar != '4');
        }

        public void RunInGameLoop()
        {
            ConsoleKeyInfo operation;
            do
            {
                operation = _gameView.DisplayMenuAndGetChoice<char>("InGameMenu", "\nWhat would you like to do: a/w/s/d/e/q? \n", _menuActionService);

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
                        _gameView.ShowError("Wrong input");
                        break;
                }
            } while (_inGame);
        }

        private void StartNewGame()
        {
            _playerCharacter.Name = _gameView.PromptForCharacterName();
            ReadKey();
            _mapService.InitializeStartingRoom(_map);
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
                _gameView.ShowError("No saved games found.");
                return;
            }

            int selectedIndex = _gameView.PromptForSaveFileSelection(saveFiles);

            if (selectedIndex > 0 && selectedIndex <= saveFiles.Length)
            {
                var selectedFile = saveFiles[selectedIndex - 1];
                var characterName = Path.GetFileNameWithoutExtension(selectedFile).Replace("_savefile", "");

                try
                {
                    var gameState = PlayerCharacter.LoadGame(characterName);
                    _playerCharacter = gameState.PlayerCharacter;
                    _map = gameState.Map;

                    _playerCharacter.CurrentMap = _map;
                    _playerController = new PlayerCharacterController(_playerCharacter, _map, _mapService);
                    _inGame = true;
                }
                catch (FileNotFoundException ex)
                {
                    _gameView.ShowError(ex.Message);
                }
            }
            else
            {
                _gameView.ShowError("Invalid selection. Returning to main menu.");
            }
        }

        private void QuitGame()
        {
            _gameView.ShowEndGameMessage();
            Environment.Exit(0);
        }

        private void MovePlayer(string direction)
        {
            _playerController.MovePlayer(direction);
            var currentRoom = _mapService.GetDiscoveredRoom(_map, _playerCharacter.CurrentX, _playerCharacter.CurrentY);
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
                choice = _gameView.PromptForInventoryChoice();

                if (choice == 'e')
                {
                    var id = _gameView.PromptForItemId("use");
                    if (id.HasValue)
                    {
                        _playerController.EquipItem(id.Value);
                    }
                }
                else if (choice == 'd')
                {
                    var id = _gameView.PromptForItemId("discard");
                    if (id.HasValue)
                    {
                        _playerController.DiscardItem(id.Value);
                    }
                }
            } while (choice != 'l');
        }

        private static void InitializeMenuActions(MenuActionService actionService)
        {
            actionService.AddNewAction(1, "New game", "Main");
            actionService.AddNewAction(2, "Continue", "Main");
            actionService.AddNewAction(3, "Game description", "Main");
            actionService.AddNewAction(4, "Quit game", "Main");

            actionService.AddNewAction('q', "Save and quit to main menu", "InGameMenu");
            actionService.AddNewAction('p', "Show player statistics", "InGameMenu");
            actionService.AddNewAction('e', "Open inventory", "InGameMenu");
            actionService.AddNewAction('a', "Move left", "InGameMenu");
            actionService.AddNewAction('w', "Move forward", "InGameMenu");
            actionService.AddNewAction('s', "Move backward", "InGameMenu");
            actionService.AddNewAction('d', "Move right", "InGameMenu");
            actionService.AddNewAction('m', "Show map", "InGameMenu");

            actionService.AddNewAction('e', "Use an item", "InventoryMenu");
            actionService.AddNewAction('d', "Discard an item", "InventoryMenu");
            actionService.AddNewAction('l', "Leave inventory", "InventoryMenu");
        }
    }
}