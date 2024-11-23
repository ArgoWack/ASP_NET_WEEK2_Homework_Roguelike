using ASP_NET_WEEK2_Homework_Roguelike.Controller;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Events
{
    public abstract class RandomEvent
    {
        public abstract void Execute(PlayerCharacter player, Room room, PlayerCharacterController controller);
    }
}
