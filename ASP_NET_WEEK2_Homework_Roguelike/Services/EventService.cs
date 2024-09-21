using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.View;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class EventService
    {
        private readonly PlayerCharacterController _playerController;
        private readonly GameView _gameView;

        public EventService(PlayerCharacterController playerController, GameView gameView)
        {
            _playerController = playerController;
            _gameView = gameView;
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
            try
            {
                player.BuyHealthPotion();
                HandleEventOutcome("You bought a health potion for 40 coins.");
            }
            catch (InvalidOperationException ex)
            {
                HandleEventOutcome(ex.Message);
            }
        }
        public void SellItem(PlayerCharacter player, int itemId)
        {
            try
            {
                player.SellItem(itemId);
                HandleEventOutcome($"You sold the item with ID {itemId}.");
            }
            catch (InvalidOperationException ex)
            {
                HandleEventOutcome(ex.Message);
            }
        }
        public void HealPlayer(PlayerCharacter player, int amount)
        {
            player.Heal(amount);
            HandleEventOutcome($"You have been healed by {amount} points.");
        }
        public void ModifyPlayerStats(PlayerCharacter player, string stat, int amount)
        {
            switch (stat)
            {
                case "experience":
                    player.Experience += amount;
                    HandleEventOutcome($"You gained {amount} experience.");
                    break;
                case "speed":
                    player.ModifySpeed(amount);
                    HandleEventOutcome($"Your speed has been modified by {amount}.");
                    break;
                case "attack":
                    player.ModifyAttack(amount);
                    HandleEventOutcome($"Your attack has been modified by {amount}.");
                    break;
                case "defense":
                    player.ModifyDefense(amount);
                    HandleEventOutcome($"Your defense has been modified by {amount}.");
                    break;
                default:
                    HandleEventOutcome("Invalid stat modification.");
                    break;
            }
        }
    }
}
