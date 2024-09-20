using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_NET_WEEK2_Homework_Roguelike.Model
{
    public class Room
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string EventStatus { get; set; }
        public Dictionary<string, Room> Exits { get; set; }
        public bool IsExplored { get; set; }

        public Room(int x, int y)
        {
            X = x;
            Y = y;
            Exits = new Dictionary<string, Room>();
            IsExplored = false;
            EventStatus = "none";
        }
        //For serialization
        public Room() { }
    }
}
