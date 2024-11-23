using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class CharacterStatsService
    {
        public float CalculateAttack(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            return (player.EquippedHelmet?.Attack ?? 0) +
                   (player.EquippedArmor?.Attack ?? 0) +
                   (player.EquippedShield?.Attack ?? 0) +
                   (player.EquippedGloves?.Attack ?? 0) +
                   (player.EquippedTrousers?.Attack ?? 0) +
                   (player.EquippedBoots?.Attack ?? 0) +
                   (player.EquippedAmulet?.Attack ?? 0) +
                   (player.EquippedSwordOneHanded?.Attack ?? 0) +
                   (player.EquippedSwordTwoHanded?.Attack ?? 0);
        }
        public float CalculateDefense(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            return (player.EquippedHelmet?.Defense ?? 0) +
                   (player.EquippedArmor?.Defense ?? 0) +
                   (player.EquippedShield?.Defense ?? 0) +
                   (player.EquippedGloves?.Defense ?? 0) +
                   (player.EquippedTrousers?.Defense ?? 0) +
                   (player.EquippedBoots?.Defense ?? 0) +
                   (player.EquippedAmulet?.Defense ?? 0) +
                   (player.EquippedSwordOneHanded?.Defense ?? 0) +
                   (player.EquippedSwordTwoHanded?.Defense ?? 0);
        }
        public int CalculateWeight(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            return player.Inventory.Sum(item => item is HealthPotion potion
                ? potion.Weight * potion.Quantity
                : item.Weight);
        }
        public float ModifyStat(float currentModifier, float amount)
        {
            return Math.Max(0, currentModifier + amount);
        }
        public (int newExperience, int newLevel, int health) LevelUp(int experience, int level, int healthLimit)
        {
            int newLevel = level;
            while (experience >= newLevel * 100)
            {
                experience -= newLevel * 100;
                newLevel++;
            }
            return (experience, newLevel, healthLimit); // returns updated values
        }
    }
}