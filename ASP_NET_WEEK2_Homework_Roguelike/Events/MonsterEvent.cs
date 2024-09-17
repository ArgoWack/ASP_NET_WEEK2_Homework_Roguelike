using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using ASP_NET_WEEK2_Homework_Roguelike.View;
using ASP_NET_WEEK2_Homework_Roguelike.ItemKinds;
using System;
using System.Linq;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class MonsterEvent : RandomEvent
    {
        private static readonly Random random = new Random();
        private static readonly Monster[] MonsterTemplates = new[]
        {
            new Monster("Goblin", 500, 300, 200, 1),
            new Monster("Orc", 1000, 500, 400, 2),
            new Monster("Troll", 1500, 700, 600, 3),
            new Monster("Dragon", 2500, 1000, 800, 5)
        };

        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            var monster = GenerateRandomMonster();
            controller.View.ShowEventEncounter(monster.Name);

            string choice;
            do
            {
                WriteLine("\nf. Fight \nh. Heal \nl. Leave/Flee");
                choice = ReadLine().ToLower();

                if (choice == "f")
                {
                    FightMonster(player, monster, controller);
                }
                else if (choice == "h")
                {
                    HealPlayer(player, controller);
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
                RewardPlayer(player, monster, controller);
                room.EventStatus = "none";
            }

            if (player.Health <= 0)
            {
                // When lost the game closes giving 5s prior to read message
                WriteLine("You have been defeated by the monster...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        private Monster GenerateRandomMonster()
        {
            return MonsterTemplates[random.Next(MonsterTemplates.Length)];
        }

        private void FightMonster(PlayerCharacter player, Monster monster, PlayerCharacterController controller)
        {
            int playerDamage = (int)Math.Max(player.Attack - monster.Defense, 0);
            int monsterDamage = (int)Math.Max(monster.Attack - player.Defense, 0);

            monster.Health -= playerDamage;
            player.Health -= monsterDamage;

            controller.View.ShowEventOutcome($"You dealt {playerDamage} damage to the {monster.Name}. It has {monster.Health} health remaining.");
            controller.View.ShowEventOutcome($"The {monster.Name} dealt {monsterDamage} damage to you. You have {player.Health} health remaining.");
        }

        private void HealPlayer(PlayerCharacter player, PlayerCharacterController controller)
        {
            var potion = player.Inventory.FirstOrDefault(i => i is HealthPotion);
            if (potion != null)
            {
                player.HealByPotion((HealthPotion)potion);
                controller.View.ShowEventOutcome("You used a health potion to heal.");
            }
            else
            {
                controller.View.ShowEventOutcome("You don't have any health potions!");
            }
        }

        private void RewardPlayer(PlayerCharacter player, Monster monster, PlayerCharacterController controller)
        {
            int experienceGained = monster.Level * 100;
            player.GetExperience(experienceGained);
            controller.View.ShowEventOutcome($"You gained {experienceGained} experience!");

            if (random.NextDouble() < 0.5)
            {
                var item = ItemFactory.GenerateItem<SwordOneHanded>();
                player.Inventory.Add(item);
                controller.View.ShowEventOutcome($"The {monster.Name} dropped a {item.Name}!");
            }

            int moneyDropped = monster.Level * 10;
            player.Money += moneyDropped;
            controller.View.ShowEventOutcome($"You found {moneyDropped} coins on the {monster.Name}.");
        }
    }
}
