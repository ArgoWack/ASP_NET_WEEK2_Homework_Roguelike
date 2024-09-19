using ASP_NET_WEEK2_Homework_Roguelike.Events;
using System;

namespace ASP_NET_WEEK2_Homework_Roguelike
{
    public static class EventGenerator
    {
        private static Random random = new Random();

        // Generates a random event based on chance
        public static RandomEvent GenerateEvent()
        {
            int roll = random.Next(100);
            if (roll < 25)
                return new FindItemEvent();
            else if (roll < 75)
                return new MonsterEvent();
            else if (roll < 85)
                return new DialogEvent();
            else
                return null; // No event
        }

        // Generates an event based on specific status (for rooms with predefined events)
        public static RandomEvent GenerateEvent(string eventStatus)
        {
            return eventStatus switch
            {
                "FindItemEvent" => new FindItemEvent(),
                "MonsterEvent" => new MonsterEvent(),
                "DialogEvent" => new DialogEvent(),
                _ => null, // No event
            };
        }
    }
}