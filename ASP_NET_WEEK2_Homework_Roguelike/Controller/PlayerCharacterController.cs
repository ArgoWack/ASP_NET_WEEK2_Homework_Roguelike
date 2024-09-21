using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using ASP_NET_WEEK2_Homework_Roguelike.View;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Events;
using ASP_NET_WEEK2_Homework_Roguelike.Services;

namespace ASP_NET_WEEK2_Homework_Roguelike.Controller
{
    public class PlayerCharacterController
    {
        private readonly PlayerCharacter _playerCharacter;
        private readonly PlayerCharacterView _view;
        private readonly MapService _mapService;
        private readonly Map _map;

        public PlayerCharacterView View => _view;

        public PlayerCharacterController(PlayerCharacter playerCharacter, Map map, MapService mapService)
        {
            _playerCharacter = playerCharacter;
            _view = new PlayerCharacterView();
            _map = map;
            _mapService = mapService ?? throw new ArgumentNullException(nameof(mapService));
            _playerCharacter.CurrentMap = map;
        }

        public PlayerCharacter PlayerCharacter => _playerCharacter;

        public void ShowCharacterStats()
        {
            _playerCharacter.UpdateStats();
            _view.ShowCharacterStats(_playerCharacter);
        }

        public void HandleEventEncounter(string eventType)
        {
            _view.ShowEventEncounter(eventType);
        }

        public void HandleEventOutcome(string outcome)
        {
            _view.ShowEventOutcome(outcome);
        }

        public void ShowMap()
        {
            _view.ShowMap(_map, _playerCharacter);
        }

        public void MovePlayer(string direction)
        {
            int currentX = _playerCharacter.CurrentX;
            int currentY = _playerCharacter.CurrentY;

            Room currentRoom = _mapService.GetDiscoveredRoom(_map, currentX, currentY);

            if (currentRoom != null && currentRoom.Exits.ContainsKey(direction))
            {
                _mapService.MovePlayer(_map, ref currentX, ref currentY, direction);

                _playerCharacter.CurrentX = currentX;
                _playerCharacter.CurrentY = currentY;

                Room newRoom = _mapService.GetDiscoveredRoom(_map, _playerCharacter.CurrentX, _playerCharacter.CurrentY);

                if (newRoom.EventStatus != "none")
                {
                    RandomEvent roomEvent = EventGenerator.GenerateEvent(newRoom.EventStatus);
                    roomEvent?.Execute(_playerCharacter, newRoom, this);
                }
            }
            else
            {
                _view.ShowErrorCannotMove();
            }
        }

        public void EquipItem(int itemId)
        {
            try
            {
                _playerCharacter.EquipItem(itemId);
                var item = _playerCharacter.Inventory.FirstOrDefault(i => i.ID == itemId);
                if (item != null)
                {
                    _view.ShowEquipItemSuccess(item.Name);
                }
            }
            catch (InvalidOperationException ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        public void DiscardItem(int itemId)
        {
            try
            {
                var item = _playerCharacter.Inventory.FirstOrDefault(i => i.ID == itemId);
                if (item != null)
                {
                    _playerCharacter.DiscardItem(itemId);
                    _view.ShowDiscardItemSuccess(item.Name);
                }
            }
            catch (InvalidOperationException ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        public void ShowInventory()
        {
            _view.DisplayInventory(_playerCharacter);
        }

        public void HandleEvent(RandomEvent randomEvent, Room room)
        {
            randomEvent.Execute(_playerCharacter, room, this);
        }

        public void BuyHealthPotion()
        {
            try
            {
                _playerCharacter.BuyHealthPotion();
                _view.ShowEventOutcome("You bought a health potion for 40 coins.");
            }
            catch (InvalidOperationException ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        public void SellItem(int itemId)
        {
            try
            {
                var item = _playerCharacter.Inventory.FirstOrDefault(i => i.ID == itemId);
                if (item != null)
                {
                    _playerCharacter.SellItem(itemId);
                    _view.ShowEventOutcome($"You sold {item.Name} for {item.MoneyWorth} coins.");
                }
            }
            catch (InvalidOperationException ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}