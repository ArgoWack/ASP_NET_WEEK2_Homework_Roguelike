using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike.ItemKinds
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
