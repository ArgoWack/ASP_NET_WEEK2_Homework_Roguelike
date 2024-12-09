using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public interface ICharacterInteractionService
    {
        void ModifyPlayerStats(PlayerCharacter player, string statType, float amount);
        void HealPlayer(PlayerCharacter player, int amount);
    }
}