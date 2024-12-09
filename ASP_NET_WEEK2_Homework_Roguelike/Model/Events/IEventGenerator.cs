namespace ASP_NET_WEEK3_Homework_Roguelike.Model.Events
{
    public interface IEventGenerator
    {
        RandomEvent GenerateEvent(string eventStatus);
        RandomEvent GenerateRandomEvent();
    }
}
