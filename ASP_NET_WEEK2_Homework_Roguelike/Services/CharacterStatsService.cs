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

            float totalAttack = 0;

            // Collect all equipped items
            var equippedItems = new List<Item>
            {
                player.EquippedHelmet,
                player.EquippedArmor,
                player.EquippedShield,
                player.EquippedGloves,
                player.EquippedTrousers,
                player.EquippedBoots,
                player.EquippedAmulet,
                player.EquippedSwordOneHanded,
                player.EquippedSwordTwoHanded
            };

            // Calculate total attack from equipped items
            foreach (var item in equippedItems)
            {
                if (item != null)
                    totalAttack += item.Attack;
            }
            return totalAttack;
        }
        public float CalculateDefense(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            float totalDefense = 0;

            // Collect all equipped items
            var equippedItems = new List<Item>
            {
                player.EquippedHelmet,
                player.EquippedArmor,
                player.EquippedShield,
                player.EquippedGloves,
                player.EquippedTrousers,
                player.EquippedBoots,
                player.EquippedAmulet,
                player.EquippedSwordOneHanded,
                player.EquippedSwordTwoHanded
            };

            // Calculate total defense from equipped items
            foreach (var item in equippedItems)
            {
                if (item != null)
                    totalDefense += item.Defense;
            }
            return totalDefense;
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