using ASP_NET_WEEK3_Homework_Roguelike.Services;
using ASP_NET_WEEK3_Homework_Roguelike.Controller;
using ASP_NET_WEEK3_Homework_Roguelike.View;

namespace ASP_NET_WEEK3_Homework_Roguelike.Model.Events
{
    public class MonsterEvent : RandomEvent
    {
        private static readonly Random random = new Random();
        private readonly CharacterInteractionService _interactionService;
        private readonly EventService _eventService;
        private readonly PlayerCharacterView _view;
        private static readonly Monster[] MonsterTemplates = new[]
        {
            new Monster("Goblin", 500, 300, 200, 1),
            new Monster("Orc", 1000, 500, 400, 2),
            new Monster("Troll", 1500, 700, 600, 3),
            new Monster("Dragon", 2500, 1000, 800, 5)
        };
        public MonsterEvent(EventService eventService, CharacterInteractionService interactionService, PlayerCharacterView view)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }
        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            if (player == null || room == null)
                throw new ArgumentNullException("Player or Room cannot be null.");

            // Prevent double execution
            if (room.EventStatus == "handled")
                return;
            try
            {
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
                    Environment.Exit(0); // Ends the game
                }
            }
            catch (Exception ex)
            {
                _eventService.HandleEventOutcome($"Error during MonsterEvent: {ex.Message}");
            }
            finally
            {
                // Mark the event as handled regardless of the outcome
                room.EventStatus = "handled"; 
            }
        }
        private Monster GenerateRandomMonster()
        {
            return MonsterTemplates[random.Next(MonsterTemplates.Length)];
        }
        private void FightMonster(PlayerCharacter player, Monster monster)
        {
            int playerDamage = Math.Max((int)(player.Attack - monster.Defense), 0);
            int monsterDamage = Math.Max((int)(monster.Attack - player.Defense), 0);
            monster.Health = Math.Max(monster.Health - playerDamage, 0);
            player.Health = Math.Max(player.Health - monsterDamage, 0);
            _eventService.HandleEventOutcome($"You dealt {playerDamage} damage to the {monster.Name}. It has {monster.Health} health remaining.");
            _eventService.HandleEventOutcome($"The {monster.Name} dealt {monsterDamage} damage to you. You have {player.Health} health remaining.");
        }
        private void RewardPlayer(PlayerCharacter player, Monster monster)
        {
            int experienceGained = monster.Level * 100;
            player.GetExperience(experienceGained);
            _eventService.HandleEventOutcome($"You gained {experienceGained} experience!");
            if (random.NextDouble() < 0.5)
            {
                var item = ItemFactoryService.GenerateRandomItem(_view);
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