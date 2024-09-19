using ASP_NET_WEEK2_Homework_Roguelike.Controller;
using ASP_NET_WEEK2_Homework_Roguelike.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike.Events
{
    public abstract class RandomEvent
    {
        public abstract void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller);
    }
}
