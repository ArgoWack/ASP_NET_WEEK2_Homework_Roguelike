using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public interface ICharacterStatsService
    {
        float CalculateAttack(PlayerCharacter player);
        float CalculateDefense(PlayerCharacter player);
        int CalculateWeight(PlayerCharacter player);
    }
}