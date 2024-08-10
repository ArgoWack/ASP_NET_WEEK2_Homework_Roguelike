using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public class DialogEvent : RandomEvent
    {
        public override void Execute(PlayerCharacter player, Room room)
        {
            // Implement dialog logic here
            WriteLine("You encounter a mysterious stranger who wants to talk.");
            WriteLine("You have a meaningful conversation and gain wisdom.");
            room.EventStatus = "none";
        }
    }
}
