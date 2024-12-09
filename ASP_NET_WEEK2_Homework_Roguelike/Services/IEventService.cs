using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public interface IEventService
    {
        void HandleEventOutcome(string outcome);
        string PromptForItemPickup();
        string GetMerchantOptions();
        int? PromptForItemIdToSell();
        string GetMonsterOptions();
        void BuyHealthPotion(PlayerCharacter player);
        void SellItem(PlayerCharacter player, int itemId);
    }
}