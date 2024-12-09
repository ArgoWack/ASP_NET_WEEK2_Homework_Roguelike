using ASP_NET_WEEK3_Homework_Roguelike.Controller;
using ASP_NET_WEEK3_Homework_Roguelike.Model;
using ASP_NET_WEEK3_Homework_Roguelike.View;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public class EventService: IEventService
    {
        private PlayerCharacterController _playerController;
        private readonly GameView _gameView;

        public EventService(PlayerCharacterController playerController, GameView gameView)
        {
            _playerController = playerController;
            _gameView = gameView ?? throw new ArgumentNullException(nameof(gameView));
        }

        // method to set the PlayerCharacterController after the object is created
        public void SetPlayerController(PlayerCharacterController playerController)
        {
            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));
        }
        public void HandleEventOutcome(string outcome)
        {
            _gameView.DisplayMessage(outcome);
        }
        public string PromptForItemPickup()
        {
            return _gameView.PromptForItemPickup();
        }
        public string GetMerchantOptions()
        {
            return _gameView.GetMerchantOptions();
        }
        public int? PromptForItemIdToSell()
        {
            return _gameView.PromptForItemIdToSell();
        }
        public string GetMonsterOptions()
        {
            return _gameView.GetMonsterOptions();
        }
        public void BuyHealthPotion(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            try
            {
                player.BuyHealthPotion();
                HandleEventOutcome($"You bought a health potion for 40 coins. Current money: {player.Money} coins.");
            }
            catch (Exception ex)
            {
                HandleEventOutcome(ex.Message);
            }
        }
        public void SellItem(PlayerCharacter player, int itemId)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            try
            {
                var item = player.Inventory.FirstOrDefault(i => i.ID == itemId);
                if (item == null) throw new InvalidOperationException("Item not found in inventory.");

                int previousMoney = player.Money;
                player.SellItem(itemId);
                int earned = player.Money - previousMoney;

                HandleEventOutcome($"You sold {item.Name} for {earned} coins. Current Wealth: {player.Money} coins.");
            }
            catch (Exception ex)
            {
                HandleEventOutcome(ex.Message);
            }
        }
    }
}