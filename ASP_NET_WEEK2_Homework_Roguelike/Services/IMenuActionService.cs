using ASP_NET_WEEK3_Homework_Roguelike.Model;

namespace ASP_NET_WEEK3_Homework_Roguelike.Services
{
    public interface IMenuActionService
    {
        void AddNewAction(int id, string name, string menuName);
        List<MenuAction> GetMenuActionsByMenuName(string menuName);
    }
}