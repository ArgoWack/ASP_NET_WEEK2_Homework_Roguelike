using ASP_NET_WEEK3_Homework_Roguelike.Model;
using ASP_NET_WEEK3_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public class CharacterStatsService
    {
        public float CalculateAttack(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Player cannot be null.");

            float totalAttack = 0;

            if (player.EquippedItems != null)
            {
                foreach (var item in player.EquippedItems.Values)
                {
                    if (item != null)
                    {
                        totalAttack += item.Attack;
                    }
                }
            }
            return totalAttack;
        }
        public float CalculateDefense(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Player cannot be null.");

            float totalDefense = 0;

            if (player.EquippedItems != null)
            {
                foreach (var item in player.EquippedItems.Values)
                {
                    if (item != null)
                    {
                        totalDefense += item.Defense;
                    }
                }
            }
            return totalDefense;
        }
        public int CalculateWeight(PlayerCharacter player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Player cannot be null.");

            int totalWeight = 0;

            // Calculate weight from inventory items
            if (player.Inventory != null)
            {
                totalWeight += player.Inventory.Sum(item =>
                    item is HealthPotion potion
                        ? potion.Weight * potion.Quantity // multiplies weight by quantity for stackable items
                        : item?.Weight ?? 0 // Safely handle null items
                );
            }

            // Calculate weight from equipped items
            if (player.EquippedItems != null)
            {
                totalWeight += player.EquippedItems.Values
                    .Where(item => item != null) // Filter out null items
                    .Sum(item => item.Weight);
            }
            return totalWeight;
        }
    }
}