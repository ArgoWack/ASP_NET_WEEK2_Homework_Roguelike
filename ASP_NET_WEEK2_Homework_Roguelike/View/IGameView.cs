using ASP_NET_WEEK3_Homework_Roguelike.Services;

namespace ASP_NET_WEEK3_Homework_Roguelike.View
{
    public interface IGameView
    {
        void DisplayMessage(string message);
        void ShowWelcomeMessage();
        void ShowDescription();
        char PromptForInventoryChoice();
        int? PromptForItemId(string action);
        string PromptForCharacterName();
        int PromptForSaveFileSelection(string[] saveFiles);
        void ShowError(string message);
        string PromptForItemPickup();
        string GetMerchantOptions();
        int? PromptForItemIdToSell();
        string GetMonsterOptions();
        void ShowEndGameMessage();
        void ShowMessage(string message);
        ConsoleKeyInfo DisplayMenuAndGetChoice<T>(string menuKind, string prompt, MenuActionService menuActionService);
    }
}