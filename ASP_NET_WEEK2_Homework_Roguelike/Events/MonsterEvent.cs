using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class MonsterEvent : RandomEvent
    {
        private static readonly List<Monster> MonsterTemplates = new List<Monster>
    {
        new Monster("Goblin", 500, 300, 200, 1),
        new Monster("Orc", 1000, 500, 400, 2),
        new Monster("Troll", 1500, 700, 600, 3),
        new Monster("Dragon", 2500, 1000, 800, 5)
    };

        public override void Execute(PlayerCharacter player, Room room)
        {
            // Select a random monster template
            var random = new Random();
            var monsterTemplate = MonsterTemplates[random.Next(MonsterTemplates.Count)];
            var monster = new Monster(monsterTemplate.Name, monsterTemplate.Health, monsterTemplate.Attack, monsterTemplate.Defense, monsterTemplate.Level);

            WriteLine($"A {monster.Name} appears with {monster.Health} health, {monster.Attack} attack, and {monster.Defense} defense! What do you want to do?");
            WriteLine("\nf. Fight \nh. Heal \nl. Leave/Flee");

            string choice;
            do
            {
                choice = ReadLine().ToLower();
                if (choice == "f")
                {
                    FightMonster(player, monster, room);
                }
                else if (choice == "h")
                {
                    HealPlayer(player);
                }
                else if (choice == "l")
                {
                    WriteLine("You flee from the monster.");
                    break;
                }
                else
                {
                    WriteLine("Invalid choice. Please choose 'f', 'h', or 'l'.");
                }

            } while (choice != "l" && monster.Health > 0 && player.Health > 0);

            if (monster.Health <= 0)
            {
                WriteLine("You defeated the monster!");
                RewardPlayer(player, monster);
                room.EventStatus = "none";
            }

            if (player.Health <= 0)
            {
                // when lost the game closes giving 5s prior to read message
                WriteLine("You have been defeated by the monster...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        private void FightMonster(PlayerCharacter player, Monster monster, Room room)
        {
            int playerDamage = (int)Math.Max(player.Attack - monster.Defense, 0);
            int monsterDamage = (int)Math.Max(monster.Attack - player.Defense, 0);

            monster.Health -= playerDamage;
            player.Health -= monsterDamage;

            WriteLine($"You dealt {playerDamage} damage to the {monster.Name}. It has {monster.Health} health remaining.");
            WriteLine($"The {monster.Name} dealt {monsterDamage} damage to you. You have {player.Health} health remaining.");
        }

        private void HealPlayer(PlayerCharacter player)
        {
            var potion = player.Inventory.FirstOrDefault(i => i is HealthPotion);
            if (potion != null)
            {
                player.HealByPotion((HealthPotion)potion);
                WriteLine("You used a health potion to heal.");
            }
            else
            {
                WriteLine("You don't have any health potions!");
            }
        }

        private void RewardPlayer(PlayerCharacter player, Monster monster)
        {
            int experienceGained = monster.Level * 100;
            player.GetExperience(experienceGained);
            WriteLine($"You gained {experienceGained} experience!");

            // Random loot drop logic
            var random = new Random();
            if (random.NextDouble() < 0.5) // 50% chance to drop an item
            {
                var itemType = ItemStats.BaseStats.Keys.ElementAt(random.Next(ItemStats.BaseStats.Count));
                var baseStats = ItemStats.BaseStats[itemType];

                int weight = (int)(baseStats.Weight * (random.NextDouble() * 0.4 + 0.8));
                int defense = (int)(baseStats.Defense * (random.NextDouble() * 0.4 + 0.8));
                int attack = (int)(baseStats.Attack * (random.NextDouble() * 0.4 + 0.8));
                int moneyWorth = (int)(baseStats.MoneyWorth * (random.NextDouble() * 0.4 + 0.8));

                var lootItem = ItemFactory.GenerateItem(itemType, weight, defense, attack, moneyWorth);
                player.Inventory.Add(lootItem);
                WriteLine($"The {monster.Name} dropped a {lootItem.Name}!");
            }

            // Random money drop
            int moneyDropped = (int)(monster.Level * 10 * (random.NextDouble() * 0.4 + 0.8));
            player.Money += moneyDropped;
            WriteLine($"You found {moneyDropped} coins on the {monster.Name}.");
        }
    }
}
