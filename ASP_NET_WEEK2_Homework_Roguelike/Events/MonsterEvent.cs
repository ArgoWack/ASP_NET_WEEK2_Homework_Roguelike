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
        public override void Execute(PlayerCharacter player, Room room)
        {
            // Implement monster encounter logic here
            WriteLine("A monster appears! Fight or flee? (f/l)");
            string choice = ReadLine();
            if (choice.ToLower() == "f")
            {
                //To be extended
                WriteLine("You fight the monster and win!");
                room.EventStatus = "none";
            }
            else
            {
                //To be extended
                WriteLine("You flee from the monster.");
            }
        }
    }
}
