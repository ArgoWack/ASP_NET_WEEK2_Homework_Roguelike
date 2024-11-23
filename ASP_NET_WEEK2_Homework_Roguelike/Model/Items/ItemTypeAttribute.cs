
namespace ASP_NET_WEEK2_Homework_Roguelike.Model.Items
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ItemTypeAttribute : Attribute
    {
        public string Kind { get; }
        public ItemTypeAttribute(string kind)
        {
            Kind = kind;
        }
    }
}