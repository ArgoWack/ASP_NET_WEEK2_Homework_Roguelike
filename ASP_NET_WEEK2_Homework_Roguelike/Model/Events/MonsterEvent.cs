﻿using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using System;
using System.Linq;
using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Events
{
    public class MonsterEvent : RandomEvent
    {
        private static readonly Random random = new Random();
        private readonly CharacterInteractionService _interactionService;
        private readonly EventService _eventService;

        private static readonly Monster[] MonsterTemplates = new[]
        {
            new Monster("Goblin", 500, 300, 200, 1),
            new Monster("Orc", 1000, 500, 400, 2),
            new Monster("Troll", 1500, 700, 600, 3),
            new Monster("Dragon", 2500, 1000, 800, 5)
        };

        public MonsterEvent(EventService eventService, CharacterInteractionService interactionService)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
        }

        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            if (player == null || room == null)
                throw new ArgumentNullException("Player or Room cannot be null.");

            var monster = GenerateRandomMonster();
            _eventService.HandleEventOutcome($"You encounter a {monster.Name}");

            string choice;
            do
            {
                choice = _eventService.GetMonsterOptions();

                switch (choice)
                {
                    case "f":
                        FightMonster(player, monster);
                        break;
                    case "h":
                        player.HealByPotion();
                        break;
                    case "l":
                        _eventService.HandleEventOutcome("You flee from the monster.");
                        break;
                    default:
                        _eventService.HandleEventOutcome("Invalid choice.");
                        break;
                }

            } while (choice != "l" && monster.Health > 0 && player.Health > 0);

            if (monster.Health <= 0)
            {
                _eventService.HandleEventOutcome("You defeated the monster!");
                RewardPlayer(player, monster);
            }

            if (player.Health <= 0)
            {
                _eventService.HandleEventOutcome("You have been defeated by the monster...");

                Environment.Exit(0);
            }

            room.EventStatus = "none";
        }

        private Monster GenerateRandomMonster()
        {
            return MonsterTemplates[random.Next(MonsterTemplates.Length)];
        }

        private void FightMonster(PlayerCharacter player, Monster monster)
        {
            int playerDamage = (int)Math.Max(player.Attack - monster.Defense, 0);
            int monsterDamage = (int)Math.Max(monster.Attack - player.Defense, 0);

            // Ensure the player's damage does not reduce monster's health below 0
            monster.Health = Math.Max(monster.Health - playerDamage, 0);

            // Ensure the player's health does not drop below 0
            int damageToPlayer = Math.Min(monsterDamage, player.Health);
            player.Health = Math.Max(player.Health - damageToPlayer, 0);

            _eventService.HandleEventOutcome($"You dealt {playerDamage} damage to the {monster.Name}. It has {monster.Health} health remaining.");
            _eventService.HandleEventOutcome($"The {monster.Name} dealt {damageToPlayer} damage to you. You have {player.Health} health remaining.");

            // Check if the player is defeated
            if (player.Health == 0)
            {
                _eventService.HandleEventOutcome("You have been defeated by the monster...");
                Environment.Exit(0); // End the game
            }
        }

        private void RewardPlayer(PlayerCharacter player, Monster monster)
        {
            int experienceGained = monster.Level * 100;
            player.GetExperience(experienceGained);
            _eventService.HandleEventOutcome($"You gained {experienceGained} experience!");

            if (random.NextDouble() < 0.5)
            {
                var item = ItemFactoryService.GenerateRandomItem();

                if (item != null)
                {
                    player.Inventory.Add(item);
                    _eventService.HandleEventOutcome($"The {monster.Name} dropped a {item.Name}!");
                }
                else
                {
                    _eventService.HandleEventOutcome("The monster dropped nothing of value.");
                }
            }

            int moneyDropped = monster.Level * 10;
            player.Money += moneyDropped;
            _eventService.HandleEventOutcome($"You found {moneyDropped} coins on the {monster.Name}.");
        }
    }
}
