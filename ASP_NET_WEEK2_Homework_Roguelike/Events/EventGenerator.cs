using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public static class EventGenerator
    {
        private static Random random = new Random();

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
    }
}
