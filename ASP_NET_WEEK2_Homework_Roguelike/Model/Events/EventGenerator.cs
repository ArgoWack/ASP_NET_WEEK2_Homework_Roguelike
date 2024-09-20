using System;
using ASP_NET_WEEK2_Homework_Roguelike.Services;
using ASP_NET_WEEK2_Homework_Roguelike.Model.Events;
namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Events
{
    public static class EventGenerator
    {
        private static EventService _eventService;
        private static CharacterInteractionService _interactionService;
        private static readonly Random random = new Random();

        // Method to initialize EventGenerator with necessary services
        public static void Initialize(EventService eventService, CharacterInteractionService interactionService)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
        }

        // Generates a random event based on chance
        public static RandomEvent GenerateEvent()
        {
            int roll = random.Next(100);
            if (roll < 25)
                return new FindItemEvent(_eventService, _interactionService);
            else if (roll < 75)
                return new MonsterEvent(_eventService, _interactionService);
            else if (roll < 85)
                return new DialogEvent(_eventService, _interactionService);
            else
                return null; // No event
        }

        // Generates an event based on specific status (for rooms with predefined events)
        public static RandomEvent GenerateEvent(string eventStatus)
        {
            return eventStatus switch
            {
                "FindItemEvent" => new FindItemEvent(_eventService, _interactionService),
                "MonsterEvent" => new MonsterEvent(_eventService, _interactionService),
                "DialogEvent" => new DialogEvent(_eventService, _interactionService),
                _ => null, // No event
            };
        }
    }
}