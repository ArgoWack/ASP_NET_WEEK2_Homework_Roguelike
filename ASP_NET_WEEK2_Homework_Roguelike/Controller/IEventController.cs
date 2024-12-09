using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Controller
{
    public interface IEventController
    {
        void ExecuteEvent(Room room);
    }
}