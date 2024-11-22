using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Items;
using System;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Events
{
    public class FindItemEvent : RandomEvent
    {
        private readonly CharacterInteractionService _interactionService;
        private readonly EventService _eventService;

        public FindItemEvent(EventService eventService, CharacterInteractionService interactionService)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
        }

        public override void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller)
        {
            if (player == null || room == null)
                throw new ArgumentNullException("Player or Room cannot be null.");


            Random random = new Random();
            int chances = random.Next(0, 100);

            if(chances<34)
            {
                // adds healthpotion(s)

                if (chances < 10)
                {
                    int foundPotions = new Random().Next(1, 4); // Randomly find 1-3 potions
                    for (int i = 0; i < foundPotions; i++)
                    {
                        player.ReceiveHealthPotion();
                    }
                    _eventService.HandleEventOutcome($"You have found {foundPotions} Health Potions.");
                }
                if (chances >= 10 && chances < 20)
                {                    
                    player.ReceiveHealthPotion(2);
                    _eventService.HandleEventOutcome($"You have found 2 HealthPotions");
                }
                if (chances >= 20 && chances < 30)
                {                    
                    player.ReceiveHealthPotion(3);

                    _eventService.HandleEventOutcome($"You have found 3 HealthPotions");
                }
                if (chances >= 30)
                {
                    player.ReceiveHealthPotion(4);

                    _eventService.HandleEventOutcome($"You are lucky, you have found 4 HealthPotions");
                }
            }
            else
            {
                // generates random item other than healthpotion
                var item = ItemFactoryService.GenerateRandomItem();

                if (item != null)
                {
                    _eventService.HandleEventOutcome($"You have found an item: {item.Name}.");
                    string choice = _eventService.PromptForItemPickup();

                    if (choice.ToLower() == "y")
                    {
                        player.Inventory.Add(item);
                        _eventService.HandleEventOutcome($"You have taken the item: {item.Name}.");
                    }
                    else
                    {
                        _eventService.HandleEventOutcome($"You have left the item: {item.Name}.");
                    }
                }
                else
                {
                    _eventService.HandleEventOutcome("No valid item generated.");
                }
            }
            room.EventStatus = "none";
        }
    }
}