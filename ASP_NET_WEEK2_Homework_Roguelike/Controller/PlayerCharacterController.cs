using ASP_NET_WEEK2_Homework_Roguelike.View;
using ASP_NET_WEEK2_Homework_Roguelike;
using ASP_NET_WEEK2_Homework_Roguelike.Events;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.Controller
{
    public class PlayerCharacterController
    {
        private PlayerCharacter _playerCharacter;
        private PlayerCharacterView _view;

        public PlayerCharacterController(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _view = new PlayerCharacterView();
        }

        public void ShowCharacterStats()
        {
            _view.DisplayCharacterStats(_playerCharacter);
        }

        public void ShowInventory()
        {
            _view.DisplayInventory(_playerCharacter);
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
                WriteLine(ex.Message);
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
                WriteLine(ex.Message);
            }
        }

        public void MovePlayer(string direction, Map map)
        {
            int currentX = _playerCharacter.CurrentX;
            int currentY = _playerCharacter.CurrentY;
            Room currentRoom = map.GetDiscoveredRoom(currentX, currentY);

            if (currentRoom != null && currentRoom.Exits.ContainsKey(direction))
            {
                _playerCharacter.MovePlayer(direction, map);
                Room newRoom = map.GetDiscoveredRoom(_playerCharacter.CurrentX, _playerCharacter.CurrentY);

                // Jeśli nowy pokój ma zdarzenie, wykonaj je
                if (newRoom.EventStatus != "none")
                {
                    RandomEvent roomEvent = EventGenerator.GenerateEvent(newRoom.EventStatus);
                    roomEvent?.Execute(_playerCharacter, newRoom);
                }
            }
            else
            {
                WriteLine("\n You cannot move in that direction. There is no room.");
            }
        }
    }
}