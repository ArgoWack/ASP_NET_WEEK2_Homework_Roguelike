using ASP_NET_WEEK2_Homework_Roguelike.Items;
using ASP_NET_WEEK2_Homework_Roguelike.Events;
using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using ASP_NET_WEEK2_Homework_Roguelike.View;

namespace ASP_NET_WEEK2_Homework_Roguelike.Controller
{
    public class PlayerCharacterController
    {
        private readonly PlayerCharacter _playerCharacter;
        public readonly PlayerCharacterView View;

        public PlayerCharacterController(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
            View = new PlayerCharacterView();
        }
        public void ShowCharacterStats()
        {
            _playerCharacter.UpdateWeight();
            _playerCharacter.UpdateAttack();
            _playerCharacter.UpdateDefense();

            View.ShowCharacterStats(_playerCharacter);
        }

        public void EquipItem(int itemId)
        {
            try
            {
                _playerCharacter.EquipItem(itemId);
                var item = _playerCharacter.Inventory.FirstOrDefault(i => i.ID == itemId);
                if (item != null)
                {
                    View.ShowEquipItemSuccess(item.Name);
                }
            }
            catch (InvalidOperationException ex)
            {
                View.ShowError(ex.Message);
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
                    View.ShowDiscardItemSuccess(item.Name);
                }
            }
            catch (InvalidOperationException ex)
            {
                View.ShowError(ex.Message);
            }
        }

        public void ShowInventory()
        {
            View.DisplayInventory(_playerCharacter);
        }
        public void MovePlayer(string direction, Map map)
        {
            _playerCharacter.MovePlayer(direction, map);
            View.ShowPlayerMovement(direction, _playerCharacter.CurrentX, _playerCharacter.CurrentY);
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
                View.ShowEventOutcome("You bought a health potion for 40 coins.");
            }
            catch (InvalidOperationException ex)
            {
                View.ShowError(ex.Message);
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
                    View.ShowEventOutcome($"You sold {item.Name} for {item.MoneyWorth} coins.");
                }
            }
            catch (InvalidOperationException ex)
            {
                View.ShowError(ex.Message);
            }
        }
    }
}