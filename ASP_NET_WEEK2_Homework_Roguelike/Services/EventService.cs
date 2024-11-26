using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.View;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class EventService
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
            }
            catch (InvalidOperationException ex)
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
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            player.Heal(amount);
            HandleEventOutcome($"You have been healed by {amount} points.");
        }
        public void ModifyPlayerStats(PlayerCharacter player, string stat, int amount)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            switch (stat.ToLower())
            {
                case "experience":
                    player.GetExperience(amount);
                    HandleEventOutcome($"You gained {amount} experience points.");
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
                    HandleEventOutcome($"Invalid stat modification: {stat}.");
                    break;
            }
        }
    }
}