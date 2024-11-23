using ASP_NET_WEEK2_Homework_Roguelike.Model;

namespace ASP_NET_WEEK2_Homework_Roguelike.Services
{
    public class MenuActionService
    {
        private List<MenuAction> menuActions;
        public MenuActionService()
        {
            menuActions = new List<MenuAction>();
        }
        public void AddNewAction(int id, string name, string menuName)
        {
            MenuAction menuAction = new MenuAction() { Id = id, Name = name, MenuName = menuName };
            menuActions.Add(menuAction);
        }
        public List<MenuAction> GetMenuActionsByMenuName(string menuName)
        {
            List<MenuAction> result = new List<MenuAction>();
            foreach (var menuAction in menuActions)
            {
                if (menuAction.MenuName == menuName)
                {
                    result.Add(menuAction);
                }
            }
            return result;
        }
    }
}